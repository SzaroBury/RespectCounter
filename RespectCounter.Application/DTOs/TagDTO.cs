namespace RespectCounter.Application.DTOs;

public record TagDTO(
    string Name,
    string Description,
    int CountActivities,
    int CountPersons,
    int Count
);
