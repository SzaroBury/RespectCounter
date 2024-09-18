using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Domain.Model
{
    public class Reaction : Entity
    {
        public ReactionType ReactionType { get; set; } = ReactionType.Like;

        public Guid? ActivityId { get; set; }
        public virtual Activity? Activity { get; set; }
        public Guid? CommentId { get; set; }
        public virtual Comment? Comment { get; set; }
        public Guid? PersonId { get; set; }
        public virtual Person? Person { get; set; }
    }

    public enum ReactionType
    {
        Hate,
        Dislike,
        NotUnderstand,
        Like,
        Love
    }
}
