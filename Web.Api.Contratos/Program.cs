using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Web.Api.Contratos.Configuration;
using Web.Api.Contratos.Context;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//configuracao da connection string
builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebApiDatabase"));
});

//adicionando configuracao do identity
builder.Services.AddIdentityConfig(builder.Configuration);

//configurações personalizadas para não povoar a class Program.cs
builder.Services.AddWebApiConfig();

builder.Services.AddSwaggerGen();

builder.Services.ResolveDependencies();

var app = builder.Build();

app.UseWebApiConfig(app.Environment);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
