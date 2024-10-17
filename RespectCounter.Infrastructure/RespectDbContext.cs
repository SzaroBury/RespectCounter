using RespectCounter.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RespectCounter.Infrastructure
{
    public class RespectDbContext : IdentityDbContext<IdentityUser>
    {
        //To add migration:         dotnet-ef migrations add <migration name> -p RespectCounter.Infrastructure -s RespectCounter.API -c RespectDbContext
        //To apply migration:       dotnet-ef update database

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public RespectDbContext(DbContextOptions<RespectDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>().HasMany(p => p.Tags).WithMany(t => t.Persons).UsingEntity("PersonHasTag");
            modelBuilder.Entity<Person>().HasMany(p => p.Activities).WithMany(a => a.Persons).UsingEntity("PersonHasActivity");
            modelBuilder.Entity<Activity>().HasMany(p => p.Tags).WithMany(t => t.Activities).UsingEntity("ActivityHasTag");

            SeedData.Seed(modelBuilder);
            //The seeding code should not be part of the normal app execution as this can cause concurrency issues when multiple instances are running and would also require the app having permission to modify the database schema
            //https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding#custom-initialization-logic
        }
    }
}