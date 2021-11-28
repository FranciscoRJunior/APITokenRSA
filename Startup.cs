using APITokenTest.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APITokenTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "APITokenTest", Version = "v1" });
            });

            #region Criar Chaves Privadas e Publicas
            // Fun��o feita para Criar as chaves quando a aplica��o for iniciada

            //Pegando as informa��es do appsetings.json para carregar a classe modelo
            var securityOptions = new SecurityOptions();
            Configuration.Bind(nameof(SecurityOptions), securityOptions);
            services.AddSingleton(securityOptions);

            //Chamando fun��o da classe ServiceInjector.cs para criar/sobrescrever pasta e XML das chaves
            services.AddRsaKeys(securityOptions);

            //Configurando a autentica��o usando o JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                //Chave Privada
                .AddJwtBearer(option =>
                {
                    //Carregando nossa chave privada 
                    // Se for Utilizar uma Chave Simetrica as linhas abaixo n�o s�o necess�rias
                    var rsa = RSA.Create();
                    string key = File.ReadAllText(securityOptions.PrivateKeyFilePath);
                    rsa.FromXmlString(key);

                    //Definindo Eventos de Sucesso e Falha na autentica��o
                    option.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = x => Task.CompletedTask,
                        OnAuthenticationFailed = x => Task.CompletedTask,
                        OnTokenValidated = x => Task.CompletedTask,
                    };

                    //Definindo parametros de autentica��o do Token
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Definindo que tipo de valida��es vamos requerer do token
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateActor = true,

                        //Definindo Emissor e servidores Validos
                        ValidIssuer = "webapi",
                        ValidAudience = "webapi",

                        //Definindo Chave de Assinatura v�lida                        
                        IssuerSigningKey = new RsaSecurityKey(rsa)
                    };
                })
                //Chave Publica
                .AddJwtBearer("public", option =>
                 {
                    //Carregando nossa chave Publica
                    var rsa = RSA.Create();
                     string key = File.ReadAllText(securityOptions.PublicKeyFilePath);
                     rsa.FromXmlString(key);

                    //Definindo Eventos de Sucesso e Falha na autentica��o
                    option.Events = new JwtBearerEvents
                     {
                         OnMessageReceived = x => Task.CompletedTask,
                         OnAuthenticationFailed = x => Task.CompletedTask,
                         OnTokenValidated = x => Task.CompletedTask,
                     };

                    //Definindo parametros de autentica��o do Token
                    option.TokenValidationParameters = new TokenValidationParameters
                     {
                        //Definindo que tipo de valida��es vamos requerer do token
                        ValidateAudience = true,
                         ValidateIssuer = true,
                         ValidateLifetime = true,
                         ValidateActor = true,

                        //Definindo Emissor e servidores Validos
                        ValidIssuer = "webapi",
                         ValidAudience = "webapi",

                        //Definindo Chave de Assinatura v�lida                        
                        IssuerSigningKey = new RsaSecurityKey(rsa)
                     };
                 })
                //Chave Simetrica
                .AddJwtBearer("symm", option =>
                 {
                    //Definindo Eventos de Sucesso e Falha na autentica��o
                    option.Events = new JwtBearerEvents
                     {
                         OnMessageReceived = x => Task.CompletedTask,
                         OnAuthenticationFailed = x => Task.CompletedTask,
                         OnTokenValidated = x => Task.CompletedTask,
                     };

                    //Definindo parametros de autentica��o do Token
                    option.TokenValidationParameters = new TokenValidationParameters
                     {
                        //Definindo que tipo de valida��es vamos requerer do token
                        ValidateAudience = true,
                         ValidateIssuer = true,
                         ValidateLifetime = true,
                         ValidateActor = true,

                        //Definindo Emissor e servidores Validos
                        ValidIssuer = "webapi",
                         ValidAudience = "webapi",

                        //Definindo Chave de Assinatura v�lida                        
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityOptions.SymmetricKey))
                     };
                 });

            #endregion

            #region Cors
            services.AddCors(options =>
            {
                options.AddPolicy("myPolicy", x =>
                {
                    x.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
                });
            });
            #endregion
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APITokenTest v1"));


            app.UseCors("myPolicy");
            app.UseRouting();

            //Para a Autentica��o JWT Funcionar, voc� precisa definir que a API vai usar autentica��o
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
