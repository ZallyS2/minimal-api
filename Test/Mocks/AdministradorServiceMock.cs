using MinimalApi.Dominio.Entities;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace Test.Mocks;

public class AdministradorServiceMock : IAdministradorService
{

    private static List<Administrador> administradores = new List<Administrador>()
    {
        new Administrador
        {
            Id = 1,
            Email = "adm@teste.com",
            Password = "123456",
            Perfil = "Adm"
        },
        new Administrador
        {
            Id = 1,
            Email = "edit@teste.com",
            Password = "123456",
            Perfil = "Editor"
        }
    };
    public Administrador? Login(LoginDTO login)
    {
        return administradores.FirstOrDefault(a => a.Email == login.Email && a.Password == login.Password);
    }

    public Administrador Include(Administrador administrador)
    {
        administrador.Id = administradores.Count() + 1;
        administradores.Add(administrador);
        return administrador;
    }

    public List<Administrador> All(int? page)
    {
        return administradores;
    }

    public Administrador? BuscarPorId(int id)
    {
        return administradores.Find(a => a.Id == id);

    }
}
