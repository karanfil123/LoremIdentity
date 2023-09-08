using LoremIdentity.Web.ClaimProvider;
using LoremIdentity.Web.Extensions;
using LoremIdentity.Web.MailServices;
using LoremIdentity.Web.Models;
using LoremIdentity.Web.OptionsModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDBContext>(optins =>
{
    optins.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});

builder.Services.Configure<SecurityStampValidatorOptions>(opt =>
{
    opt.ValidationInterval = TimeSpan.FromMinutes(30);//yarým saatte bir  veritabanýndaki securitystamp deðerini cokidekiyle karþýlaþtýrýr farklýysa logine atar.
});
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));//picture yolu eriliþmi
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddIdentityExt();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
   name: "KullaniciListesi",
                    pattern: "kullanici-listesi",
                    defaults: new { controller = "Home", action = "UserList" });

app.Run();