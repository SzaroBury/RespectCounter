using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Domain.Model
{
    public class Reaction : IEntity
    {
        public ReactionType ReactionType { get; set; } = ReactionType.Like;

        public int? ActivityId { get; set; }
        public virtual Activity? Activity { get; set; }
        public int? CommentId { get; set; }
        public virtual Comment? Comment { get; set; }
        public int? PersonId { get; set; }
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
