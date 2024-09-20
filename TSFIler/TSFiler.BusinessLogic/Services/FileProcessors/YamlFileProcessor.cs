using TSFiler.BusinessLogic.Interfaces;
using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Services.FileProcessors;

public class YamlFileProcessor : IFileProcessor
{
    public bool SupportsFileType(FileType fileType)
    {
        return fileType == FileType.Yaml;
    }

    public async Task<string> ReadFileAsync(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        var fileData = await reader.ReadToEndAsync();
        return fileData;
    }

    public async Task WriteFileAsync(Stream outputStream, string content)
    {
        using var writer = new StreamWriter(outputStream);
        await writer.WriteAsync(content);
    }
}
