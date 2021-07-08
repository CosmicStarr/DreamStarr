using System.Collections.Generic;
using System.Threading.Tasks;
using StarrAPI.AutoMapperHelp;
using StarrAPI.DTOs;
using StarrAPI.Models;

namespace StarrAPI.Data.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLikes> GetUserLikes(int SourceId, int LikeUserId);
        Task<AppUser> GetUserWithLikes(int UserId);
        Task<PagerList<LikeDTO>> GetUserLikes(LikeParams likeParams);

    }
}