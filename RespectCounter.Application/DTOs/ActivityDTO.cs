using RespectCounter.Domain.Model;

namespace RespectCounter.Application.DTOs;

public record ActivityDTO(
    string Id,
    string PersonId,
    string PersonFullName,
    int PersonRespect,
    string PersonImagePath,
    string Status,
    string CreatedBy,
    string CreatedById,
    string Value,
    string Description, 
    string Location, 
    string Source, 
    string Tags,
    string Happend, 
    int CommentsCount,
    int Type, 
    int Respect,
    ReactionType? CurrentUsersReaction
);