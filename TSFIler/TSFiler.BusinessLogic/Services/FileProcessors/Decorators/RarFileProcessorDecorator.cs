using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using TSFiler.BusinessLogic.Factories.Interfaces;
using TSFiler.BusinessLogic.Services.Interfaces;
using TSFiler.Common.Helpers;

namespace TSFiler.BusinessLogic.Services.FileProcessors.Decorators
{
    public class RarFileProcessorDecorator : IFileProcessor
    {
        private readonly IFileProcessorFactory _factory;
        private IFileProcessor _inner;
        private string _innerExt;

        public RarFileProcessorDecorator(IFileProcessorFactory factory)
        {
            _factory = factory;
        }

        public async Task<string> ReadFileAsync(Stream inputStream)
        {
            using var archive = RarArchive.Open(inputStream);
            var entry = archive.Entries.FirstOrDefault(e => !e.IsDirectory);
            if (entry == null) throw new Exception("Empty RAR archive.");

            _innerExt = Path.GetExtension(entry.Key);
            var fileType = FileTypeDeterminator.GetFileType(entry.Key);
            _inner = _factory.GetFileProcessor(fileType);

            using var ms = new MemoryStream();
            entry.WriteTo(ms);
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
