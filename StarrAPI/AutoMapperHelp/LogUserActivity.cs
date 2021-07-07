using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using StarrAPI.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using StarrAPI.Extensions;
using System;

namespace StarrAPI.AutoMapperHelp
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var results = await next();
            if(!results.HttpContext.User.Identity.IsAuthenticated) return;
            var UserId = results.HttpContext.User.GetUserId();
            var UserRepo = results.HttpContext.RequestServices.GetService<IUserRepository>();
            var user = await UserRepo.GetUserByIdAsync(UserId);
            user.LastActive = DateTime.Now;
            await UserRepo.SaveAllAsync();

        }
    }
}