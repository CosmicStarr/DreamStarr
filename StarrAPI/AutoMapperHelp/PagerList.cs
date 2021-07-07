using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StarrAPI.AutoMapperHelp
{
    public class PagerList<T>:List<T>
    {
        public PagerList(IEnumerable<T> Items, int Count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(Count/(double) pageSize);
            PageSize = pageSize;
            TotalCount = Count;
            AddRange(Items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PagerList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber -1) * pageSize).Take(pageSize).ToListAsync();
            return new PagerList<T>(items,count,pageNumber,pageSize);
        }
        
    }
}