using System;

namespace StarrAPI.Errors
{
    public class ApiErrors
    {
        public ApiErrors(int statusCode, string message = null, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }     
        public string Message { get; set; }     
        public String Details { get; set; }     
    }
}