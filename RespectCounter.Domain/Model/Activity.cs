using RespectCounter.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RespectCounter.Domain.Model
{
    public class Activity : IEntity
    {
        public string Value { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Happend { get; set; } = DateTime.MinValue;
        public string Source { get; set; } = string.Empty;
        public bool Verified { get; set; } = false;
        public ActivityType Type { get; set; } = ActivityType.Event;
        public List<Person> Persons { get; set; } = new();
        public List<Reaction> Reactions { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }

    public enum ActivityType
    {
        Event,
        Quote
    }
}
