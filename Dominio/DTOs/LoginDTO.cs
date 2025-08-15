using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.Dominio.DTOs {
    public class LoginDTO {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}