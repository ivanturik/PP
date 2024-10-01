using TSFiler.BusinessLogic.Factories.Interfaces;
using TSFiler.BusinessLogic.Models;

namespace TSFiler.BusinessLogic.Services;

public class FileService
{
    private readonly IFileProcessorFactory _fileProcessorFactory;
    private readonly IDataProcessorFactory _dataProcessorFactory;

    public FileService(IFileProcessorFactory fileProcessorFactory, IDataProcessorFactory dataProcessorFactory)
    {
        _fileProcessorFactory = fileProcessorFactory;
        _dataProcessorFactory = dataProcessorFactory;
    }

    public async Task ProcessFileAsync(FileInfoModel fileInfo, Stream inputFileStream, Stream outputFileStream)
    {
        var fileProcessor = _fileProcessorFactory.GetFileProcessor(fileInfo.FileType);
        var dataProcessor = _dataProcessorFactory.GetDataProcessor(fileInfo.ProcessType);

        string data = await fileProcessor.ReadFileAsync(inputFileStream);
        string processedData = dataProcessor.ProcessData(data);
        await fileProcessor.WriteFileAsync(outputFileStream, processedData);
    }
}
