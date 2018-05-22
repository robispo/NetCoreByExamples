using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.WepApi.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int GetAge(this DateTime dateTime)
        {
            var age = DateTime.Today.Year - dateTime.Year;

            if (dateTime.AddYears(age) > DateTime.Today)
                age--;

            return
                age;
        }
    }
}
