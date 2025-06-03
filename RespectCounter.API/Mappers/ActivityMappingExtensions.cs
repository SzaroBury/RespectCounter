using System.Security.Claims;
using RespectCounter.API.Requests;
using RespectCounter.Application.Commands;

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
}