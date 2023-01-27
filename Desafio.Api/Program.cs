using Desafio.Api.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Desafio.Persistence.Contexts.DesafioContext>(context => 
        context.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));

builder.Services.AddMappingProfiles();

builder.Services.InjectDependencies();

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseCors(x => x.AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowAnyOrigin());
app.Run();
