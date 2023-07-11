using Entities.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFContext
{
    internal static class SeedData
    {
        private static IdentityUser? SysUser;

        public static void Seed(ModelBuilder mb)
        {
            SeedIdentity(mb);

            mb.Entity<Person>().HasData(new List<Person>
            {
                CreateDummyPerson(1, "Robert", "Lewandowski", new DateTime(1988, 08, 21), "Polish"),
                CreateDummyPerson(2, "Robert", "Kubica", new DateTime(1984, 12, 07), "Polish"),
                CreateDummyPerson(3, "Andrzej", "Duda", new DateTime(1972, 05, 16), "Polish"),
                CreateDummyPerson(4, "Donald", "Tusk", new DateTime(1957, 04, 22), "Polish")
            });
            mb.Entity<Tag>().HasData(new List<Tag>
            {
                CreateDummyTag(1, "Sport", true),
                CreateDummyTag(2, "Football"),
                CreateDummyTag(3, "FC Barcelona"),
                CreateDummyTag(4, "F1"),
                CreateDummyTag(5, "WEC"),
                CreateDummyTag(6, "Politics", true),
                CreateDummyTag(7, "PiS"),
                CreateDummyTag(8, "PO"),
            });
            mb.Entity<Activity>().HasData(new List<Activity>
            {
                CreateDummyActivity(1, 1, "Milik jest słaby", "", "Dude, just trust me", quote: true),
                CreateDummyActivity(2, 2, "Monaco GP 2010: Robeeeeeeeert Kubica P2 in Quali", "Można utknąć w eeeee korku", "https://www.youtube.com/watch?v=qbYMoKxif6I", happend: new DateTime(2010, 05, 15), ver: true)
            });
            mb.Entity<Comment>().HasData(new List<Comment>
            {
                CreateDummyComment(1, "Najlepszy zawodnik!",                perId: 1),
                CreateDummyComment(2, "No nie wiem. Milik lepszy!",         parentId: 1),
                CreateDummyComment(3, "Jest całkiem dobry faktycznie",      parentId: 1),
                CreateDummyComment(4, "Fajność!",                           actId: 1),
                CreateDummyComment(5, "Zgadza się!",                        parentId: 4),
                CreateDummyComment(6, "Niefajność",                         actId: 1),             
                CreateDummyComment(7, "Lepsza weeeeeersja: https://www.youtube.com/watch?v=vmLonweq6wA", actId: 2),
                CreateDummyComment(8, "Bardzo memiczna osoba",              perId: 3),
                CreateDummyComment(9, "Hańba!",                             parentId : 8),
                CreateDummyComment(10, "Chyba ty",                          parentId : 8),
                CreateDummyComment(11, "Ja tam mu nei ufam",                perId: 3),
                CreateDummyComment(12, "Nie lubiem go, bo Andrzej to dziwne imię", perId: 3)
            });
            mb.Entity<Reaction>().HasData(new List<Reaction>
            {
                CreateDummyReaction(8, ReactionType.Hate,           perId: 1),
                CreateDummyReaction(9, ReactionType.NotUnderstand,  perId: 1),
                CreateDummyReaction(10, ReactionType.Dislike,       perId: 1),
                CreateDummyReaction(11, ReactionType.Like,          perId: 1),
                CreateDummyReaction(12, ReactionType.Love,          perId: 1),
                CreateDummyReaction(13, ReactionType.Love,          perId: 1),
                CreateDummyReaction(14, ReactionType.Love,          perId: 1),
                CreateDummyReaction(15, ReactionType.Love,          perId: 1),
                CreateDummyReaction(4, ReactionType.Like,           actId: 1),
                CreateDummyReaction(5, ReactionType.NotUnderstand,  actId: 1),
                CreateDummyReaction(6, ReactionType.Like,           actId: 1),
                CreateDummyReaction(7, ReactionType.Love,           actId: 1),
                CreateDummyReaction(3, ReactionType.Love,           comId: 4),
                CreateDummyReaction(1, ReactionType.Like,           comId: 5),
                CreateDummyReaction(2, ReactionType.Love,           comId: 5)
            });
        }

        private static void SeedIdentity(ModelBuilder mb)
        { 
            string adminRoleGuid = Guid.NewGuid().ToString();
            string sysUserGuid = Guid.NewGuid().ToString();

            mb.Entity<IdentityRole>().HasData(new List<IdentityRole>
            {
                new IdentityRole("Admin") { Id = adminRoleGuid },
                new IdentityRole("User")
            });

            // Seed sys user
            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
            var sysUserPassword = "123";
            string pass = new PasswordHasher<IdentityUser>().HashPassword(null, sysUserPassword);
            SysUser = new IdentityUser { UserName = "sys", PasswordHash = pass, Id = sysUserGuid };
            mb.Entity<IdentityUser>().HasData(SysUser);

            mb.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>()
            {
                UserId = sysUserGuid,
                RoleId = adminRoleGuid
            });
            
        }

        private static Person CreateDummyPerson(int id, string firstName, string lastName, DateTime birthDate, string nationality, DateTime? deathDate = null, float score = 5.0f, string desc = "Test desc")
        {
            return new Person
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Description = desc,
                Birthday = birthDate,
                DeathDate = deathDate.GetValueOrDefault(),
                Nationality = nationality,
                PublicScore = score,

                Created = DateTime.Now,
                CreatedById = SysUser.Id,
                LastUpdated = DateTime.Now,
                LastUpdatedById = SysUser.Id
            };
        }
        private static Tag CreateDummyTag(int id, string name, bool mainTag = false, string desc = "Test desc")
        {
            return new Tag
            {
                Id = id,
                Name = name,
                IsMainTag = mainTag,
                Description = desc, 
                //Persons = null

                Created = DateTime.Now,
                CreatedById = SysUser.Id,
                LastUpdated = DateTime.Now,
                LastUpdatedById = SysUser.Id
            };
        }
        private static Activity CreateDummyActivity(int id, int personId, string value, string desc, string source, DateTime? happend = null, bool quote = false, bool ver = false)
        {
            return new Entities.Model.Activity
            {
                Id = id, 
                Value = value,
                Description = desc,
                Happend = happend.GetValueOrDefault(),
                IsQuote = quote,
                Source = source,
                Verified = ver,
                PersonId = personId, 
                //Person
                //Reactions
                //Comments

                Created = DateTime.Now,
                CreatedById = SysUser.Id,
                LastUpdated = DateTime.Now,
                LastUpdatedById = SysUser.Id
            };
        }
        private static Comment CreateDummyComment(int id, string content, int? perId = null, int? actId = null, int? parentId = null) //, List<Reaction>? rea = null, List<Comment>? chil = null
        {
            return new Comment
            {
                Id = id,      
                Content = content,
                PersonId = perId,
                ParentId = parentId,
                ActivityId = actId,
                //Reactions = rea ?? new(),
                //Children = chil ?? new(),

                Created = DateTime.Now,
                CreatedById = SysUser.Id,
                LastUpdated = DateTime.Now,
                LastUpdatedById = SysUser.Id
            };
        }
        private static Reaction CreateDummyReaction(int id, ReactionType type, int? perId = null, int? actId = null, int? comId = null)
        {
            return new Reaction
            {
                Id = id,
                ReactionType = type,
                PersonId = perId,
                ActivityId = actId,
                CommentId = comId,

                Created = DateTime.Now,
                CreatedById = SysUser.Id,
                LastUpdated = DateTime.Now,
                LastUpdatedById = SysUser.Id
            };
        }   
    }
}
