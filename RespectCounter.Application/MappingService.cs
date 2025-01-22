using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;

namespace RespectCounter.Application;

public static class MappingService
{
    public static ActivityDTO ToDTO(this Activity a)
    {
        return new ActivityDTO(
            a.Id.ToString(),
            a.Person?.Id.ToString() ?? "??",
            $"{a.Person?.FirstName ?? "?"} {a.Person?.LastName ?? "?"}",
            RespectService.CountRespect(a.Person?.Reactions ?? []),
            $"person_{a.Person?.LastName}.jpg",
            a.CreatedBy?.UserName ?? "??",
            a.CreatedById,
            a.Value,
            a.Description,
            a.Location,
            a.Source,
            string.Join(",", a.Tags.Select(t => t.Name)),
            a.Happend?.ToLongDateString() ?? "",
            a.Comments.Count + a.Comments.Sum(c => c.ChildrenCount),
            (int)a.Type,
            RespectService.CountRespect(a.Reactions)
        );
    } 

    public static CommentDTO ToCommentDTO(this Comment c, int levelsToSearch)
    {
        return new CommentDTO(
            c.Id.ToString(),
            c.CreatedBy?.UserName ?? "??",
            c.CreatedById,
            c.Content, 
            c.ActivityId?.ToString() ?? "",
            c.PersonId?.ToString() ?? "",
            c.ParentId?.ToString() ?? "",
            (int)c.CommentStatus,
            RespectService.CountRespect(c.Reactions),
            c.ChildrenCount,
            levelsToSearch > 0 ? c.Children.Select(ch => ch.ToCommentDTO(levelsToSearch - 1)).ToList() : []
        );
    }
}