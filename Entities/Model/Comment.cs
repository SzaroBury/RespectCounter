using Entities.Interfaces;

namespace Entities.Model
{
    public class Comment : IEntity
    {
        public string Content { get; set; } = string.Empty;
        public Activity? Activity { get; set; }
        public Person? Person { get; set; }
        public Comment? Parent { get; set; }
        public List<Reaction> Reactions { get; set; } = new();
        public List<Comment> Children { get; set; } = new();
    }
}
