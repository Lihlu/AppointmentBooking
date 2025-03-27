using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Data
{
    public class JwtSettings
    {
        public required string Token { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
    }
}
