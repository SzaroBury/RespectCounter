using RespectCounter.Domain.Model;

namespace RespectCounter.Application;

public static class RespectService
{
    private static int defaultRespect = 10;
    public static int CountRespect(List<Reaction> reactions)
    {
        int result = defaultRespect;
        foreach(Reaction r in reactions)
        {
            switch(r.ReactionType)
            {
                case ReactionType.Hate:
                    result -= 2;
                    break;
                case ReactionType.Dislike:
                    result -= 1;
                    break;
                case ReactionType.Like:
                    result += 1;
                    break;
                case ReactionType.Love:
                    result += 2;
                    break;
            }
        }

        return result;
    }

}