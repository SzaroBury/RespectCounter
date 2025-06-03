using System.Security.Claims;
using RespectCounter.API.Requests;
using RespectCounter.Application.Commands;

namespace RespectCounter.API.Mappers;

public static class PersonMappingExtensions
{
    public static AddPersonCommand ToCommand(this ProposePersonRequest request, Guid userId)
    {
        return new AddPersonCommand(
            request.FirstName, 
            request.LastName,
            request.NickName ?? "",
            request.Description ?? "", 
            request.Nationality, 
            request.Birthday.ToNullableDateTime(), 
            request.DeathDate.ToNullableDateTime(), 
            request.Tags,
            userId
        );
    }
}