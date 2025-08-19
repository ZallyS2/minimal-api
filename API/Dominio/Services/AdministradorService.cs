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

        public List<Administrador> All(int? page) {
            var query = _context.Administradores.AsQueryable();
            int pageSize = 10;
            if (page != null) {
                query = query.Skip(((int)page - 1) * pageSize)
                             .Take(pageSize);
            }
            return query.ToList();
        }

        public Administrador? BuscarPorId(int id) {
            return _context.Administradores.Where(a => a.Id == id).FirstOrDefault();

        }

        public Administrador Include(Administrador administrador) {
            _context.Administradores.Add(administrador);
            _context.SaveChanges();
            return administrador;
        }

        public Administrador? Login(LoginDTO login) {
            var adm = (_context.Administradores.Where(a => a.Email == login.Email && a.Password == login.Password).FirstOrDefault());
            return adm;

        }
    }
}