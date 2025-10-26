using UrlShortner.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAcessEFCore
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Url> Urls { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add custom configuration here
            // USER
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UuId)
                .IsUnique();

            modelBuilder.Entity<User>()
               .HasOne(u => u.UserCredential)
               .WithOne(c => c.User)
               .HasForeignKey<UserCredential>(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            // One-to-many User ↔ UserTokens
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserTokens)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many User ↔ Url
            modelBuilder.Entity<User>()
                .HasMany(u => u.Urls)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // URL 
            modelBuilder.Entity<Url>()
                .HasIndex(u => u.UuId)
                .IsUnique();

            // USER-URL
            // Unique index on UuId for consistency
            modelBuilder.Entity<UserUrl>()
                .HasIndex(u => u.UuId)
                .IsUnique();

            // Many-to-many via UserUrl
            modelBuilder.Entity<UserUrl>()
                .HasOne(uu => uu.User)
                .WithMany(u => u.UserUrls)  // You need a collection in User
                .HasForeignKey(uu => uu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserUrl>()
                .HasOne(uu => uu.Url)
                .WithMany(u => u.UserUrls)  // You need a collection in Url
                .HasForeignKey(uu => uu.UrlId)
                .OnDelete(DeleteBehavior.Restrict);

            // Optional: unique constraint to prevent duplicate UserId + UrlId
            modelBuilder.Entity<UserUrl>()
                .HasIndex(uu => new { uu.UserId, uu.UrlId })
                .IsUnique();
        }
    }
}