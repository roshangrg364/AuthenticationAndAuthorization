using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModule.Dto;

namespace UserModule.Authentication
{
    public interface TokenGeneratorInterface
    {
        string GenerateToken(TokenDto dto);
    }
}
