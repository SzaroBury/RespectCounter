namespace RespectCounter.API.Models;

public record CreateActivityModel(
    string Persons,
    string Value, 
    string Description, 
    string Location, 
    string Happend, 
    string Source, 
    int Type, 
    string Tags
);