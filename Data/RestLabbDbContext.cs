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

            //Seed People
            modelBuilder.Entity<Person>().HasData(
                new Person {FirstName = "Anna", LastName = "Svensson", PhoneNumber = "070-1234567" },
                new Person {FirstName = "Erik", LastName = "Johansson", PhoneNumber = "070-2345678" },
                new Person {FirstName = "Maria", LastName = "Nilsson", PhoneNumber = "070-3456789" },
                new Person {FirstName = "Oscar", LastName = "Lindberg", PhoneNumber = "070-4567890" },
                new Person {FirstName = "Sofia", LastName = "Andersson", PhoneNumber = "070-5678901" }
            );

            //Seed Interests
            modelBuilder.Entity<Interest>().HasData(
                new Interest {InterestName = "Programming", InterestDescription = "Writing and developing software applications" },
                new Interest {InterestName = "Photography", InterestDescription = "Capturing moments through a camera lens" },
                new Interest {InterestName = "Cooking", InterestDescription = "Preparing and experimenting with food recipes" },
                new Interest {InterestName = "Gaming", InterestDescription = "Playing video games across various platforms" },
                new Interest {InterestName = "Reading", InterestDescription = "Exploring books and literature" },
                new Interest {InterestName = "Hiking", InterestDescription = "Walking and exploring nature trails" },
                new Interest {InterestName = "Music", InterestDescription = "Playing instruments or listening to music" },
                new Interest {InterestName = "Traveling", InterestDescription = "Visiting new places and experiencing cultures" },
                new Interest {InterestName = "Gardening", InterestDescription = "Growing plants and maintaining gardens" },
                new Interest {InterestName = "Fitness", InterestDescription = "Exercising and maintaining physical health" }
            );

            //Seed Links (connecting to both Person and Interest)
            modelBuilder.Entity<Link>().HasData(
                new Link {LinkUrl = "https://github.com/anna-codes", PersonId = 1, InterestId = 1 },
                new Link {LinkUrl = "https://instagram.com/erik-photos", PersonId = 2, InterestId = 2 },
                new Link {LinkUrl = "https://recipes.com/maria-kitchen", PersonId = 3, InterestId = 3 },
                new Link {LinkUrl = "https://twitch.tv/anna-gaming", PersonId = 1, InterestId = 4 }
            );

            //Seed Many-to-Many relationship between Person and Interest
            modelBuilder.Entity<Person>()
                .HasMany(p => p.Interests)
                .WithMany(i => i.People)
                .UsingEntity(j => j.HasData(
                    new { PeoplePersonId = 1, InterestsInterestId = 1 },  // Anna -> Programming
                    new { PeoplePersonId = 1, InterestsInterestId = 4 },  // Anna -> Gaming
                    new { PeoplePersonId = 2, InterestsInterestId = 2 },  // Erik -> Photography
                    new { PeoplePersonId = 3, InterestsInterestId = 3 }   // Maria -> Cooking
                ));
        }
    }
}
