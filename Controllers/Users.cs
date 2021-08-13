using loginVS2.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APITokenTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {
        public readonly List<LoginModel> userList = new List<LoginModel>
        {
            new LoginModel{username="bsato",password="1234"},
            new LoginModel{username="fribeiro",password="1234"},
        };
        // GET: api/<Users>
        [HttpGet]
        public IEnumerable<LoginModel> Get()
        {
            return userList;
        }

        // GET api/<Users>/5
        [HttpGet("{username},{password}")]
        public LoginModel Get(string username,string password)
        {
            return userList.Where(x => x.username == username && x.password == password).Single();
        }

        // POST api/<Users>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Users>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Users>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
