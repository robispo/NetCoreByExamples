using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.WepApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.WepApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly DataContext _context;

        public BaseController(DataContext context)
        {
            this._context = context;
        }
    }
}