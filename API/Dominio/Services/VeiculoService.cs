using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entities;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi.Dominio.Services {
    public class VeiculoService : IVeiculoService {

        private readonly DbContexto _context;
        public VeiculoService(DbContexto context) {
            _context = context;
        }

        public List<Veiculo> All(int? page = 1, string? nome = null, string? marca = null) {
            var query = _context.Veiculos.AsQueryable();
            if (!string.IsNullOrEmpty(nome)) {
                query = query.Where(v => EF.Functions.Like(v.Nome.ToLower(), $"%{nome.ToLower()}%"));
            }

            int pageSize = 10;
            if (page != null) {
                query = query.Skip(((int)page - 1) * pageSize)
                             .Take(pageSize);
            }
            return query.ToList();
        }

        public void Atualizar(Veiculo veiculo) {
            _context.Veiculos.Update(veiculo);
            _context.SaveChanges();
        }

        public Veiculo? BuscarPorId(int id) {
            return _context.Veiculos.Where(v => v.Id == id).FirstOrDefault();

        }

        public void Deletar(Veiculo veiculo) {
            _context.Veiculos.Remove(veiculo);
            _context.SaveChanges();
        }

        public void Incluir(Veiculo veiculo) {
            _context.Veiculos.Add(veiculo);
            _context.SaveChanges();
        }
    }
}