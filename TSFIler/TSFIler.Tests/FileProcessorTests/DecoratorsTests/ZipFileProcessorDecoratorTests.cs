using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using TSFiler.BusinessLogic.Factories.Interfaces;
using TSFiler.BusinessLogic.Services.FileProcessors.Decorators;
using TSFiler.BusinessLogic.Services.Interfaces;
using TSFiler.Common.Enums;
using TSFiler.Common.Helpers;

namespace TSFiler.Tests
{
    public class ZipFileProcessorDecoratorTests
    {
        [Fact]
        public async Task ReadFileAsync_ZipWithJsonEntry_ShouldCallInnerProcessorAndReturnResult()
        {
            var jsonData = "{\"key\":\"value\"}";
            var zipStream = CreateZipStream("test.json", jsonData);
            var factoryMock = new Mock<IFileProcessorFactory>();
            var innerProcessorMock = new Mock<IFileProcessor>();
            innerProcessorMock
                .Setup(x => x.ReadFileAsync(It.IsAny<Stream>()))
                .ReturnsAsync("OK");
            factoryMock
                .Setup(x => x.GetFileProcessor(FileType.Json))
                .Returns(innerProcessorMock.Object);

            var decorator = new ZipFileProcessorDecorator(factoryMock.Object);

            var result = await decorator.ReadFileAsync(zipStream);

            Assert.Equal("OK", result);
            innerProcessorMock.Verify(x => x.ReadFileAsync(It.IsAny<Stream>()), Times.Once);
        }

        [Fact]
        public async Task WriteFileAsync_ShouldCallInnerProcessorAndWriteCleanData()
        {
            var factoryMock = new Mock<IFileProcessorFactory>();
            var innerProcessorMock = new Mock<IFileProcessor>();
            innerProcessorMock
                .Setup(x => x.WriteFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            factoryMock
                .Setup(x => x.GetFileProcessor(It.IsAny<FileType>()))
                .Returns(innerProcessorMock.Object);

            var decorator = new ZipFileProcessorDecorator(factoryMock.Object);

            using var output = new MemoryStream();
            await decorator.WriteFileAsync(output, "SOME_DATA");
            innerProcessorMock.Verify(x => x.WriteFileAsync(It.IsAny<Stream>(), "SOME_DATA"), Times.Once);
        }

        private MemoryStream CreateZipStream(string entryName, string content)
        {
            var ms = new MemoryStream();
            using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                var entry = archive.CreateEntry(entryName);
                using var entryStream = entry.Open();
                var bytes = Encoding.UTF8.GetBytes(content);
                entryStream.Write(bytes, 0, bytes.Length);
            }
            ms.Position = 0;
            return ms;
        }
    }
}
