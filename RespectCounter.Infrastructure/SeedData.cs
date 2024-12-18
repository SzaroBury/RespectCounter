using RespectCounter.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace RespectCounter.Infrastructure
{
    internal static class SeedData
    {
        private static IdentityUser? SysUser;

        public static void Seed(ModelBuilder mb)
        {
            SeedIdentity(mb);

            Guid RL = Guid.NewGuid();
            Guid RK = Guid.NewGuid();
            Guid AD = Guid.NewGuid();
            Guid DT = Guid.NewGuid();
            mb.Entity<Person>().HasData(new List<Person>
            {
                CreateDummyPerson(RL, "Robert", "Lewandowski", new DateOnly(1988, 08, 21), "Polish"),
                CreateDummyPerson(RK, "Robert", "Kubica", new DateOnly(1984, 12, 07), "Polish", status: PersonStatus.NotVerified),
                CreateDummyPerson(AD, "Andrzej", "Duda", new DateOnly(1972, 05, 16), "Polish"),
                CreateDummyPerson(DT, "Donald", "Tusk", new DateOnly(1957, 04, 22), "Polish")
            });

            Guid RLactivity = Guid.NewGuid();
            Guid RKactivity = Guid.NewGuid();
            mb.Entity<Activity>().HasData(new List<Activity>
            {
                CreateDummyActivity(RLactivity, RL, "Milik jest słaby", "", "", "Dude, just trust me", type: ActivityType.Quote),
                CreateDummyActivity(RKactivity, RK, "Monaco GP 2010: Robeeeeeeeert Kubica P2 in Quali", "Monaco, MC", "Można utknąć w eeeee korku", "https://www.youtube.com/watch?v=qbYMoKxif6I", happend: new DateTime(2010, 05, 15), status: ActivityStatus.Verified)
            });

            Guid RLComment = Guid.NewGuid();
            Guid RLActivityComment = Guid.NewGuid();
            Guid ADComment = Guid.NewGuid();
 
            mb.Entity<Comment>().HasData(new List<Comment>
            {
                CreateDummyComment(RLComment, "Najlepszy zawodnik!",                    perId: RL),
                CreateDummyComment(Guid.NewGuid(), "No nie wiem. Milik lepszy!",        parentId: RLComment),
                CreateDummyComment(Guid.NewGuid(), "Jest całkiem dobry faktycznie",     parentId: RLComment),
                CreateDummyComment(RLActivityComment, "Fajność!",                       actId: RLactivity),
                CreateDummyComment(Guid.NewGuid(), "Zgadza się!",                       parentId: RLActivityComment),
                CreateDummyComment(Guid.NewGuid(), "Niefajność",                        actId: RLactivity),             
                CreateDummyComment(Guid.NewGuid(), "Lepsza weeeeeersja: https://www.youtube.com/watch?v=vmLonweq6wA", actId: RKactivity),
                CreateDummyComment(ADComment, "Bardzo memiczna osoba",                  perId: AD),
                CreateDummyComment(Guid.NewGuid(), "Hańba!",                            parentId : ADComment),
                CreateDummyComment(Guid.NewGuid(), "Chyba ty",                          parentId : ADComment),
                CreateDummyComment(Guid.NewGuid(), "Ja tam mu nei ufam",                perId: AD),
                CreateDummyComment(Guid.NewGuid(), "Nie lubiem go, bo Andrzej to dziwne imię", perId: AD)
            });

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
                CreateDummyTag(sportTag, "Sport", 1),
                CreateDummyTag(footballTag, "Football"),
                CreateDummyTag(fcbarcelonaTag, "FC Barcelona"),
                CreateDummyTag(f1Tag, "F1"),
                CreateDummyTag(wecTag, "WEC"),
                CreateDummyTag(politicsTag, "Politics", 1),
                CreateDummyTag(pisTag, "PiS"),
                CreateDummyTag(poTag, "PO"),
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
            mb.Entity<Reaction>().HasData(new List<Reaction>
            {
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Hate,           perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.NotUnderstand,  perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Dislike,        perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Like,           perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Love,           perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Love,           perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Love,           perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Love,           perId: RL),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Like,           actId: RLactivity),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.NotUnderstand,  actId: RLactivity),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Like,           actId: RLactivity),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Love,           actId: RLactivity),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Love,           comId: RLComment),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Like,           comId: RLActivityComment),
                CreateDummyReaction(Guid.NewGuid(), ReactionType.Love,           comId: RLActivityComment)
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
            string pass = new PasswordHasher<IdentityUser>().HashPassword(null!, "123");
            SysUser = new IdentityUser { UserName = "sys", PasswordHash = pass, Id = sysUserGuid };
            mb.Entity<IdentityUser>().HasData(SysUser);

            mb.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>()
            {
                UserId = sysUserGuid,
                RoleId = adminRoleGuid
            });
            
        }

        private static Person CreateDummyPerson(Guid id, string firstName, string lastName, DateOnly birthDate, string nationality, DateOnly? deathDate = null, PersonStatus status = PersonStatus.Verified, string desc = "Test desc")
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
                Status = status,

                Created = DateTime.Now,
                CreatedById = SysUser?.Id ?? "sys",
                LastUpdated = DateTime.Now,
                LastUpdatedById = SysUser?.Id ?? "sys"
            };
        }
        private static Tag CreateDummyTag(Guid id, string name, int level = 5, string desc = "Test desc")
        {
            return new Tag
            {
                Id = id,
                Name = name,
                Level = level,
                Description = desc, 
                //Persons = null

                Created = DateTime.Now,
                CreatedById = SysUser?.Id ?? "sys",
                LastUpdated = DateTime.Now,
                LastUpdatedById = SysUser?.Id ?? "sys"
            };
        }
        private static Activity CreateDummyActivity(Guid id, Guid personId, string val, string loc, string desc, string source, DateTime? happend = null, ActivityType type = ActivityType.Quote, ActivityStatus status = ActivityStatus.NotVerified)
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
                CreatedById = SysUser?.Id ?? "sys",
                LastUpdated = DateTime.Now,
                LastUpdatedById = SysUser?.Id ?? "sys"
            };
        }
        private static Comment CreateDummyComment(Guid id, string content, Guid? perId = null, Guid? actId = null, Guid? parentId = null) //, List<Reaction>? rea = null, List<Comment>? chil = null
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
                CreatedById = SysUser?.Id ?? "sys",
                LastUpdated = DateTime.Now,
                LastUpdatedById = SysUser?.Id ?? "sys"
            };
        }
        private static Reaction CreateDummyReaction(Guid id, ReactionType type, Guid? perId = null, Guid? actId = null, Guid? comId = null)
        {
            return new Reaction
            {
                Id = id,
                ReactionType = type,
                PersonId = perId,
                ActivityId = actId,
                CommentId = comId,

                Created = DateTime.Now,
                CreatedById = SysUser?.Id ?? "sys",
                LastUpdated = DateTime.Now,
                LastUpdatedById = SysUser?.Id ?? "sys"
            };
        }   
    }
}
