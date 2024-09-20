
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

        public DbSet<User> Users { get; set; }
        public DbSet<Deal> Deals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Locations)
                .WithOne()
                .HasForeignKey(l => l.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Deals)
                .WithOne()
                .HasForeignKey(d => d.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Bids)
                .WithOne()
                .HasForeignKey(l => l.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Messages)
                .WithOne()
                .HasForeignKey(m => m.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(u => u.InvitesSent)
                .WithOne()
                .HasForeignKey(i => i.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(u => u.InvitesReceived)
                .WithOne()
                .HasForeignKey(i => i.UserInvitedId)
                .IsRequired();

            modelBuilder.Entity<User>().HasData(new User {
                UserId = 1,
                Name = "Teste",
                Email = "teste@gmail.com",
                Login = "teste",
                Password = "123",
                Locations = new List<UserLocation>(),
                Deals = null,
                Bids = null
            });

        modelBuilder.Entity<UserLocation>().HasData(new UserLocation {
                UserLocationId = 1,
                UserId = 1,
                Lat = -23.55052,
                Lng = -46.633308,
                Address = "123 Main St",
                City = "São Paulo",
                State = "SP",
                ZipCode = 12345
            });

            modelBuilder.Entity<Deal>()
                .HasOne(d => d.Location)
                .WithOne()
                .IsRequired();

            modelBuilder.Entity<Deal>()
                .HasMany(d => d.DealImages)
                .WithOne()
                .HasForeignKey(i => i.DealId)
                .IsRequired();

            modelBuilder.Entity<Deal>()
                .HasMany(d => d.Bids)
                .WithOne()
                .HasForeignKey(b => b.DealId)
                .IsRequired();

            modelBuilder.Entity<Deal>()
                .HasMany(d => d.Messages)
                .WithOne()
                .HasForeignKey(m => m.DealId)
                .IsRequired();

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