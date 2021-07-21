using Microsoft.AspNetCore.Mvc;
using StarrAPI.AutoMapperHelp;

namespace StarrAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [ServiceFilter(typeof(LogUserActivity))]
    public class BaseAPIController:ControllerBase
    {
        
    }
}