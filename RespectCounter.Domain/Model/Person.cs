using RespectCounter.Domain.Interfaces;
using System.Net;

namespace RespectCounter.Domain.Model
{
    public class Person : IEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public DateTime Birthday { get; set; } = DateTime.MinValue;
        public DateTime DeathDate { get; set; } = DateTime.MinValue;
        public bool Verified { get; set; } = false;
        public float PublicScore { get; set; } = 5.0f;
        public List<Activity> Activities { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public List<Reaction> Reactions { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();
    }
}