using System.ComponentModel.DataAnnotations.Schema;
using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Domain.Model
{
    public class Comment : IEntity
    {
        public string Content { get; set; } = string.Empty;
        
        public int? ActivityId { get; set; }
        public virtual Activity? Activity { get; set; }
        public int? PersonId { get; set; }
        public virtual Person? Person { get; set; }
        [ForeignKey("Comment")]
        public int? ParentId { get; set; }
        public virtual Comment? Parent { get; set; }
        public virtual List<Reaction> Reactions { get; set; } = new();
        public virtual List<Comment> Children { get; set; } = new();
    }
}
