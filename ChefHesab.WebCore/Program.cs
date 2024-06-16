using ChefHesab.Share.Extiontions;
using ChefHesab.Share.ModelBinder;
using DNTCommon.Web.Core;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(options => {
        options.AllowAnyOrigin();
        options.AllowAnyMethod();
        options.AllowAnyHeader();
    });
});
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ApiExtention>();
builder.Services.AddDNTCommonWeb();

builder.Services.AddMvcCore(options => {

    options.ModelBinderProviders.Insert(0, new PersianDateModelBinderProvider());
    options.UseYeKeModelBinder();
});


builder.Services.AddControllersWithViews(options => {
    options.ModelBinderProviders.Insert(0, new PersianDateModelBinderProvider());
    options.Filters.Add(typeof(ApplyCorrectYeKeFilterAttribute));
    
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US"),
});
app.MapControllerRoute(
    name: "defaultArea",
    pattern: "{area}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();
