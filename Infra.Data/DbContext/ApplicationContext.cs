using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.DbContext
{
    public class ApplicationContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Icon> Icons { get; set; }
        public DbSet<Usage> Usages { get; set; }
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.HasDefaultSchema("Yalda_IconService");

            // modelBuilder.Entity<Notification>().Property(n => n.Username).
            //     HasConversion(value => value.Encrypt(),value=>value.Decrypt());

        }
    }
}