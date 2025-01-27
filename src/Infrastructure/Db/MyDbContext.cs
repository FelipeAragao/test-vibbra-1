
using Microsoft.EntityFrameworkCore;
using src.Domain.Entities;
using src.Infrastructure.Security;

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
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Invite> Invites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.UserId)
                .ValueGeneratedOnAdd();

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
                .HasMany(u => u.Deliveries)
                .WithOne()
                .HasForeignKey(m => m.UserId)
                .IsRequired();

            modelBuilder.Entity<Invite>()
                .HasOne(i => i.User)
                .WithMany(u => u.InvitesSent)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invite>()
                .HasOne(i => i.UserInvited)
                .WithMany(u => u.InvitesReceived)
                .HasForeignKey(i => i.UserInvitedId)
                .OnDelete(DeleteBehavior.Restrict);

            byte[] salt = PasswordHasher.GenerateSalt();
            
            modelBuilder.Entity<User>().HasData(new User {
                UserId = 1,
                Name = "Super User",
                Email = "super@user.com",
                Login = "super",
                Password = PasswordHasher.HashPassword("123", salt),
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
                .HasForeignKey<DealLocation>(d => d.DealId)
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

            modelBuilder.Entity<Deal>()
                .HasOne(d => d.Delivery)
                .WithOne()
                .HasForeignKey<Delivery>(d => d.DealId)
                .IsRequired();
/*
            modelBuilder.Entity<Delivery>()
                .HasMany(d => d.Steps)
                .WithOne()
                .HasForeignKey(d => d.DeliveryId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
*/
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