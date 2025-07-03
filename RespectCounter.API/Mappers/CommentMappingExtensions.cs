using RespectCounter.Application.Common;

namespace RespectCounter.API.Mappers;

public static class CommentMappingExtensions
{
    public static CommentSortBy ToCommentSortByEnum(this string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
        {
            return CommentSortBy.MostRespected;
        }

        if (!Enum.TryParse<CommentSortBy>(order, true, out var sortBy))
        {
            var possibleValues = string.Join(", ", Enum.GetNames<CommentSortBy>());
            throw new FormatException($"Invalid order format. Possible values: {possibleValues}");
        }

        return sortBy;
    }
}
