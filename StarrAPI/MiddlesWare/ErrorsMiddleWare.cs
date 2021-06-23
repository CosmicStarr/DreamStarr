using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StarrAPI.Errors;

namespace StarrAPI.MiddlesWare
{
    public class ErrorsMiddleWare
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<ErrorsMiddleWare> _Logger;
        private readonly IHostEnvironment _env;
        public ErrorsMiddleWare(RequestDelegate Next, ILogger<ErrorsMiddleWare> Logger, IHostEnvironment env)
        {
            _Logger = Logger;
            _Next = Next;
            _env = env;
        }
 
        public async Task InvokeAsync(HttpContext Context)
        {
            try
            {
                await _Next(Context);
            }
            catch (Exception EX)
            {
               _Logger.LogError(EX, EX.Message);
               Context.Response.ContentType = "application/json";
               Context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

               var response = _env.IsDevelopment()
               ? new ApiErrors(Context.Response.StatusCode, EX.Message,EX.StackTrace?.ToString())
               : new ApiErrors(Context.Response.StatusCode);
                var Options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var Json = JsonSerializer.Serialize(response,Options);
                await Context.Response.WriteAsync(Json);
            }
        }
    }
}