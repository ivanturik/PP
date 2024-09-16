using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Interfaces;

public interface IFileProcessor
{
    bool SupportsFileType(FileType fileType);
    Task<string> ReadFileAsync(Stream fileStream);
    Task WriteFileAsync(Stream outputStream, string content);
}
