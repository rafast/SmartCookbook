using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartCookbook.Domain.Extension;
using SmartCookbook.Infrastructure;
using SmartCookbook.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRepository(builder.Configuration);

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

UpdateDatabase();

app.Run();

void UpdateDatabase()
{
    var connection = builder.Configuration.GetConnection();
    var databaseName = builder.Configuration.GetDatabaseName();

    Database.CreateDatabase(connection, databaseName);

    app.MigrateDatabase();
}
