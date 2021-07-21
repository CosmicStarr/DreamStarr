

using System.Threading.Tasks;
using StarrAPI.Models;

namespace StarrAPI.InterfacesandClasses
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser appUser);
    }
}