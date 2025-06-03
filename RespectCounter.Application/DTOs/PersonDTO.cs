using RespectCounter.Domain.Model;

namespace RespectCounter.Application.DTOs;

public record PersonDTO(
    string Id,
    string FirstName,
    string LastName,
    string NickName,
    string FullName,
    string Description,
    string ImagePath,
    string Nationality,
    string Birthday,
    string DeathDate,
    string CreatedBy,
    string CreatedById, 
    List<SimpleTagDTO> Tags,
    int ActivitiesCount,
    int Respect,
    ReactionType? CurrentUsersReaction
);