using Autofac.Extensions.DependencyInjection;
using Autofac;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Domain.Peresentition.IRepositories;
using ChefHesab.Data.Presentition.Reositories;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Domain.Peresentition.IRepositories.IGenericRepository;
using AutoMapper.Features;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using ChefHesab.Application;
using ChefHesab.Data;
using ChefHesab.Application.services.define;
using ChefHesab.Application.Interface.define;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IPersonalService, PersonalService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(autofacBuilder =>
{
    var assemblyData = typeof(ChefHesab.Data.Middleware).Assembly;
    autofacBuilder.RegisterAssemblyTypes(assemblyData)
           .AsImplementedInterfaces()
           .InstancePerLifetimeScope();



    var assemblyApplication = typeof(ChefHesab.Application.Middleware).Assembly;
    autofacBuilder.RegisterAssemblyTypes(assemblyApplication)
           .AsImplementedInterfaces()
           .InstancePerLifetimeScope();

 
    // Add this line
    autofacBuilder
        .RegisterType<ChefHesabContext>()
        .InstancePerLifetimeScope();
    
});
builder.Services.AddData();
builder.Services.AddApplication();
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
