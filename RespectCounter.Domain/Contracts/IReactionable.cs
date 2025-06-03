using RespectCounter.Domain.Model;

namespace RespectCounter.Domain.Contracts;

public interface IReactionable
{
    public List<Reaction> Reactions { get; set; }
}