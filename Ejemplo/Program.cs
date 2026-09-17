using Ejemplo.Repositories;
using Ejemplo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Esta es la unica linea que cambia la persistencia del caso de uso.
builder.Services.AddSingleton<IUsuarioRepository, JsonUsuarioRepository>();
// Para PostgreSQL, reemplazar la linea anterior por:
//builder.Services.AddScoped<IUsuarioRepository, PostgresUsuarioRepository>();
builder.Services.AddScoped<AltaUsuarioUseCase>();

var app = builder.Build();
app.UseCors();
app.MapControllers();
app.Run();

public partial class Program { }