using TSFiler.BusinessLogic.Interfaces;
using TSFiler.BusinessLogic.Models;

namespace TSFiler.BusinessLogic.Services
{
    public class FileService
    {
        private readonly IEnumerable<IFileProcessor> _fileProcessors;
        private readonly IEnumerable<IDataProcessor> _dataProcessors;

        public FileService(IEnumerable<IFileProcessor> fileProcessors, IEnumerable<IDataProcessor> dataProcessors)
        {
            _fileProcessors = fileProcessors;
            _dataProcessors = dataProcessors;
        }

        public async Task ProcessFileAsync(FileInfoModel fileInfo, Stream inputFileStream, Stream outputFileStream)
        {
            var fileProcessor = _fileProcessors.FirstOrDefault(p => p.SupportsFileType(fileInfo.FileType));
            if (fileProcessor == null)
            {
                throw new NotSupportedException($"File processor for {fileInfo.FileType} not found.");
            }

            var dataProcessor = _dataProcessors.FirstOrDefault(p => p.SupportsProcessType(fileInfo.ProcessType));
            if (dataProcessor == null)
            {
                throw new NotSupportedException($"Data processor for {fileInfo.ProcessType} not found.");
            }

            string data = await fileProcessor.ReadFileAsync(inputFileStream);
            string processedData = dataProcessor.ProcessData(data);
            await fileProcessor.WriteFileAsync(outputFileStream, processedData);
        }
    }
}
