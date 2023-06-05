using LinkStorage.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkStorage.DataBase
{
    public class DbLinkStorageContext : DbContext
    {
        public DbLinkStorageContext(DbContextOptions<DbLinkStorageContext> options) : base(options)
        {
            Database.Migrate();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<SmartContract> SmartContracts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>();
            modelBuilder.Entity<SmartContract>().HasKey(l=>l.LinkToContract);
        }
    }
}
