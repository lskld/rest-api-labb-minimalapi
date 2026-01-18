using Microsoft.EntityFrameworkCore;
using rest_api_labb_minimalapi.Models.Entities;

namespace rest_api_labb_minimalapi.Data
{
    public class RestLabbDbContext : DbContext
    {
        public RestLabbDbContext(DbContextOptions<RestLabbDbContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<Link> Links { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Rules for People
            modelBuilder.Entity<Person>()
                .Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Person>()
                .Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Person>()
                .Property(p => p.PhoneNumber)
                .IsRequired()
                .HasMaxLength(50);

            //Rules for Interests
            modelBuilder.Entity<Interest>()
                .Property(i => i.InterestName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Interest>()
                .Property(i => i.InterestDescription)
                .HasMaxLength(500);

            //Rules for Links
            modelBuilder.Entity<Link>()
                .Property(l => l.LinkUrl)
                .IsRequired();
        }
    }
}
