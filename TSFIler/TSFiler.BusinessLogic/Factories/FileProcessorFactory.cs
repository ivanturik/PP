using Microsoft.Extensions.DependencyInjection;
using TSFiler.BusinessLogic.Factories.Interfaces;
using TSFiler.BusinessLogic.Services.FileProcessors;
using TSFiler.BusinessLogic.Services.FileProcessors.Decorators;
using TSFiler.BusinessLogic.Services.Interfaces;
using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Factories;

public class FileProcessorFactory : IFileProcessorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public FileProcessorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IFileProcessor GetFileProcessor(FileType fileType)
    {
        switch (fileType)
        {
            case FileType.Txt:
                return _serviceProvider.GetRequiredService<PlainTextFileProcessor>();
            case FileType.Json:
                return _serviceProvider.GetRequiredService<JsonFileProcessor>();
            case FileType.Xml:
                return _serviceProvider.GetRequiredService<XmlFileProcessor>();
            case FileType.Yaml:
                return _serviceProvider.GetRequiredService<YamlFileProcessor>();
            case FileType.Zip:
                return new ZipFileProcessorDecorator(_serviceProvider.GetRequiredService<IFileProcessorFactory>());
            case FileType.Rar:
                return new RarFileProcessorDecorator(_serviceProvider.GetRequiredService<IFileProcessorFactory>());
            case FileType.Enc:
                return new EncryptedFileProcessorDecorator(_serviceProvider.GetRequiredService<IFileProcessorFactory>());
            default:
                throw new NotSupportedException($"Обработчик для {fileType} не найден.");
        }
    }


}
