using Microsoft.EntityFrameworkCore;

namespace LinkStorage.Models
{
    public class DbLinkStorageContext: DbContext
    {
        public DbLinkStorageContext(DbContextOptions<DbLinkStorageContext> options):base(options)
        {
            Database.EnsureCreated();
            Database.Migrate();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Authorization> Authorization { get; set; }
        public DbSet<SmartContract> SmartContracts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Authorization>().HasKey(x => x.UserId);
        }
    }
}
