using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarrAPI.Models;

namespace StarrAPI.Controllers
{
    public class AdminController : BaseAPIController
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> UserWithRoles()
        {
            var users = await _userManager.Users
            .Include(r => r.ListOfAppUserRole)
            .ThenInclude(r => r.AppRole)
            .OrderBy(u => u.UserName)
            .Select(u => new
            {
                u.Id,
                Username = u.UserName,
                Roles =u.ListOfAppUserRole.Select(u => u.AppRole.Name).ToList()
            })
            .ToListAsync();

            return Ok(users);
        }

        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username,[FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();
            //if(selectedRoles == null) return BadRequest($"{selectedRoles}! Does not exist");
            var user = await _userManager.FindByNameAsync(username);
            if(user == null) return NotFound("Could not find user!");
            var userRoles = await _userManager.GetRolesAsync(user);
            var results = await _userManager.AddToRolesAsync(user,selectedRoles.Except(userRoles));
            if(!results.Succeeded) BadRequest("Failed to add to Roles!");
            results = await _userManager.RemoveFromRolesAsync(user,userRoles.Except(selectedRoles));
            if(!results.Succeeded) BadRequest("Faied to remove from Roles!");
            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ManagePhotoRole")]
        [HttpGet("Photos-To-Manage")]
        public IActionResult ManagePhotos()
        {
            return Ok("Only Admins and Managers can see this!");
        }
    }
}