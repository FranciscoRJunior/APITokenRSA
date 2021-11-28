    using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APITokenTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SuzanoController : ControllerBase
    {
        // GET: api/<SuzanoController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /*[HttpGet("/SendJson/{json}")]
        //public string SendJson(string json)
        //{
        //    string resp = "";
        //    WebRequest request = WebRequest.Create("https://apiclientesh.webtraining.com.br/api/RegistraParticipacaoSimulador");
        //    request.Method = "POST";

        //    byte[] byteArray = Encoding.UTF8.GetBytes(json);

        //    request.ContentType = "application/json";
        //    request.ContentLength = byteArray.Length;
        //    request.Headers.Add("User-Token", "A1CDB122-033C-4E85-8949-C6F706C72883");

        //    Stream dataStream = request.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();

        //    WebResponse response = request.GetResponse();
        //    Console.WriteLine(((HttpWebResponse)response).StatusDescription);

        //    using (dataStream = response.GetResponseStream())
        //    {
        //        StreamReader reader = new StreamReader(dataStream);
        //        resp = reader.ReadToEnd();

        //        Console.WriteLine(resp);
        //    }

        //    // Close the response.
        //    response.Close();
        //    return resp;
        //}
        */

        //adicionado 28/11
        [HttpGet("/SendJson/{json}")]
        public string SendJson(string json)
        {
            return this.Post(json);
        }

        // GET api/<SuzanoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SuzanoController>
        [HttpPost]
        public string Post(string value)
        {
            string resp = "";
            WebRequest request = WebRequest.Create("https://apiclientesh.webtraining.com.br/api/RegistraParticipacaoSimulador");
            request.Method = "POST";

            byte[] byteArray = Encoding.UTF8.GetBytes(value);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            request.Headers.Add("User-Token", "A1CDB122-033C-4E85-8949-C6F706C72883");

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                resp = reader.ReadToEnd();

                Console.WriteLine(resp);
            }

            // Close the response.
            response.Close();
            return resp;
        }

        // PUT api/<SuzanoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SuzanoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
