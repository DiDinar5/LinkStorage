using Microsoft.EntityFrameworkCore;

namespace LinkStorage.Models
{
    public class DbLinkStorageContext: DbContext
    {
        public DbLinkStorageContext(DbContextOptions<DbLinkStorageContext> options):base(options)
        {
            Database.Migrate();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Authorization> Authorization { get; set; }
        public DbSet<SmartContract> SmartContracts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<Authorization>().HasKey(x => x.UserId);
            //modelBuilder.Entity<SmartContract>().HasKey(x => x.Id);


            //modelBuilder.Entity<User>()
            //   .HasOne(e => e.Authorization)
            //   .WithOne(E => E.UserInfo)
            //   .HasForeignKey<User>(x => x.Id);

            //modelBuilder.Entity<Authorization>()
            //    .HasOne(x => x.UserInfo)
            //    .WithOne(x => x.Authorization)
            //    .HasForeignKey<Authorization>(x => x.UserId);

            //modelBuilder.Entity<SmartContract>()
            //    .HasOne(s => s.User)
            //    .WithMany(c => c.SmartContracts);


        }
    }
}
