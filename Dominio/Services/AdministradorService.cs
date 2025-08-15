using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entities;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi.Dominio.Services {
    public class AdministradorService : IAdministradorService {

        private readonly DbContexto _context;
        public AdministradorService(DbContexto context) {
            _context = context;
        }
        public Administrador? Login(LoginDTO login) {
            var adm = (_context.Administradores.Where(a => a.Email == login.Email && a.Password == login.Password).FirstOrDefault());
            return adm;

        }
    }
}