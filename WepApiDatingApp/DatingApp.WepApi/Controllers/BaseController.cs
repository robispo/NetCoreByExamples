using DatingApp.WepApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.WepApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class BaseController : Controller
    {
        protected readonly DataContext _context;

        public BaseController(DataContext context)
        {
            this._context = context;
        }
    }
}