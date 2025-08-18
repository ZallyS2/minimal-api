using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalApi.Dominio.Enuns;

namespace MinimalApi.Dominio.DTOs {
    public class AdministradorDTO {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public Perfil? Perfil { get; set; } = default!;
    }
}