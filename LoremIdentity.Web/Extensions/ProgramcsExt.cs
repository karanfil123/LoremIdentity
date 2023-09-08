using LoremIdentity.Web.CustomValidations;
using LoremIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace LoremIdentity.Web.Extensions
{
    public static class ProgramcsExt
    {
        public static void AddIdentityExt(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromMinutes(30);
            });//şifre sıfırlamada link ömrü için token süresi
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";

                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                options.Lockout.MaxFailedAccessAttempts = 3;
            })
                .AddPasswordValidator<PasswordValidator>()
                .AddUserValidator<UserValidator>()
                .AddErrorDescriber<LocalIdentityDescriber>()
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(opt =>
            {
                var cokieBuilder = new CookieBuilder();
                cokieBuilder.Name = "LoremIdentity.Web.Cokie";
                opt.LoginPath = new PathString("/Home/SignIn");
                opt.LogoutPath = new PathString("/Member/LogOut");
                opt.AccessDeniedPath = new PathString("/Member/AccessDenied");
                opt.Cookie = cokieBuilder;
                opt.ExpireTimeSpan = TimeSpan.FromDays(5);//5 gün boyunca cokiie saklanır
                opt.SlidingExpiration = true;//her giriş yapıldığında o andan itibaren 5 gü daha uzamış olur
            });
        }
    }
}