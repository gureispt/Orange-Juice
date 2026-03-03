using Microsoft.EntityFrameworkCore;
using OrangeJuiceBank.Application.Interfaces;
using OrangeJuiceBank.Application.Services;
using OrangeJuiceBank.Domain.Interfaces;
using OrangeJuiceBank.Infrastructure.Data;
using OrangeJuiceBank.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

//REGISTRA DBCONTEXT
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

// Registrar Repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IContaRepository, ContaRepository>();
builder.Services.AddScoped<IAtivoRepository, AtivoRepository>();
builder.Services.AddScoped<ICarteiraRepository, CarteiraRepository>();
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IContaService, ContaService>();
builder.Services.AddScoped<IAtivoService, AtivoService>();
builder.Services.AddScoped<ICarteiraService, CarteiraService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//CONFIGURANDO CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(context);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReact");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();