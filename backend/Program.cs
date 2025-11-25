using backend.Data;
using backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

////////////////////////////////////////////////////////////
// Base de datos en memoria temportal para probar las api
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MiBDEnMemoria"));

// Registrar servicios personalizados
builder.Services.AddScoped<RolService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<CursoService>();
builder.Services.AddScoped<InscripcionService>();
builder.Services.AddScoped<MaterialService>();
builder.Services.AddScoped<MensajeService>();

builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

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
