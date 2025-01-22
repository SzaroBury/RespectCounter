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

    public static PersonDTO ToDTO(this Person p)
    {
        return new PersonDTO(
            p.Id.ToString(),
            p.FirstName,
            p.LastName,
            p.NickName,
            $"{p.FirstName} {p.LastName}" + (string.IsNullOrEmpty(p.NickName) ? "" : $" ({p.NickName})"),
            p.Description,
            $"person_{p.Id}.jpg",
            p.Nationality,
            p.Birthday?.ToString() ?? "",
            p.DeathDate?.ToString() ?? "",
            p.CreatedBy?.UserName ?? "??",
            p.CreatedById, 
            p.Tags.OrderByDescending(t => t.Count).Select(t => t.ToSimpleDTO()).ToList(),
            p.Activities.Count,
            RespectService.CountRespect(p.Reactions)
        );
    }

    public static CommentDTO ToDTO(this Comment c, int levelsToSearch)
    {
        return new CommentDTO(
            c.Id.ToString(),
            c.CreatedBy?.UserName ?? "??",
            c.CreatedById,
            c.Created.ToString("o"),
            c.Content, 
            c.ActivityId?.ToString() ?? "",
            c.PersonId?.ToString() ?? "",
            c.ParentId?.ToString() ?? "",
            (int)c.CommentStatus,
            RespectService.CountRespect(c.Reactions),
            c.ChildrenCount,
            levelsToSearch > 0 ? 
                c.Children.OrderByDescending(ch => ch.Created).Select(ch => ch.ToDTO(levelsToSearch - 1)).ToList() 
                : []
        );
    }

    public static SimplePersonDTO ToSimpleDTO(this Person p)
    {
        return new SimplePersonDTO(
            p.Id.ToString(),
            $"{p.FirstName} {p.LastName}" + (string.IsNullOrEmpty(p.NickName) ? "" : $" ({p.NickName})")
        );
    }

    public static SimpleTagDTO ToSimpleDTO(this Tag t)
    {
        return new SimpleTagDTO(
            t.Id.ToString(),
            t.Name
        );
    }
}