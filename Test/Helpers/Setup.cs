using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi;
using MinimalApi.Dominio.Interfaces;
using Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Helpers
{
    public class Setup : WebApplicationFactory<Startup>
    {
        public static HttpClient Client = default!;

        [AssemblyInitialize]
        public static void ClassInit(TestContext context)
        {
            var factory = new Setup();
            Client = factory.CreateClient();
        }

        [AssemblyCleanup]
        public static void ClassCleanup()
        {
            Client.Dispose();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // substitui implementação real pelo mock
                services.AddScoped<IAdministradorService, AdministradorServiceMock>();
            });
        }
    }
}
