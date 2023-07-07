using Entities.Interfaces;

namespace Entities.Model
{
    public class Activity : IEntity
    {
        public string Value { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Happend { get; set; } = DateTime.MinValue;
        public string Source { get; set; } = string.Empty;
        public bool Verified { get; set; } = false;
        public bool IsQuote { get; set; } = false;
        public Person Person { get; set; } = new();
        public List<Reaction> Reactions { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }
}
