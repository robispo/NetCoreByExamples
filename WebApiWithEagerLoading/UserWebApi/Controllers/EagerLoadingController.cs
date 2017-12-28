using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserWebApi.Models;
using UserWebApi.Services;

namespace UserWebApi.Controllers
{
    public class EagerLoadingController : BaseController
    {
        public EagerLoadingController(DataBaseELContext dataBaseELContext, IJwtService jwtService) : base(dataBaseELContext, jwtService) { }

        [HttpGet("Index")]
        public IActionResult Index(string entity, string id, string with)
        {
            object result;
            string[] withParse;

            try
            {
                withParse = this.ParseWith(with);
                switch (entity)
                {
                    case "users":
                        IQueryable<UserEntity> dbSet;
                        dbSet = _dataBaseELContext.UserEntities;

                        for (int i = 0; i < withParse.Length; i++)
                            dbSet = dbSet.Include(withParse[i]);

                        if (!string.IsNullOrWhiteSpace(id))
                            result = dbSet.FirstOrDefault(u => u.UserLogin == id);
                        else
                            result = dbSet.Select(u => u).ToArray();
                        break;
                    default:
                        result = null;
                        break;
                }

                return
                   Ok(result);
            }
            catch (Exception ex)
            {
                return
                    BadRequest(new { ex.Message });
            }
        }

        private string[] ParseWith(string with)
        {
            string[] includes, thenincludes;
            CultureInfo cultureInfo;
            TextInfo textInfo;

            cultureInfo = Thread.CurrentThread.CurrentCulture;
            textInfo = cultureInfo.TextInfo;

            includes = with.Split(",");

            for (int i = 0; i < includes.Length; i++)
            {
                thenincludes = includes[i].Split(".");

                for (int j = 0; j < thenincludes.Length; j++)
                    thenincludes[j] = textInfo.ToTitleCase(thenincludes[j]);

                includes[i] = string.Join(".", thenincludes);
            }

            return
                includes;
        }
    }
}