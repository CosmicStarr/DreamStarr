using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarrAPI.AutoMapperHelp;
using StarrAPI.Data.Interfaces;
using StarrAPI.DTOs;
using StarrAPI.Extensions;
using StarrAPI.Models;

namespace StarrAPI.Data.Repositories
{
    public class LikeRepository : ILikesRepository
    {
        private readonly ApplicationDbContext _Context;
        public LikeRepository(ApplicationDbContext Context)
        {
            _Context = Context;

        }
        public async Task<UserLikes> GetUserLikes(int SourceId, int LikeUserId)
        {
            return await _Context.Likes.FindAsync(SourceId,LikeUserId);
        }

        public async Task<PagerList<LikeDTO>> GetUserLikes(LikeParams likeParams)
        {
           var Users = _Context.Users.OrderBy(u => u.UserName).AsQueryable();
           var Likes = _Context.Likes.AsQueryable();
           if(likeParams.Predicate == "Liked")
           {
               /*retrieving a list of users that the Current User Likes.
               The userId needs to match the Id that was retrieved in the database*/
               Likes = Likes.Where(z => z.SourceUserId == likeParams.UserId);
               Users = Likes.Select(z =>z.LikedUser);
           }
           if(likeParams.Predicate == "LikedBy")
           {
               /*retrieving a list of users that liked the current user. their like
               Id needs to match the current user Id */
               Likes = Likes.Where(z => z.LikedUserId == likeParams.UserId);
               Users = Likes.Select(z =>z.SourceUser);
           }

           var LikedUsers = Users.Select(a => new LikeDTO
           {
               Username = a.UserName,
               AlsoKnownAs = a.AlsoKnownAs,
               Age = a.DateOfBirth.CalulateAge(),
               PhotoUrl = a.Photos.FirstOrDefault(p =>p.MainPic).PhotoUrl,
               City = a.City,
               Id = a.Id
           });
           return await PagerList<LikeDTO>.CreateAsync(LikedUsers,likeParams.PageNumber,likeParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _Context.Users.Include(x => x.LikedUsers)
            .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
