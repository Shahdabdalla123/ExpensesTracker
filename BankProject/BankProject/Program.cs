using BankProject.Models;
using Expenses_App.infastructure.Repositories;
using Expenses_App_.Core.Interfaces;
using Expenses_App_.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using AutoMapper;
using BankProject.Middleware;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Expenses_App.Application.CQRS.Queries;

namespace BankProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Controllers
            builder.Services.AddControllers();

            // HttpClient
            builder.Services.AddHttpClient();

            // Swagger + JWT
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Expenses API", Version = "v1" });
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id=JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme="Oauth2",
                            Name=JwtBearerDefaults.AuthenticationScheme,
                            In=ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            // JWT Auth
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            // Db Contexts
            builder.Services.AddDbContext<BankprojectContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("connection")));
            builder.Services.AddDbContext<AppDbcontext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("connection")));

            // DI
            builder.Services.AddScoped<IAuth, AuthenticationService>();
            builder.Services.AddScoped<IExpensesRepositry, ExpensesRepository>();

            // Identity
            builder.Services.AddIdentityCore<Appuser>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<Appuser>>("Bank Loan System")
                .AddEntityFrameworkStores<AppDbcontext>()
                .AddDefaultTokenProviders();

            // Prevent redirects for APIs
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });

            // Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .WriteTo.Console()
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Infinite, shared: true)
                .WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("connection"),
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
                    columnOptions: new ColumnOptions())
                .CreateLogger();

            builder.Host.UseSerilog();

            // --- CORS ---
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                    policy.WithOrigins("http://localhost:4200") // Match Angular origin
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });
            builder.Services.AddMediatR(cfg =>
            {
                // This ensures MediatR scans the assembly that contains your handlers
                cfg.RegisterServicesFromAssemblyContaining<GetAllExpensesQuery>();
            });
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Middleware order is critical
            app.UseCors("AllowAngularApp");
            app.UseLoggingMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
