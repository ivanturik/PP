using TSFiler.BusinessLogic.Interfaces;
using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Services.FileProcessors;

public class PlainTextFileProcessor : IFileProcessor
{
    public bool SupportsFileType(FileType fileType)
    {
        return fileType == FileType.Txt;
    }

    public string ReadFile(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        return reader.ReadToEnd();
    }

    public void WriteFile(Stream outputStream, string content)
    {
        using var writer = new StreamWriter(outputStream);
        writer.Write(content);
        writer.Flush();
    }
}
