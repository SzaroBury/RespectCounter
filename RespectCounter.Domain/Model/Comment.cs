using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Domain.Model
{
    public class Comment : IEntity
    {
        public string Content { get; set; } = string.Empty;
        
        public int? ActivityId { get; set; }
        public Activity? Activity { get; set; }
        public int? PersonId { get; set; }
        public Person? Person { get; set; }
        public int? ParentId { get; set; }
        public Comment? Parent { get; set; }
        public List<Reaction> Reactions { get; set; } = new();
        public List<Comment> Children { get; set; } = new();
    }
}
