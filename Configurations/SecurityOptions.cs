using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITokenTest.Configurations
{
    public class SecurityOptions
    {
        public string PrivateKeyFilePath { get; set; }
        public string PublicKeyFilePath { get; set; }
        public string SymmetricKey { get; set; }
    }

}
