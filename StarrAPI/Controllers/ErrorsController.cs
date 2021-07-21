using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarrAPI.Data;
using StarrAPI.Models;

namespace StarrAPI.Controllers
{
    public class ErrorsController : BaseAPIController
    {
        private readonly ApplicationDbContext _dbContext;
        public ErrorsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpGet("auth") ]
        [Authorize]
        public ActionResult<string> GetSecret()
        {
            return "Woonderful! You're in.";
        } 

        [HttpGet("Not-Found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing =_dbContext.Users.Find(-1);
            if(thing == null)
            return NotFound(thing);   
            return Ok("Wow!");
        } 

        [HttpGet("Server-Error") ]
        public ActionResult<string> GetServerError()
        {
            var thing =_dbContext.Users.Find(-1);
            var thingtoreturn = thing.ToString();
            return thingtoreturn;
        }


        [HttpGet("Bad-Request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("Wrong Turn");
        } 

         
    }
}