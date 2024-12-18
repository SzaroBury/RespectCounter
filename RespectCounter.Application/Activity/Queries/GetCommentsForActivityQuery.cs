using MediatR;
using Microsoft.EntityFrameworkCore;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Infrastructure.Repositories;

namespace RespectCounter.Application.Queries;

public record GetCommentsForActivityQuery(string actId, int level) : IRequest<List<CommentDTO>>;

public class GetCommentsForActivityQueryHandler : IRequestHandler<GetCommentsForActivityQuery, List<CommentDTO>>
{
    private readonly IUnitOfWork uow;

    public GetCommentsForActivityQueryHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    } 

    public async Task<List<CommentDTO>> Handle(GetCommentsForActivityQuery request, CancellationToken cancellationToken)
    {
        if(!Guid.TryParse(request.actId, out Guid actGuid))
        {
            throw new FormatException($"Given value for id parameter '{request.actId}' has not a valid guid format.");
        }

        var result = await uow.Repository().FindQueryable<Comment>(c => c.ActivityId == actGuid && c.CommentStatus != CommentStatus.Hidden)
            .Include("Children").Include("Reactions").Include("CreatedBy").Select(c => MappingService.MapCommentToDTO(c))
            .ToListAsync();
        return result;
    }
}