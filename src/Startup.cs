
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using src.Application.DTOs;
using src.Application.Interfaces;
using src.Application.Services;
using src.Configurations;
using src.Infrastructure.Db;

namespace EcommerceVibbra
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option => {
                option.TokenValidationParameters = new TokenValidationParameters {
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"] ?? "")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                option.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.NoResult();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "text/plain";
                        return context.Response.WriteAsync("Invalid token.");
                    }
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"] ?? "";
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"] ?? "";
                googleOptions.Events.OnRemoteFailure = context =>
                {
                    // Log detailed error
                    var error = context.Failure;
                    context.Response.Redirect("/error?message=" + error?.Message);
                    context.HandleResponse(); // Suppress the exception
                    return Task.CompletedTask;
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("JwtBearer", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("GoogleSSO", policy =>
                {
                    policy.AuthenticationSchemes.Add(GoogleDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "EcommerceVibbra API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT"
                });

                options.AddSecurityDefinition("Google", new OpenApiSecurityScheme {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/auth"),
                            TokenUrl = new Uri("https://oauth2.googleapis.com/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "Access your profile information" },
                                { "email", "Access your email address" },
                                { "profile", "Access your basic profile information" }
                            }
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Google"
                            }
                        },
                        new[] { "openid", "email", "profile" }
                    }
                });
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.LoginPath = "/api/v1/authenticate/sso";
                options.LogoutPath = "/api/v1/authenticate/logout";
            });

            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IUserService, UserService>();

            services.AddControllersWithViews();

            services.AddDbContext<MyDbContext>(options => {
                options.UseMySql(
                    Configuration.GetConnectionString("MySql"),
                    ServerVersion.AutoDetect(Configuration.GetConnectionString("MySql")));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode == 404)
                {
                    response.ContentType = "application/json";
                    var errorResponse = new {
                        error = "Route not found"
                    };
                    var jsonResponse = JsonSerializer.Serialize(errorResponse);
                    if (!string.IsNullOrEmpty(jsonResponse))
                    {
                        await response.WriteAsync(jsonResponse);
                    }
                }
            });
            app.UseSwagger();
            app.UseSwaggerUI(swaggerConfig =>
            {
                swaggerConfig.SwaggerEndpoint("/swagger/v1/swagger.json", "EcommerceVibbra v1");

                // Configurações de OAuth para Google
                swaggerConfig.OAuthClientId("your-google-client-id.apps.googleusercontent.com");
                swaggerConfig.OAuthClientSecret("your-google-client-secret");
                swaggerConfig.OAuthUsePkce();  // Usar PKCE

                // Se necessário, você também pode configurar mais opções do OAuth
                swaggerConfig.OAuthScopeSeparator(" ");
                swaggerConfig.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}