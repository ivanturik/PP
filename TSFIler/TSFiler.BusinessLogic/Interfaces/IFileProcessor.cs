using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Interfaces;

public interface IFileProcessor
{
    bool SupportsFileType(FileType fileType);
    string ReadFile(Stream fileStream);
    void WriteFile(Stream outputStream, string content);
}
