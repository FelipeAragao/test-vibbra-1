
using Microsoft.EntityFrameworkCore;
using src.Domain.Entities;

namespace src.Infrastructure.Db
{
    public class MyDbContext : DbContext
    {
        private readonly IConfiguration _configuracaoAppSettings;

        public MyDbContext(IConfiguration configuracaoAppSettings)
        {
            this._configuracaoAppSettings = configuracaoAppSettings;
        }

        public DbSet<User> Users { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User {
                UserId = 1,
                Name = "Teste",
                Email = "teste@gmail.com",
                Login = "teste",
                Password = "123",
                Locations = new List<UserLocation>()
            });

            modelBuilder.Entity<UserLocation>().HasData(
                new UserLocation
                {
                    UserLocationId = 1,
                    UserId = 1,
                    Lat = -23.55052,
                    Lng = -46.633308,
                    Address = "123 Main St",
                    City = "São Paulo",
                    State = "SP",
                    ZipCode = 12345
                }
            );

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                var stringConexao = _configuracaoAppSettings.GetConnectionString("MySql")?.ToString();
                if (!string.IsNullOrEmpty(stringConexao))
                    optionsBuilder.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
            }
        }
    }
}