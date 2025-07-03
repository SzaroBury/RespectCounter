using RespectCounter.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RespectCounter.Infrastructure.Identity;

namespace RespectCounter.Infrastructure
{
    internal static class SeedData
    {
        public static void Seed(ModelBuilder mb)
        {
            var adminId = SeedIdentity(mb);

            Guid RL = Guid.NewGuid();
            Guid RK = Guid.NewGuid();
            Guid AD = Guid.NewGuid();
            Guid DT = Guid.NewGuid();
            mb.Entity<Person>().HasData(new List<Person>
            {
                CreateDummyPerson(RL, "Robert", "Lewandowski", "Footballer", new DateOnly(1988, 08, 21), "Polish", adminId, nickname: "Lewy"),
                CreateDummyPerson(RK, "Robert", "Kubica", "Racer", new DateOnly(1984, 12, 07), "Polish", adminId, status: PersonStatus.NotVerified),
                CreateDummyPerson(AD, "Andrzej", "Duda", "Politician", new DateOnly(1972, 05, 16), "Polish", adminId),
                CreateDummyPerson(DT, "Donald", "Tusk", "Politician", new DateOnly(1957, 04, 22), "Polish", adminId)
            });

            Guid RLactivity = Guid.NewGuid();
            Guid RKactivity = Guid.NewGuid();
            mb.Entity<Activity>().HasData(new List<Activity>
            {
                CreateDummyActivity(RLactivity, RL, "Milik jest słaby", "", "Test description", "Dude, just trust me", adminId, type: ActivityType.Quote),
                CreateDummyActivity(RKactivity, RK, "Monaco GP 2010: Robeeeeeeeert Kubica P2 in Quali", "Monaco, MC", "Można utknąć w eeeee korku", "https://www.youtube.com/watch?v=qbYMoKxif6I", adminId, type:ActivityType.Act, happend: new DateTime(2010, 05, 15), status: ActivityStatus.Verified)
            });

            Guid RLComment = Guid.NewGuid();
            Guid RLActivityComment = Guid.NewGuid();
            Guid RLActivityNegativeComment = Guid.NewGuid();
            Guid ADComment = Guid.NewGuid();
            Guid ADResponse = Guid.NewGuid();

            mb.Entity<Comment>().HasData(
            [
                CreateDummyComment(RLComment, "Najlepszy zawodnik!",                    adminId, 2, 2, perId: RL),
                CreateDummyComment(Guid.NewGuid(), "No nie wiem. Milik lepszy!",        adminId, parentId: RLComment),
                CreateDummyComment(Guid.NewGuid(), "Jest całkiem dobry faktycznie",     adminId, parentId: RLComment),
                CreateDummyComment(RLActivityComment, "Fajność!",                       adminId, 2, 2, actId: RLactivity),
                CreateDummyComment(Guid.NewGuid(), "Zgadza się!",                       adminId, parentId: RLActivityComment),
                CreateDummyComment(Guid.NewGuid(), "Też się zgadzam. Fajność!",         adminId, parentId: RLActivityComment),
                CreateDummyComment(RLActivityNegativeComment, "Niefajność",             adminId, 1, 1, actId: RLactivity),
                CreateDummyComment(Guid.NewGuid(), "Nie zgadzam się. Fajność.",         adminId, parentId: RLActivityNegativeComment),
                CreateDummyComment(Guid.NewGuid(), "Lepsza weeeeeersja: https://www.youtube.com/watch?v=vmLonweq6wA", adminId, actId: RKactivity),
                CreateDummyComment(ADComment, "Bardzo memiczna osoba",                  adminId, 2, 3, perId: AD),
                CreateDummyComment(Guid.NewGuid(), "Hańba!",                            adminId, parentId : ADComment),
                CreateDummyComment(ADResponse, "Chyba ty",                              adminId, parentId : ADComment),
                CreateDummyComment(Guid.NewGuid(), "Nie, bo ty",                        adminId, parentId : ADResponse),
                CreateDummyComment(Guid.NewGuid(), "Ja tam mu nei ufam",                adminId, perId: AD),
                CreateDummyComment(Guid.NewGuid(), "Nie lubiem go, bo Andrzej to dziwne imię", adminId, perId: AD)
            ]);

            Guid sportTag = Guid.NewGuid();
            Guid footballTag = Guid.NewGuid();
            Guid fcbarcelonaTag = Guid.NewGuid();
            Guid f1Tag = Guid.NewGuid();
            Guid wecTag = Guid.NewGuid();
            Guid politicsTag = Guid.NewGuid();
            Guid pisTag = Guid.NewGuid();
            Guid poTag = Guid.NewGuid();
            mb.Entity<Tag>().HasData(new List<Tag>
            {
                CreateDummyTag(sportTag, "Sport", adminId, 1),
                CreateDummyTag(footballTag, "Football", adminId),
                CreateDummyTag(fcbarcelonaTag, "FC Barcelona", adminId),
                CreateDummyTag(f1Tag, "F1", adminId),
                CreateDummyTag(wecTag, "WEC", adminId),
                CreateDummyTag(politicsTag, "Politics", adminId, 1),
                CreateDummyTag(pisTag, "PiS", adminId),
                CreateDummyTag(poTag, "PO", adminId),
            });
            mb.Entity("PersonHasTag").HasData(
                new { TagsId = sportTag, PersonsId = RL },
                new { TagsId = sportTag, PersonsId = RK },
                new { TagsId = footballTag, PersonsId = RL },
                new { TagsId = fcbarcelonaTag, PersonsId = RL },
                new { TagsId = f1Tag, PersonsId = RK },
                new { TagsId = wecTag, PersonsId = RK },
                new { TagsId = politicsTag, PersonsId = AD },
                new { TagsId = politicsTag, PersonsId = DT },
                new { TagsId = pisTag, PersonsId = AD },
                new { TagsId = poTag, PersonsId = DT }
            );
            mb.Entity("ActivityHasTag").HasData(
                new { TagsId = sportTag, ActivitiesId = RLactivity },
                new { TagsId = footballTag, ActivitiesId = RLactivity },
                new { TagsId = sportTag, ActivitiesId = RKactivity },
                new { TagsId = f1Tag, ActivitiesId = RKactivity }
            );
            mb.Entity<Reaction>().HasData(new List<Reaction>
            {
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Hate,    adminId, perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Like,    adminId, perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Dislike, adminId, perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Like,    adminId, perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Love,    adminId, perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Love,    adminId, perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Love,    adminId, perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Love,    adminId, perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Like,    adminId, actId: RLactivity),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Dislike, adminId, actId: RLactivity),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Like,    adminId, actId: RLactivity),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Love,    adminId, actId: RLactivity),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Love,    adminId, comId: RLComment),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Like,    adminId, comId: RLActivityComment),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Love,    adminId, comId: RLActivityComment)
            });
        }

        private static Guid SeedIdentity(ModelBuilder mb)
        {
            Guid adminRoleId = Guid.NewGuid();
            Guid userRoleId = Guid.NewGuid();
            Guid adminId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            mb.Entity<IdentityRole<Guid>>().HasData(new List<IdentityRole<Guid>>
            {
                new("Admin") { Id = adminRoleId },
                new("User") { Id = userRoleId }
            });

            // Seed sys user
            var hasher = new PasswordHasher<AppUser>();
            var admin = new AppUser
            {
                Id = adminId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");
            var user = new AppUser
            {
                Id = userId,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                UserName = "user",
                NormalizedUserName = "USER",
                Email = "user@example.com",
                NormalizedEmail = "USER@EXAMPLE.COM",
                EmailConfirmed = true
            };
            user.PasswordHash = hasher.HashPassword(user, "User123!");

            mb.Entity<AppUser>().HasData(admin, user);

            mb.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>()
                {
                    UserId = adminId,
                    RoleId = adminRoleId
                }
            );

            mb.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>()
                {
                    UserId = userId,
                    RoleId = userRoleId
                }
            );

            return adminId;
        }
        private static Person CreateDummyPerson(Guid id, string firstName, string lastName, string profession, DateOnly birthDate, string nationality, Guid createdById, string nickname = "", DateOnly? deathDate = null, PersonStatus status = PersonStatus.Verified, string desc = "Test desc")
        {
            var now = DateTime.Now;
            return new Person
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                NickName = nickname,
                Profession = profession,
                Description = desc,
                Birthday = birthDate,
                DeathDate = deathDate.GetValueOrDefault(),
                Nationality = nationality,
                Status = status,

                Created = now,
                CreatedById = createdById,
                LastUpdated = now,
                LastUpdatedById = createdById
            };
        }
        private static Tag CreateDummyTag(Guid id, string name, Guid createdById, int level = 5, string desc = "Test desc")
        {
            return new Tag
            {
                Id = id,
                Name = name,
                Level = level,
                Description = desc, 
                //Persons = null

                Created = DateTime.Now,
                CreatedById = createdById,
                LastUpdated = DateTime.Now,
                LastUpdatedById = createdById
            };
        }
        private static Activity CreateDummyActivity(Guid id, Guid personId, string val, string loc, string desc, string source, Guid createdById, DateTime? happend = null, ActivityType type = ActivityType.Quote, ActivityStatus status = ActivityStatus.NotVerified)
        {
            return new Activity
            {
                Id = id, 
                Location = loc,
                Value = val,
                Description = desc,
                Happend = happend.GetValueOrDefault(),
                Type = type,
                Source = source,
                Status = status,
                PersonId = personId,
                //Reactions
                //Comments

                Created = DateTime.Now,
                CreatedById = createdById,
                LastUpdated = DateTime.Now,
                LastUpdatedById = createdById
            };
        }
        private static Comment CreateDummyComment(
            Guid id,
            string content,
            Guid createdById,
            int directChildrenCount = 0,
            int allChildrenCount = 0,
            Guid? perId = null,
            Guid? actId = null,
            Guid? parentId = null) //, List<Reaction>? rea = null, List<Comment>? chil = null
        {
            return new Comment
            {
                Id = id,      
                Content = content,
                DirectChildrenCount = directChildrenCount,
                AllChildrenCount = allChildrenCount,

                PersonId = perId,
                ParentId = parentId,
                ActivityId = actId,
                //Reactions = rea ?? new(),
                //Children = chil ?? new(),

                Created = DateTime.Now,
                CreatedById = createdById,
                LastUpdated = DateTime.Now,
                LastUpdatedById = createdById
            };
        }
        private static Reaction CreateDummyReaction(Guid id, ReactionType type, Guid createdById, Guid? perId = null, Guid? actId = null, Guid? comId = null)
        {
            return new Reaction
            {
                Id = id,
                ReactionType = type,
                PersonId = perId,
                ActivityId = actId,
                CommentId = comId,

                Created = DateTime.Now,
                CreatedById = createdById,
                LastUpdated = DateTime.Now,
                LastUpdatedById = createdById
            };
        }   
    }
}
