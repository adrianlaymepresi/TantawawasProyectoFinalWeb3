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
builder.Services.AddScoped<EvaluacionService>();
builder.Services.AddScoped<ResultadoEvaluacionService>();

builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

////////////////////////////////////////////////////////////

// CREACION DE POLITICAS DE AUTORIZACION

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EsAdmin", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("EsDocente", policy => policy.RequireRole("Docente"));
    options.AddPolicy("EsEstudiante", policy => policy.RequireRole("Estudiante"));
    options.AddPolicy("AdminODocente", policy =>
        policy.RequireRole("Administrador", "Docente"));
});

////////////////////////////////////////////////////////////

// Add services to the container.

//builder.Services.AddControllers();
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
