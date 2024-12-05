using System.ComponentModel.DataAnnotations.Schema;

namespace RespectCounter.Domain.Model
{
    public class Comment : Entity
    {
        public string Content { get; set; } = string.Empty;
        public CommentStatus CommentStatus { get; set; } = CommentStatus.Created;
        
        public Guid? ActivityId { get; set; }
        public virtual Activity? Activity { get; set; }
        public Guid? PersonId { get; set; }
        public virtual Person? Person { get; set; }
        [ForeignKey("Comment")]
        public Guid? ParentId { get; set; }
        public virtual Comment? Parent { get; set; }
        public virtual List<Reaction> Reactions { get; set; } = new();
        public virtual List<Comment> Children { get; set; } = new();
    }

    public enum CommentStatus
    {
        Created,
        Edited,
        Hidden
    }
}
