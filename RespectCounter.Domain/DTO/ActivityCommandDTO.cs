namespace RespectCounter.Domain.DTO;

public record ActivityCommandDTO(
    string Persons,
    string Value, 
    string Description, 
    string Location, 
    string Happend, 
    string Source, 
    int Type, 
    string Tags
);