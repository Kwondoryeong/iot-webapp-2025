using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyPortfolioWebApp.Models;

namespace MyPortfolioWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // DB연결 초기화 설정
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(
                builder.Configuration.GetConnectionString("SmartHomeConnection"),
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SmartHomeConnection"))
            ));

            // ASP.NET Core Identity 설정
            // 원본은 IdentityUser -> CustomUser로 변경
            builder.Services.AddIdentity<CustomUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // 패스워드 정책
            // 변경 전. 최대 6자리 이상, 특수문자 1개 포함, 영어대소문자 포함

            // 변경 후. 최대 4자리 이상, 숫자 포함, 특수문자 제외
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // 패스워드 정책 설정 Require(필수)
                options.Password.RequireDigit = true; // 숫자 포함 
                options.Password.RequireLowercase = false; // 소문자 포함
                options.Password.RequireUppercase = false; // 대문자 포함
                options.Password.RequiredLength = 4; // 최소 길이 4
                options.Password.RequireNonAlphanumeric = false; // 특수문자 포함 여부
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();  // ASP.NET Core Identity 계정
            app.UseAuthorization();   // 권한

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
