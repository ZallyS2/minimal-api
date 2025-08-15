using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entities;

namespace MinimalApi.Dominio.Interfaces {
    public interface IVeiculoService {
        List<Veiculo> All(int? page = 1, string? nome = null, string? marca = null);
        Veiculo? BuscarPorId(int id);
        void Incluir(Veiculo veiculo);
        void Atualizar(Veiculo veiculo);
        void Deletar(Veiculo veiculo);
    }
}