using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.Entities;

namespace Test.Domain.Entities;

[TestClass]
public class VeiculoTeste
{
    [TestMethod]
    public void TestGetSetProp()
    {
        //Arange
        var veiculo = new Veiculo();


        //Act
        veiculo.Id = 1;
        veiculo.Nome = "Fusca";
        veiculo.Marca = "Volkswagen";
        veiculo.Ano = 1976;

        //Assert
        Assert.AreEqual(1, veiculo.Id);
        Assert.AreEqual("Fusca", veiculo.Nome);
        Assert.AreEqual("Volkswagen", veiculo.Marca);
        Assert.AreEqual(1976, veiculo.Ano);
    }
}
