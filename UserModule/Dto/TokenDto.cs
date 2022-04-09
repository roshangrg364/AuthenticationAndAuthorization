﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserModule.Dto
{
    public class TokenDto
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string JwtKey { get; set; }
    }
}
