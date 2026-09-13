namespace Medical.Application.Services;

public class ExportService
{
    public string ExportToCsv<T>(IEnumerable<T> data)
    {
        var properties = typeof(T).GetProperties();
        var sb = new StringBuilder();

        // Header
        sb.AppendLine(string.Join(",", properties.Select(p => EscapeCsvValue(p.Name))));

        // Data rows
        foreach (var item in data)
        {
            var values = properties.Select(p =>
            {
                var value = p.GetValue(item);
                return EscapeCsvValue(value?.ToString() ?? "");
            });
            sb.AppendLine(string.Join(",", values));
        }

        return sb.ToString();
    }

    private string EscapeCsvValue(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        return value;
    }

    public byte[] GetCsvBytes(string csvContent)
    {
        return Encoding.UTF8.GetBytes(csvContent);
    }
}
