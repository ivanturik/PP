using TSFiler.BusinessLogic.Interfaces;
using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Services.FileProcessors;

public class PlainTextFileProcessor : IFileProcessor
{
    public bool SupportsFileType(FileType fileType)
    {
        return fileType == FileType.Txt;
    }

    public async Task<string> ReadFileAsync(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        return await reader.ReadToEndAsync();
    }

    public async Task WriteFileAsync(Stream outputStream, string content)
    {
        using var writer = new StreamWriter(outputStream);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
    }
}
