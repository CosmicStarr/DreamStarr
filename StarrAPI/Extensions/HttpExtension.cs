using System.Text.Json;
using Microsoft.AspNetCore.Http;
using StarrAPI.AutoMapperHelp;

namespace StarrAPI.Extensions
{
    public static class HttpExtension
    {
        public static void AddPaginationHeader(this HttpResponse response,int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var pagingHeader = new PagerHeader(currentPage,itemsPerPage,totalItems,totalPages);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            response.Headers.Add("pagination", JsonSerializer.Serialize(pagingHeader,options));
            response.Headers.Add("access-control-expose-headers","pagination");
        }
        
    }
}