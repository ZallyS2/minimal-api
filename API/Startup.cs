using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entities;
using MinimalApi.Dominio.Enuns;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Services;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi;

public class Startup {



    public Startup(IConfiguration configuration) {
        Configuration = configuration;
        key = Configuration?.GetSection("Jwt")?.ToString() ?? "12345";
    }

    private string key;

    public IConfiguration Configuration { get; set; } = default!;

    public void ConfigureServices(IServiceCollection services) {



        services.AddAuthentication(option => {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        services.AddAuthorization();


        services.AddScoped<IAdministradorService, AdministradorService>();
        services.AddScoped<IVeiculoService, VeiculoService>();


        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options => {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Insira o token JWT: "
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
        });

        services.AddDbContext<DbContexto>(options => {
            options.UseMySql(
                Configuration.GetConnectionString("mysql")!,
                ServerVersion.AutoDetect(Configuration.GetConnectionString("mysql"))
            );
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {


        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseRouting();

        app.UseEndpoints(endpoints => {

            #region Home
            endpoints.MapGet("/", () => {
                var home = new Home();
                var html = $@"
                    <!DOCTYPE html>
                    <html lang='pt-BR'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Documentação da API</title>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background: linear-gradient(135deg, #4f46e5, #9333ea);
                                color: white;
                                text-align: center;
                                padding: 50px;
                            }}
                            h1 {{
                                font-size: 2.5em;
                                margin-bottom: 20px;
                            }}
                            p {{
                                font-size: 1.2em;
                                margin-bottom: 30px;
                            }}
                            .bottom {{
                                background-color: white;
                                color: #4f46e5;
                                padding: 15px 30px;
                                border-radius: 8px;
                                text-decoration: none;
                                font-weight: bold;
                                transition: background 0.3s, color 0.3s;
                            }}
                            .bottom:hover {{
                                background-color: #4f46e5;
                                color: white;
                                border: 2px solid white;
                            }}
                            .rodape {{
                                margin-top: 100px;
                                bottom: 0%;
                                position: relative;
                                width: 100%;
                            }}
                        </style>
                    </head>
                    <body>
                        <h1>Bem-vindo à API</h1>
                        <p>Explore e teste os endpoints da API usando nossa documentação interativa.</p>
                        <a href='/{home.Documentation}' class='bottom'>📄 Abrir Swagger</a>
                        <br />
                        
                        <div class ='rodape'>
                            <hr />
                            <p align='center'>Feito com 💜 por Laila Zappiello</p>
                            <a href='https://www.instagram.com/zzappiello.o/'><img src='https://img.shields.io/badge/-Instagram-%23E4405F?style=for-the-badge&logo=instagram&logoColor=white' /></a>
                            <a href='mailto:lailazappiello90@gmail.com'><img src='https://img.shields.io/badge/Gmail-333333?style=for-the-badge&logo=gmail&logoColor=red' /></a>
                            <a href='https://wa.me/5511981642627'><img src='https://img.shields.io/badge/WhatsApp-25D366?style=for-the-badge&logo=whatsapp&logoColor=white' /></a>
                            <a href='https://www.linkedin.com/in/laila-zappiello/' target='_blank'><img src='https://img.shields.io/badge/-LinkedIn-%230077B5?style=for-the-badge&logo=linkedin&logoColor=white' target='_blank'></a> 
                            <br><br>
                            <p align='center'>
                                🌌 <strong>'Em um lugar escuro estamos nós. E mais conhecimento ilumina nosso caminho.'</strong> – Star Wars
                            </p>
                            <p align='center'>
                                <img src='https://github.com/zallih/Images/blob/main/Jedi%20grogu%F0%9F%92%9A.jpeg?raw=true' width='250px' />
                            </p>

                        </div>
                    </body>
                    </html>
                    ";

                return Results.Content(html, "text/html");
            }).AllowAnonymous().WithTags("Home");

            #endregion

            #region Administrador

            string GerarTokenJwt(Administrador administrador) {
                if (string.IsNullOrEmpty(key)) return string.Empty;
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim> {
                    new Claim("Email", administrador.Email),
                    new Claim("Perfil", administrador.Perfil),
                    new Claim(ClaimTypes.Role, administrador.Perfil),
                 };

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            //logar 
            endpoints.MapPost("/admins/login", ([FromBody] LoginDTO loginDTO, IAdministradorService administradorService) => {
                var adm = administradorService.Login(loginDTO);
                if (adm != null) {
                    string token = GerarTokenJwt(adm);
                    return Results.Ok(new AdministradorLogin {
                        Email = adm.Email,
                        Perfil = adm.Perfil,
                        Token = token
                    });
                }
                else
                    return Results.Problem(
                      detail: "Usuário ou senha inválidos.",
                      statusCode: StatusCodes.Status401Unauthorized,
                      title: "Não autorizado"
                  );
            }).AllowAnonymous().WithTags("Administradores");
            //cadastrar 
            endpoints.MapPost("/admins", ([FromBody] AdministradorDTO loginDTO, IAdministradorService administradorService) => {
                var validation = new ErrorValidation {
                    Mensagens = new List<string>()
                };

                if (string.IsNullOrEmpty(loginDTO.Email))
                    validation.Mensagens.Add("O email é obrigatório.");
                if (string.IsNullOrEmpty(loginDTO.Password))
                    validation.Mensagens.Add("A senha é obrigatória.");
                if (loginDTO.Perfil == null)
                    validation.Mensagens.Add("O perfil é obrigatório.");
                if (validation.Mensagens.Count > 0)
                    return Results.BadRequest(validation);

                var adm = new Administrador {
                    Email = loginDTO.Email,
                    Password = loginDTO.Password,
                    Perfil = loginDTO.Perfil.ToString() ?? Perfil.Editor.ToString()
                };

                administradorService.Include(adm);

                return Results.Created($"/admins/{adm.Id}", new AdmModelView {
                    Id = adm.Id,
                    Email = adm.Email,
                    Perfil = adm.Perfil
                });


            }).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Administradores");

            //buscar todos os administradores
            endpoints.MapGet("/admins", ([FromQuery] int? page, IAdministradorService administradorService) => {
                var adm = new List<AdmModelView>();
                var admAll = administradorService.All(page);
                foreach (var a in admAll) {
                    adm.Add(new AdmModelView {
                        Id = a.Id,
                        Email = a.Email,
                        Perfil = a.Perfil
                    });
                }
                return Results.Ok(adm);
            }).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Administradores");

            //buscar administrador por id
            endpoints.MapGet("/admins/{id}", ([FromRoute] int id, IAdministradorService administradorService) => {
                var adm = administradorService.BuscarPorId(id);
                if (adm == null)
                    return Results.NotFound();
                return Results.Ok(new AdmModelView {
                    Id = adm.Id,
                    Email = adm.Email,
                    Perfil = adm.Perfil
                });
            }).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Administradores");

            #endregion

            #region Veiculos

            ErrorValidation validaDTO(VeiculoDTO veiculoDTO) {
                var validation = new ErrorValidation {
                    Mensagens = new List<string>()
                };
                if (string.IsNullOrEmpty(veiculoDTO.Nome))
                    validation.Mensagens.Add("O nome do veículo é obrigatório.");
                if (string.IsNullOrEmpty(veiculoDTO.Marca))
                    validation.Mensagens.Add("A marca do veículo é obrigatória.");
                if (veiculoDTO.Ano <= 1950)
                    validation.Mensagens.Add("O ano do veículo é muito antigo.");

                return validation;
            }

            // Cadastrar Veículos
            endpoints.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculos, IVeiculoService veiculosService) => {
                var validation = validaDTO(veiculos);
                if (validation.Mensagens.Count > 0)
                    return Results.BadRequest(validation);

                var veiculo = new Veiculo {
                    Nome = veiculos.Nome,
                    Marca = veiculos.Marca,
                    Ano = veiculos.Ano
                };
                veiculosService.Incluir(veiculo);
                return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
            }).RequireAuthorization().WithTags("Veiculos");

            //pegar veículo por paginas
            endpoints.MapGet("/veiculos", ([FromQuery] int? page, IVeiculoService veiculosService) => {
                var veiculos = veiculosService.All(page);
                return Results.Ok(veiculos);
            }).RequireAuthorization().WithTags("Veiculos");

            //pegar veículo por Id
            endpoints.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculoService veiculosService) => {
                var veiculo = veiculosService.BuscarPorId(id);
                if (veiculo == null)
                    return Results.NotFound();
                return Results.Ok(veiculo);
            }).RequireAuthorization().WithTags("Veiculos");

            //mudar veículo por id
            endpoints.MapPut("/veiculos/{id}", ([FromRoute] int id, [FromBody] VeiculoDTO veiculoDTO, IVeiculoService veiculosService) => {
                var veiculo = veiculosService.BuscarPorId(id);
                if (veiculo == null)
                    return Results.NotFound();

                var validation = validaDTO(veiculoDTO);
                if (validation.Mensagens.Count > 0)
                    return Results.BadRequest(validation);

                veiculo.Nome = veiculoDTO.Nome;
                veiculo.Marca = veiculoDTO.Marca;
                veiculo.Ano = veiculoDTO.Ano;

                veiculosService.Atualizar(veiculo);
                return Results.Ok(veiculo);
            }).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Veiculos");

            // Deletar veículo
            endpoints.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculoService veiculosService) => {
                var veiculo = veiculosService.BuscarPorId(id);
                if (veiculo == null)
                    return Results.NotFound();

                veiculosService.Deletar(veiculo);
                return Results.NoContent();
            }).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Veiculos");

            #endregion

        });
    }

}