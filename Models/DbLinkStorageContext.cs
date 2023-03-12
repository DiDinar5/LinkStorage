using Microsoft.EntityFrameworkCore;

namespace LinkStorage.Models
{
    public class DbLinkStorageContext: DbContext
    {
        public DbLinkStorageContext(DbContextOptions<DbLinkStorageContext> options):base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Authorization> Authorization { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Authorization>().HasKey(e => e.Email);
        //    modelBuilder.Entity<User>().HasOne<Authorization>(e=>e.Authorization).WithOne(E => E.UserInfo);
        }
    }
}
