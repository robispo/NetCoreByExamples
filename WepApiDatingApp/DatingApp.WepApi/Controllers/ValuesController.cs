using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.WepApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.WepApi.Controllers
{
    public class ValuesController : BaseController
    {
        public ValuesController(DataContext context) : base(context) { }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return
                Ok(await _context.Values.ToArrayAsync());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return
                 Ok(await _context.Values.FirstOrDefaultAsync(v => v.Id == id));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
