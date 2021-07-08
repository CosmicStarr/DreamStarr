using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarrAPI.AutoMapperHelp;
using StarrAPI.Data.Interfaces;
using StarrAPI.DTOs;
using StarrAPI.Extensions;
using StarrAPI.Models;

namespace StarrAPI.Controllers
{
    [Authorize]
    public class LikesController : BaseAPIController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;

        }

        [HttpPost("{Username}")]
        public async Task<ActionResult> AddLikes(string Username)
        {
            //Retrieving the Current User
            var sourceUserId = User.GetUserId();
            //Retrieving the Liked Users
            var likedUser = await _userRepository.GetUserByUsernameAsync(Username);
            //Retrieving the Current User with their likes.
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if(likedUser == null) return NotFound();
            if(sourceUser.Username == Username) return BadRequest("Liking yourself is not allowed!");

            var userlike = await _likesRepository.GetUserLikes(sourceUserId,likedUser.UserId);

            if(userlike != null) return BadRequest("You cannot like someone twice!");

            userlike = new UserLikes
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.UserId
            };

            sourceUser.LikedUsers.Add(userlike);

            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to Like User!");
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDTO>>> GetUserLikes([FromQuery]LikeParams likeParams)
        {
            likeParams.UserId = User.GetUserId();
            var Users = await _likesRepository.GetUserLikes(likeParams);
            Response.AddPaginationHeader(Users.CurrentPage, Users.PageSize,Users.TotalCount,Users.TotalPages);
            return Ok(Users);
        }
    }
}