using Entities.Interfaces;

namespace Entities.Model
{
    public class Reaction : IEntity
    {
        public ReactionType ReactionType { get; set; } = ReactionType.Like;
        public Activity? Activity { get; set; }
        public Comment? Comment { get; set; }
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
