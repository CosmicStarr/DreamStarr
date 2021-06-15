using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarrAPI.Data;
using StarrAPI.Models;

namespace StarrAPI.Controllers
{

    public class UsersController : BaseAPIController
    {

        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
              return await _context.GetAppUsers.ToListAsync();
        }

        [HttpGet("{Id}")]
        [Authorize]
        public async Task<ActionResult<AppUser>> GetUser(int Id)
        {
            return await _context.GetAppUsers.FirstOrDefaultAsync(u => u.UserId == Id);
            
        }
    }
}