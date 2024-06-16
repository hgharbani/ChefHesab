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
using System.Text.Json;
using DNTCommon.Web.Core;
using ChefHesab.Share.ModelBinder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});


builder.Services.AddMvcCore(options => {
   // options.UsePersianDateModelBinder();

    options.ModelBinderProviders.Insert(0, new PersianDateModelBinderProvider());
    options.UseYeKeModelBinder();
    });
builder.Services.AddControllers().AddJsonOptions(options =>
options.JsonSerializerOptions.PropertyNamingPolicy = null);

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
builder.Services.AddCors();
builder.Services.AddData();
builder.Services.AddApplication();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
