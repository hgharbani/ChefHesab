using ChefHesab.Share.Extiontions;
using DNTCommon.Web.Core;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ApiExtention>();
builder.Services.AddDNTCommonWeb();
builder.Services.AddMvc(options => options.UsePersianDateModelBinder());
builder.Services.AddMvc(options => options.UseYeKeModelBinder());
builder.Services.AddMvcCore(options => options.UsePersianDateModelBinder());
builder.Services.AddMvcCore(options => options.UseYeKeModelBinder());
builder.Services.AddControllersWithViews(options => options.Filters.Add(typeof(ApplyCorrectYeKeFilterAttribute)));
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
