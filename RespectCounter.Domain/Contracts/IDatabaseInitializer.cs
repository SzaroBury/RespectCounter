namespace RespectCounter.Domain.Contracts;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
}