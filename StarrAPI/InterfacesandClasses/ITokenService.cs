

using StarrAPI.Models;

namespace StarrAPI.InterfacesandClasses
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}