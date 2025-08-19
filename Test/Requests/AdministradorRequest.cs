using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.DTOs;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class AdministradorRequest
{
    private static Setup factory = default!;
    private static HttpClient client = default!;

    [ClassInitialize]
    public static void ClassInit(TestContext context)
    {
        factory = new Setup();
        client = factory.CreateClient();
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        client.Dispose();
        factory.Dispose();
    }

    [TestMethod]
    public async Task TestLogin()
    {
        var loginDto = new LoginDTO { Email = "adm@teste.com", Password = "123456" };

        var content = new StringContent(
            JsonSerializer.Serialize(loginDto),
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync("/admins/login", content);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}
