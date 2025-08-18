using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.Dominio.ModelViews;

public record AdministradorLogin {

    public string Email { get; set; }
    public string Perfil { get; set; }
    public string Token { get; set; }
}
