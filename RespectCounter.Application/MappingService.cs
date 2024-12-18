using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;

namespace RespectCounter.Application;

public static class MappingService
{
    public static ActivityQueryDTO MapActivityToQueryDTO(Activity a)
    {
        return new ActivityQueryDTO(
            a.Id.ToString(),
            string.Join(",", a.Persons.Select(p => p.Id.ToString())),
            string.Join(",", a.Persons.Select(p => p.FirstName + " " + p.LastName)),
            string.Join(",", a.Persons.Select(p => RespectService.CountRespect(p.Reactions))),
            string.Join(",", a.Persons.Select(p => "/persons/person_" + p.LastName.ToLower() + ".jpg")),
            a.CreatedBy?.UserName ?? "??",
            a.CreatedById,
            a.Value,
            a.Description,
            a.Location,
            a.Source,
            string.Join(",", a.Tags),
            a.Happend.ToLongDateString(),
            a.Comments.Count,
            (int)a.Type,
            RespectService.CountRespect(a.Reactions)
        );
    } 

    public static CommentDTO MapCommentToDTO(Comment c)
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
            c.Children.Count,
            0
        );
    }
}