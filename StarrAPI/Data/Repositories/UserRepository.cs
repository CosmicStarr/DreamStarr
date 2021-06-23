using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StarrAPI.Data.Interfaces;
using StarrAPI.DTOs;
using StarrAPI.Models;

namespace StarrAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _Context;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext Context, IMapper mapper)
        {
            _mapper = mapper;
            _Context = Context;

        }

        public async Task<MemberDTO> GetMemberDTOAsync(string username)
        {
             return await _Context.GetAppUsers
            .Where(x => x.Username == username)
            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersDTOAsync()
        {
            return await _Context.GetAppUsers
            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int Id)
        {
            return await _Context.GetAppUsers.FirstOrDefaultAsync(i => i.UserId == Id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _Context.GetAppUsers
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _Context.GetAppUsers
            .Include(p => p.Photos)
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _Context.SaveChangesAsync() > 0;
        }

        public void UpdateUser(AppUser User)
        {
            _Context.Entry(User).State = EntityState.Modified;
        }

    }
}