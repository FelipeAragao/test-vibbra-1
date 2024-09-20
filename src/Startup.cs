
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Application.DTOs;
using src.Application.Interfaces;
using src.Application.Services;
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
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<IUserService, UserService>();

            services.AddDbContext<MyDbContext>(options => {
                options.UseMySql(
                    Configuration.GetConnectionString("MySql"),
                    ServerVersion.AutoDetect(Configuration.GetConnectionString("MySql")));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/", () => "Hello World!");

                endpoints.MapPost("/login", async (HttpContext context) => {
                    var userService = context.RequestServices.GetRequiredService<IUserService>();
                    var loginDTO = await context.Request.ReadFromJsonAsync<LoginDTO>();

                    if (loginDTO == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsJsonAsync(new { error = "Invalid login data" });
                        return;
                    }

                    var userDTO = await userService.Login(loginDTO);

                    if (userDTO != null)
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        await context.Response.WriteAsJsonAsync(userDTO);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new { error = "Unauthorized" });
                    }
                });
/*
                endpoints.MapPost("/login", async ([FromBody] LoginDTO loginDTO, IUserService userService) => {
                    var userDTO = await userService.Login(loginDTO);

                    if(userDTO != null)
                        return Results.Ok(userDTO);
                    else
                        return Results.Unauthorized();
                });*/
            });
        }
    }
}