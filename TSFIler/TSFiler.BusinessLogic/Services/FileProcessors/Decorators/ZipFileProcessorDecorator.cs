using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using TSFiler.BusinessLogic.Factories.Interfaces;
using TSFiler.BusinessLogic.Services.Interfaces;
using TSFiler.Common.Helpers;

namespace TSFiler.BusinessLogic.Services.FileProcessors.Decorators
{
    public class ZipFileProcessorDecorator : IFileProcessor
    {
        private readonly IFileProcessorFactory _factory;
        private IFileProcessor _inner;
        private string _innerExt;

        public ZipFileProcessorDecorator(IFileProcessorFactory factory)
        {
            _factory = factory;
        }

        public async Task<string> ReadFileAsync(Stream inputStream)
        {
            using var archive = new ZipArchive(inputStream, ZipArchiveMode.Read, true);
            var entry = archive.Entries.FirstOrDefault();
            if (entry == null) throw new Exception("Empty zip archive.");

            _innerExt = Path.GetExtension(entry.Name);
            var fileType = FileTypeDeterminator.GetFileType(entry.Name);
            _inner = _factory.GetFileProcessor(fileType);

            using var entryStream = entry.Open();
            using var ms = new MemoryStream();
            await entryStream.CopyToAsync(ms);
            ms.Position = 0;

            return await _inner.ReadFileAsync(ms);
        }

        public async Task WriteFileAsync(Stream outputStream, string data)
        {
            if (_inner == null)
            {
                _innerExt = ".txt";
                _inner = _factory.GetFileProcessor(Common.Enums.FileType.Txt);
            }

            using var ms = new MemoryStream();
            await _inner.WriteFileAsync(ms, data);
            ms.Position = 0;

            await ms.CopyToAsync(outputStream);
        }
    }
}
