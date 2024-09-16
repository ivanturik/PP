using System.Xml.Linq;
using TSFiler.BusinessLogic.Interfaces;
using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Services.FileProcessors;

public class XmlFileProcessor : IFileProcessor
{
    public bool SupportsFileType(FileType fileType)
    {
        return fileType == FileType.Xml;
    }

    public async Task<string> ReadFileAsync(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        return await reader.ReadToEndAsync();
    }

    public async Task WriteFileAsync(Stream outputStream, string content)
    {
        var xmlDocument = XDocument.Parse(content);
        await Task.Run(() => xmlDocument.Save(outputStream));
    }
}
