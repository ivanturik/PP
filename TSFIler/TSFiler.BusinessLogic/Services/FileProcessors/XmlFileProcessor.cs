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

    public string ReadFile(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        return reader.ReadToEnd();
    }

    public void WriteFile(Stream outputStream, string content)
    {
        XDocument xmlDocument = XDocument.Parse(content);
        xmlDocument.Save(outputStream);
    }
}
