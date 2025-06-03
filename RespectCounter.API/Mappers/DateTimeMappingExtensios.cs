namespace RespectCounter.API.Mappers;

public static class DateTimeMappingExtensions
{
    public static DateTime ToDateTime(this string dateTimeString)
    {
        if (DateTime.TryParse(dateTimeString, out var dateTime))
        {
            throw new FormatException("Invalid DateTime format");
        }

        return dateTime;
    }

    public static DateTime? ToNullableDateTime(this string? dateTimeString)
    {
        if (string.IsNullOrWhiteSpace(dateTimeString))
        {
            return null;
        }
        
        if (DateTime.TryParse(dateTimeString, out var dateTime))
        {
            return dateTime;
        }

        return null;
    }
}