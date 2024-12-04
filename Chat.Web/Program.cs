using Chat.Data.Contexts;
using Chat.Data.Entities;
using Chat.Repository.Interfaces;
using Chat.Repository.Repositories;
using Chat.Services.Helper;
using Chat.Services.Interfaces;
using Chat.Services.Mapping.Profiles;
using Chat.Services.Services;
using Chat.Web.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = 150000000;

            });
            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            builder.Services.AddDbContext<ChatDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.Configure<TwillioSettings>(builder.Configuration.GetSection("Twillio"));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IConnectionService, ConnectionService>();
            builder.Services.AddScoped<IMessagesService, MessageService>();
            builder.Services.AddAutoMapper(typeof(ConnectionProfile));
            builder.Services.AddAutoMapper(typeof(MessageProfile));

            builder.Services.AddTransient<ISMSServices, SMSServices>();
            builder.Services.AddScoped<IAuthService, AuthService>();    

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                        .AddEntityFrameworkStores<ChatDbContext>()
                        .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("default", p =>
                {
                    p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("default", p =>
                {
                    p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.MapHub<ChatHub>("/chathub");

            app.Run();
        }
    }
}
