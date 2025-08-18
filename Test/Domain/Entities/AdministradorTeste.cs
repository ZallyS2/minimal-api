using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.Entities;

namespace Test.Domain.Entities;

[TestClass]
public class AdministradorTeste {
    [TestMethod]
    public void TestGetSetProp() {
        //Arange
        var adm = new Administrador();


        //Act
        adm.Id = 1;
        adm.Email = "teste@teste.com";
        adm.Password = "123456";
        adm.Perfil = "Adm";


        //Assert
        Assert.AreEqual(1, adm.Id);
        Assert.AreEqual("teste@teste.com", adm.Email);
        Assert.AreEqual("123456", adm.Password);
        Assert.AreEqual("Adm", adm.Perfil);
    }
}
