using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.Entities;
using MinimalApi.Dominio.Services;
using MinimalApi.Infraestrutura.Db;
using Microsoft.Extensions.Configuration;

namespace Test.Domain.Services;

[TestClass]
public class AdmService
{
    private DbContexto CriarContexto()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();
        return new DbContexto(configuration);
    }

    [TestMethod]
    public void TestMethod1()
    {
        // Arrange
        var adm = new Administrador
        {
            Id = 1,
            Email = "teste@teste.com",
            Password = "123456",
            Perfil = "Adm"
        };

        var context = CriarContexto();
        context.Administradores.RemoveRange(context.Administradores);
        context.SaveChanges();

        var service = new AdministradorService(context);

        // Act
        service.Include(adm);
        service.BuscarPorId(1);

        // Assert
        Assert.AreEqual(1, service.All(1).Count);
    }

}
