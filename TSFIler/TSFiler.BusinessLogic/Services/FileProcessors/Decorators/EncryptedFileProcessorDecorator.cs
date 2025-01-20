using System;
using System.IO;
using System.Threading.Tasks;
using TSFiler.BusinessLogic.Factories.Interfaces;
using TSFiler.BusinessLogic.Services.Interfaces;

namespace TSFiler.BusinessLogic.Services.FileProcessors.Decorators
{
    public class EncryptedFileProcessorDecorator : IFileProcessor
    {
        private readonly IFileProcessorFactory _factory;
        private IFileProcessor _inner;
        private string _innerExt;

        public EncryptedFileProcessorDecorator(IFileProcessorFactory factory)
        {
            _factory = factory;
        }

        public async Task<string> ReadFileAsync(Stream inputStream)
        {
            using var ms = new MemoryStream();
            await DecryptAsync(inputStream, ms);
            ms.Position = 0;
            _innerExt = ".txt";
            _inner = _factory.GetFileProcessor(Common.Enums.FileType.Txt);
            return await _inner.ReadFileAsync(ms);
        }

        public async Task WriteFileAsync(Stream outputStream, string data)
        {
            using var ms = new MemoryStream();
            if (_inner == null)
            {
                _innerExt = ".txt";
                _inner = _factory.GetFileProcessor(Common.Enums.FileType.Txt);
            }
            await _inner.WriteFileAsync(ms, data);
            ms.Position = 0;
            await EncryptAsync(ms, outputStream);
        }

        private async Task DecryptAsync(Stream input, Stream output)
        {
            await input.CopyToAsync(output);
        }

        private async Task EncryptAsync(Stream input, Stream output)
        {
            await input.CopyToAsync(output);
        }
    }
}
