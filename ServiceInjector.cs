using APITokenTest.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APITokenTest
{
    public static class ServiceInjector
    {
        public static IServiceCollection AddRsaKeys(this IServiceCollection services, SecurityOptions options)
        {
            //Buscando o caminho de onde as chaves serão salvas
            string keysFolder = Path.GetDirectoryName(options.PrivateKeyFilePath);

            //Verifica se a pasta já esxiste no projeto, caso não exista ela será criada.
            if (!Directory.Exists(keysFolder))
            {
                Directory.CreateDirectory(keysFolder);

                //Criando as chaves privada e publica utilizando o metodo RSA
                var rsa = RSA.Create();
                string privateKeyXml = rsa.ToXmlString(true);
                string publicKeyXml = rsa.ToXmlString(false);

                using var privateFile = File.Create(options.PrivateKeyFilePath);
                using var publicFile = File.Create(options.PublicKeyFilePath);

                //Criando/Sobrescrevendo dados das chaves em um arquivo XML 
                privateFile.Write(Encoding.ASCII.GetBytes(privateKeyXml));
                publicFile.Write(Encoding.ASCII.GetBytes(publicKeyXml));
            }
            //Retornando o serviço solicitado
            return services;
        }
    }
}
