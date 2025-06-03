using RespectCounter.Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RespectCounter.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace RespectCounter.Infrastructure
{
    public class RespectDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        //To add migration:         dotnet-ef migrations add <migration name> -p RespectCounter.Infrastructure -s RespectCounter.API -c RespectDbContext
        //To apply migration:       dotnet-ef update database

        public required DbSet<Activity> Activities { get; set; }
        public required DbSet<Comment> Comment { get; set; }
        public required DbSet<Person> Persons { get; set; }
        public required DbSet<Reaction> Reactions { get; set; }
        public required DbSet<Tag> Tags { get; set; }

        public RespectDbContext(DbContextOptions<RespectDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Activity>().Ignore(t => t.CreatedBy);
            modelBuilder.Entity<Activity>().Ignore(t => t.LastUpdatedBy);
            modelBuilder.Entity<Person>().Ignore(t => t.CreatedBy);
            modelBuilder.Entity<Person>().Ignore(t => t.LastUpdatedBy);
            modelBuilder.Entity<Comment>().Ignore(t => t.CreatedBy);
            modelBuilder.Entity<Comment>().Ignore(t => t.LastUpdatedBy);
            modelBuilder.Entity<Reaction>().Ignore(t => t.CreatedBy);
            modelBuilder.Entity<Reaction>().Ignore(t => t.LastUpdatedBy);
            modelBuilder.Entity<Tag>().Ignore(t => t.CreatedBy);
            modelBuilder.Entity<Tag>().Ignore(t => t.LastUpdatedBy);
            modelBuilder.Entity<AppUser>().Ignore(u => u.RecentlyBrowsedTags);
            modelBuilder.Entity<AppUser>().Ignore(u => u.FavoriteTags);

            modelBuilder.Entity<Person>().HasMany(p => p.Tags).WithMany(t => t.Persons).UsingEntity("PersonHasTag");
            modelBuilder.Entity<Activity>().HasMany(p => p.Tags).WithMany(t => t.Activities).UsingEntity("ActivityHasTag");

            SeedData.Seed(modelBuilder);
        }
    }
}