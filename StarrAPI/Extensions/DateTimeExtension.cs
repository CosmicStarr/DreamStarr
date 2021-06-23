
using System;

namespace StarrAPI.Extensions
{
    public static class DateTimeExtension
    {
        public static int CalulateAge(this DateTime DOB)
        {
            var Today = DateTime.Today;
            var Age = Today.Year - DOB.Year;
            if(DOB.Date > Today.AddYears(-Age)) Age--;
            return Age;
        }
    }
}