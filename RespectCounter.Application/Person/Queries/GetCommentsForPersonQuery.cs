using MediatR;
using Microsoft.EntityFrameworkCore;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Queries;

public record GetCommentsForPersonQuery(string personId, int levels) : IRequest<List<CommentDTO>>;

public class GetCommentsForPersonQueryHandler : IRequestHandler<GetCommentsForPersonQuery, List<CommentDTO>>
{
    private readonly IUnitOfWork uow;

    public GetCommentsForPersonQueryHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    } 

    public async Task<List<CommentDTO>> Handle(GetCommentsForPersonQuery request, CancellationToken cancellationToken)
    {
        if(!Guid.TryParse(request.personId, out Guid personGuid))
        {
            throw new FormatException($"Given value for id parameter '{request.personId}' has not a valid guid format.");
        }

        var result = await uow.Repository()
            .FindQueryable<Comment>(c => c.PersonId == personGuid && c.CommentStatus != CommentStatus.Hidden)
            .Include(c => c.Children).Include(c => c.Reactions).Include(c => c.CreatedBy)
            .OrderByDescending(c => c.Created)
            .Select(c => c.ToDTO(request.levels))
            .ToListAsync();
        return result;
    }
}