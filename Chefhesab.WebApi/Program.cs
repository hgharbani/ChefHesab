using Autofac;
using Autofac.Extensions.DependencyInjection;
using ChefHesab.Application;
using ChefHesab.Application.Interface.define;
using ChefHesab.Application.services.define;
using ChefHesab.Data;
using ChefHesab.Data.Presentition.Context;
using DNTCommon.Web.Core;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Autofac.Core;
using ChefHesab.Data.Presentition.Reositories;
using ChefHesab.Domain.Peresentition.IRepositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddMvcCore(options =>
{
    // options.UsePersianDateModelBinder();

    options.ModelBinderProviders.Insert(0, new PersianDateModelBinderProvider());
    options.UseYeKeModelBinder();
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(autofacBuilder =>
{
    // Add this line
    autofacBuilder
        .RegisterType<ChefHesabContext>()
        .InstancePerLifetimeScope();


    var assemblyData = typeof(ChefHesab.Data.Middleware).Assembly;
    autofacBuilder.RegisterAssemblyTypes(assemblyData)
           .AsImplementedInterfaces()
           .InstancePerLifetimeScope();



    var assemblyApplication = typeof(ChefHesab.Application.Middleware).Assembly;
    autofacBuilder.RegisterAssemblyTypes(assemblyApplication)
           .AsImplementedInterfaces()
           .InstancePerLifetimeScope();
    autofacBuilder.RegisterType<ChefHesabUnitOfWork>().As<IChefHesabUnitOfWork>();

 


});
builder.Services.AddData();

builder.Services.AddCors();

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
