using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.Services;
using MinimalApi.Dominio.Entidades;

namespace Test.Domain.Services
{
    [TestClass]
    public class AdministradorServiceTest
    {

        private DbContexto CriarContexto()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            var connectionString = configuration.GetConnectionString("mysql");
            var options = new DbContextOptionsBuilder<DbContexto>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;
            return new DbContexto(options);
        }

        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            var contexto = CriarContexto();
            contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

            var adm = new Administrador();
            adm.Email = "teste@teste.com";
            adm.Password = "123456";
            adm.Perfil = "Adm";

            var service = new AdministradorService(contexto);


            //Act

            service.Include(adm);

            //Assert
            Assert.AreEqual(1, service.All(1).Count());

        }
    }
}