using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Data.Contexts;
using Company.DAL.Models;
using Company.PL.Helper;
using Company.PL.Mapper;
using Company.PL.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(); // Register Built-in MVC services

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Register DI for UnitOfWork
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Register DI for DepartmentRepository
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); // Register DI for EmployeeRepository
            //builder.Services.AddAutoMapper(typeof(EmployeeMapper)); 
            builder.Services.AddAutoMapper(M=> M.AddProfile( new EmployeeMapper())); // Register DI for EmployeeMapper

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                                    .AddEntityFrameworkStores<CompanyDbContext>()// Register DI for IdentityRole
                                    .AddDefaultTokenProviders();
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));

            builder.Services.AddScoped<IMailService, MailService>(); // register DI for MailService ????? ??????? 
            builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection(nameof(TwilioSettings)));
            builder.Services.AddScoped<ITwilioService, TwilioService>();

            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            },ServiceLifetime.Scoped); // Register DI for CompanyDbContext

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";


            });


            // 
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

            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
