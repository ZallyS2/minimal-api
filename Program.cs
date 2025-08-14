
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Welcome!");

app.MapPost("/login", (MinimalApi.DTOs.LoginDTO login) => {
    if (login.Email == "adm@teste.com" && login.Password == "12345")
        return Results.Ok("Login successful");
    else
        return Results.Unauthorized();
});

app.Run();

