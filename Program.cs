
using Microsoft.EntityFrameworkCore;
using MinimalApi.Infraestrutura.Db;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.Services;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Entities;


#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorService, AdministradorService>();
builder.Services.AddScoped<IVeiculoService, VeiculoService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql")!,
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});
var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => {
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
}).WithTags("Home");

#endregion

#region Administrador
app.MapPost("/admins/login", ([FromBody] LoginDTO loginDTO, IAdministradorService administradorService) => {
    if (administradorService.Login(loginDTO) != null)
        return Results.Ok("Login realizado com successo!");
    else
        return Results.Problem(
          detail: "Usuário ou senha inválidos.",
          statusCode: StatusCodes.Status401Unauthorized,
          title: "Não autorizado"
      );
}).WithTags("Administradores");
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
app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculos, IVeiculoService veiculosService) => {
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
}).WithTags("Veiculos");

//pegar veículo por paginas
app.MapGet("/veiculos", ([FromQuery] int? page, IVeiculoService veiculosService) => {
    var veiculos = veiculosService.All(page);
    return Results.Ok(veiculos);
}).WithTags("Veiculos");

//pegar veículo por Id
app.MapGet("$/veiculos/{id}", ([FromRoute] int id, IVeiculoService veiculosService) => {
    var veiculo = veiculosService.BuscarPorId(id);
    if (veiculo == null)
        return Results.NotFound();
    return Results.Ok(veiculo);
}).WithTags("Veiculos");

//mudar veículo por id
app.MapPut("/veiculos/{id}", ([FromRoute] int id, [FromBody] VeiculoDTO veiculoDTO, IVeiculoService veiculosService) => {
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
}).WithTags("Veiculos");

// Deletar veículo
app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculoService veiculosService) => {
    var veiculo = veiculosService.BuscarPorId(id);
    if (veiculo == null)
        return Results.NotFound();

    veiculosService.Deletar(veiculo);
    return Results.NoContent();
}).WithTags("Veiculos");

#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
#endregion

