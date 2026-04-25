
using Dal;
using Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service;
using Service.Mapping;
using System.Security.Claims;
using System.Text;
using DotNetEnv;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
            });

            // DB connection
            builder.Services.AddDbContext<ProjectContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("EarnifyDBConnection"));
            });
            builder.Services.AddScoped<DbContext, ProjectContext>();

            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(
            //            Encoding.UTF8.GetBytes(builder.Configuration["JWTConfig:Key"] ?? throw new Exception("JWTConfig:Key Not Found"))),

            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        ValidateLifetime = true,

            //        // 🔹 Only name identifier (userId) is relevant
            //        NameClaimType = ClaimTypes.NameIdentifier,
            //        RoleClaimType = ClaimTypes.Role // optional, won't be used
            //    };

            //    options.Events = new JwtBearerEvents
            //    {
            //        OnMessageReceived = context =>
            //        {
            //            // 1️⃣ Check cookie first (optional)
            //            if (context.Request.Cookies.ContainsKey("Token"))
            //            {
            //                context.Token = context.Request.Cookies["Token"];
            //            }

            //            // 2️⃣ Fallback to Authorization header
            //            if (string.IsNullOrEmpty(context.Token) &&
            //                context.Request.Headers.ContainsKey("Authorization"))
            //            {
            //                var authHeader = context.Request.Headers["Authorization"].ToString();
            //                if (authHeader.StartsWith("Bearer "))
            //                {
            //                    context.Token = authHeader.Substring("Bearer ".Length).Trim();
            //                }
            //            }

            //            return Task.CompletedTask;
            //        }
            //    };
            //});

            //builder.Services.AddAuthentication("Cookies")
            //    .AddCookie("Cookies", options =>
            //    {
            //        options.Cookie.Name = "AuthCookie";
            //        options.Cookie.HttpOnly = true;
            //        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //        options.Cookie.SameSite = SameSiteMode.None;

            //        //options.LoginPath = "/api/Auth/Unauthorized";
            //    });

            builder.Services.AddAuthentication(options =>
            {
                // Tell ASP.NET to use JwtBearer by default
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWTConfig:Key"] ?? throw new Exception("Key Missing"))),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        NameClaimType = ClaimTypes.NameIdentifier
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // This MUST match the name used in Cookies.Append("Token", ...)
            if (context.Request.Cookies.ContainsKey("Token"))
            {
                context.Token = context.Request.Cookies["Token"];
            }
            return Task.CompletedTask;
        }
    };
});

            builder.Services.AddHttpClient();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            // add mapper
            builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

            // add repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // add services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOAuthService, OAuthService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseCors("AllowAngular");

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
