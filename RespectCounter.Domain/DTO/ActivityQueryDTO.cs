namespace RespectCounter.Domain.DTO;

public record ActivityQueryDTO(
    string Id,
    string PersonId,
    string PersonName,
    string PersonRespect,
    string PersonImagePath,
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
    int Respect 
);