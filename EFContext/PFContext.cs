using Entities.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFContext
{
    public class PFContext : IdentityUserContext<IdentityUser>
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Tag> Tags { get; set; }


        public PFContext(DbContextOptions<PFContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}