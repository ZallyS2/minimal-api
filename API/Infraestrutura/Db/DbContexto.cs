using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalApi.Dominio.Entities;

namespace MinimalApi.Infraestrutura.Db;

public class DbContexto : DbContext {

    private readonly IConfiguration _configurationAppSettings;
    public DbContexto(IConfiguration configurationAppSettings) {
        _configurationAppSettings = configurationAppSettings;
    }
    public DbSet<Administrador> Administradores { get; set; } = default!;
    public DbSet<Veiculo> Veiculos { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Administrador>().HasData(
            new Administrador {
                Id = 1,
                Perfil = "Adm",
                Email = "adm@teste.com",
                Password = "12345"
            }
        );
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            var stringConexao = _configurationAppSettings.GetConnectionString("mysql")?.ToString();
            if (!string.IsNullOrEmpty(stringConexao)) {
                optionsBuilder.UseMySql(
                    stringConexao,
                    ServerVersion.AutoDetect(stringConexao)
                );
            }
        }
    }

}