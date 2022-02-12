using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAutores.Entidades
{
    public class Usuario : IdentityUser
    {
        public bool MalaPaga { get; set; }
    }
}
