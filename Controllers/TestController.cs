using APITokenTest.Configurations;
using loginVS2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APITokenTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly SecurityOptions options;

        public TestController(SecurityOptions options)
        {
            this.options = options;
        }

        //Metodo para retornar o token usando a chave privada
        [HttpGet(nameof(GetPrivateToken))]
        private async Task<IActionResult> GetPrivateToken()
        {
            //Instanciando o metodo create do RSA
            var rsa = RSA.Create();

            //Lendo Informações da chave privada (/Keys/PrivateKey.xml)
            string key = await System.IO.File.ReadAllTextAsync(options.PrivateKeyFilePath);
            
            //Passando a chave para a instancia do RSA
            rsa.FromXmlString(key);

            //Criando nossas credenciais de acesso usando o algoritimo RSA
            var credentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            var jwt = new JwtSecurityToken(
                new JwtHeader(credentials),
                new JwtPayload(
                    "webapi",
                    "webapi",
                    new List<Claim> { new Claim("id","1"), new Claim("name", "Francisco"), },
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMinutes(5)
                    )
                );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(new { Token = token});
        }


        //Metodo para retornar o token usando a chave Publica
        [HttpGet(nameof(GetPublicToken))]
        public async Task<IActionResult> GetPublicToken()
        {
            //Instanciando o metodo create do RSA
            var rsa = RSA.Create();
            await GetPrivateToken();

            //Lendo Informações da chave privada (/Keys/PrivateKey.xml)
            string key = await System.IO.File.ReadAllTextAsync(options.PublicKeyFilePath);

            //Passando a chave para a instancia do RSA
            rsa.FromXmlString(key);

            //Criando nossas credenciais de acesso usando o algoritimo RSA
            var credentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            var jwt = new JwtSecurityToken(
                new JwtHeader(credentials),
                new JwtPayload(
                    "webapi",
                    "webapi",
                    new List<Claim>(),
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMinutes(5)
                    )
                );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(new { Token = token });
        }

        //Metodo para Teste
        [HttpPost]
        public async Task<IActionResult> PostPublicToken(string username, string password)
        {
            //if (user == null) { return BadRequest(); }

            if (username.Equals("bsato") && password.Equals("1234"))
            {
                //Instanciando o metodo create do RSA
                var rsa = RSA.Create();

                //Lendo Informações da chave privada (/Keys/PrivateKey.xml)
                string key = await System.IO.File.ReadAllTextAsync(options.PublicKeyFilePath);

                //Passando a chave para a instancia do RSA
                rsa.FromXmlString(key);

                //Criando nossas credenciais de acesso usando o algoritimo RSA
                var credentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

                var jwt = new JwtSecurityToken(
                    new JwtHeader(credentials),
                    new JwtPayload(
                        "webapi",
                        "webapi",
                        new List<Claim>(),
                        DateTime.UtcNow,
                        DateTime.UtcNow.AddMinutes(5)
                        )
                    );

                string token = new JwtSecurityTokenHandler().WriteToken(jwt);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
     

        //Metodo para retornar o token usando uma chave Simetrica
        [HttpGet(nameof(GetSymetricToken))]
        public async Task<IActionResult> GetSymetricToken()
        {
            //Puxando a chave simetrica que esta no appsettings.json
            string key = options.SymmetricKey;
            //Criando nossas credenciais de acesso usando o algoritimo HMAC
            var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                new JwtHeader(credentials),
                new JwtPayload(
                    "webapi",
                    "webapi",
                    new List<Claim>(),
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMinutes(5)
                    )
                );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(new { Token = token });
        }



        // Função para verificar se o Token está funcionando 
        // - > No postman defina na autorização o token retornado
        // - > Retornando valor 200 o token foi um sucesso!
        // - > Retornando valor 401 ou 403 o token não funcionou ou está expirado
        [Authorize]
        [HttpGet(nameof(VerifyToken))]
        public async Task<IActionResult> VerifyToken()
        {
            return Ok(new { Result = "O Token está Funcionando!" });
        }

        [Authorize(AuthenticationSchemes = "public")]
        [HttpGet(nameof(VerifyTokenPublic))]
        public async Task<IActionResult> VerifyTokenPublic()
        {
            return Ok(new { Result = "O Token está Funcionando!" });
        }

        [Authorize(AuthenticationSchemes = "symm")]
        [HttpGet(nameof(VerifyTokenSymmetric))]
        public async Task<IActionResult> VerifyTokenSymmetric()
        {
            return Ok(new { Result = "O Token Simétrico está Funcionando!" });
        }

    }

}
