using System.Security.Claims;
using RespectCounter.API.Requests;
using RespectCounter.Application.Commands;
using RespectCounter.Application.Common;

namespace RespectCounter.API.Mappers;

public static class PersonMappingExtensions
{
    public static AddPersonCommand ToCommand(this ProposePersonRequest request, Guid userId)
    {
        return new AddPersonCommand(
            request.FirstName,
            request.LastName,
            request.NickName ?? "",
            request.Profession,
            request.Description ?? "",
            request.Nationality,
            request.Birthday.ToNullableDateTime(),
            request.DeathDate.ToNullableDateTime(),
            request.Tags,
            userId
        );
    }
    
    public static PersonSortBy ToPersonSortByEnum(this string? order)
    {
        if(string.IsNullOrWhiteSpace(order))
        {
            return PersonSortBy.Trending;
        }

        if (!Enum.TryParse<PersonSortBy>(order, true, out var sortBy))
        {
            var possibleValues = string.Join(", ", Enum.GetNames<PersonSortBy>());
            throw new FormatException($"Invalid order format. Possible values: {possibleValues}");
        }

        return sortBy;
    }
}