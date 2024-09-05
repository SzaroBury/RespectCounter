using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Domain.Model
{
    public class Reaction : IEntity
    {
        public ReactionType ReactionType { get; set; } = ReactionType.Like;

        public int? ActivityId { get; set; }
        public Activity? Activity { get; set; }
        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }
        public int? PersonId { get; set; }
        public Person? Person { get; set; }
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
