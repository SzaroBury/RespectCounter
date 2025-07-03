using RespectCounter.API.Requests;
using RespectCounter.Application.Commands;
using RespectCounter.Application.Common;
using RespectCounter.Domain.Model;

namespace RespectCounter.API.Mappers;

public static class ActivityMappingExtensions
{
    public static AddActivityCommand ToCommand(this ProposeActivityRequest request, Guid userId)
    {
        return new AddActivityCommand(
            request.PersonId,
            request.Title,
            request.Description ?? "",
            request.Location ?? "",
            request.Happend ?? "",
            request.Source,
            request.Type,
            request.Tags,
            userId
        );
    }

    public static ActivitySortBy ToActivitySortByEnum(this string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
        {
            return ActivitySortBy.Trending;
        }

        if (!Enum.TryParse<ActivitySortBy>(order, true, out var sortBy))
        {
            var possibleValues = string.Join(", ", Enum.GetNames<ActivitySortBy>());
            throw new FormatException($"Invalid order format. Possible values: {possibleValues}");
        }

        return sortBy;
    }

    public static ActivityType? ToActivityTypeEnum(this string? typeInput)
    {
        if (string.IsNullOrWhiteSpace(typeInput))
        {
            return null;
        }

        if (!Enum.TryParse<ActivityType>(typeInput, true, out var type))
        {
            var possibleValues = string.Join(", ", Enum.GetNames<ActivityType>());
            throw new FormatException($"Invalid order format. Possible values: {possibleValues}");
        }

        return type;
    }

    public static List<ActivityStatus> ToActivityStatusList(this bool? onlyVerified)
    {
        var result = new List<ActivityStatus>();
        if (onlyVerified.HasValue && onlyVerified.Value)
        {
            result = [ActivityStatus.Verified];
        }
        return result;
    }
}