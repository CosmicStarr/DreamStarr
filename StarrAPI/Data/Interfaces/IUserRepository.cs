using System.Collections.Generic;
using System.Threading.Tasks;
using StarrAPI.AutoMapperHelp;
using StarrAPI.DTOs;
using StarrAPI.Models;

namespace StarrAPI.Data.Interfaces
{
    public interface IUserRepository
    {
        void UpdateUser(AppUser User);
        Task<bool> SaveAllAsync(); 
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int Id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<PagerList<MemberDTO>> GetMembersDTOAsync(UserParams userParams);
        Task<MemberDTO> GetMemberDTOAsync(string username);
    }
}