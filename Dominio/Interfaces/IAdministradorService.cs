using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entities;

namespace MinimalApi.Dominio.Interfaces {
    public interface IAdministradorService {
        Administrador? Login(LoginDTO login);
    }
}