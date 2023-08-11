using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Web.Api.Contratos.Configuration;
using Web.Api.Contratos.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//configuracao da connection string
string Sql = builder.Configuration.GetConnectionString("WebApiDatabase");
builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(Sql));
builder.Services.AddSwaggerGen();

builder.Services.ResolveDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
