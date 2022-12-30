using FluentValidation.AspNetCore;
using Posterr.Infra.CrossCutting.IoC.RestAPI;
using Posterr.RestAPI;
using Posterr.RestAPI.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapperSetup();
builder.Services.AddFluentValidation();

// IoC registers
DependencyInjectorStartup.Register(builder.Services, builder.Configuration);

var app = builder.Build();


MigrationManager.ApplyMigration(app.Services);

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
