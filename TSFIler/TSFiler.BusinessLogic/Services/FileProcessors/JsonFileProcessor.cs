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

    public async Task<string> ReadFileAsync(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        var fileData = await reader.ReadToEndAsync();
        return fileData;
    }

    public async Task WriteFileAsync(Stream outputStream, string content)
    {
        var jsonObject = JsonSerializer.Deserialize<JsonElement>(content);
        var jsonFormatted = JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        using var writer = new StreamWriter(outputStream);
        await writer.WriteAsync(jsonFormatted);
        await writer.FlushAsync();
    }
}
