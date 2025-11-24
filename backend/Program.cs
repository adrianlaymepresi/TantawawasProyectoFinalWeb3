using backend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

////////////////////////////////////////////////////////////
// Base de datos en memoria temportal para probar las api
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MiBDEnMemoria"));

// Registrar servicios personalizados
builder.Services.AddScoped<backend.Services.RolService>();
builder.Services.AddScoped<backend.Services.UsuarioService>();

////////////////////////////////////////////////////////////

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
