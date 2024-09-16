using System.Text.Json;
using TSFiler.BusinessLogic.Interfaces;
using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Services.FileProcessors;

public class JsonFileProcessor : IFileProcessor
{
    public bool SupportsFileType(FileType fileType)
    {
        return fileType == FileType.Json;
    }

    public string ReadFile(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        return reader.ReadToEnd();
    }

    public void WriteFile(Stream outputStream, string content)
    {
        var jsonObject = JsonSerializer.Deserialize<object>(content);
        var jsonFormatted = JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        using var writer = new StreamWriter(outputStream);
        writer.Write(jsonFormatted);
        writer.Flush();
    }
}
