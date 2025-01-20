using System.IO;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using TSFiler.BusinessLogic.Factories.Interfaces;
using TSFiler.BusinessLogic.Services.FileProcessors.Decorators;
using TSFiler.BusinessLogic.Services.Interfaces;
using TSFiler.Common.Enums;

namespace TSFiler.Tests
{
    public class EncryptedFileProcessorDecoratorTests
    {
        [Fact]
        public async Task ReadFileAsync_ShouldDecryptAndCallInnerProcessor()
        {
            var factoryMock = new Mock<IFileProcessorFactory>();
            var innerProcessorMock = new Mock<IFileProcessor>();
            innerProcessorMock
                .Setup(x => x.ReadFileAsync(It.IsAny<Stream>()))
                .ReturnsAsync("DECRYPTED_DATA");
            factoryMock
                .Setup(x => x.GetFileProcessor(FileType.Txt))
                .Returns(innerProcessorMock.Object);

            var decorator = new EncryptedFileProcessorDecorator(factoryMock.Object);

            using var input = new MemoryStream(Encoding.UTF8.GetBytes("ENCRYPTED_DATA"));
            var result = await decorator.ReadFileAsync(input);

            Assert.Equal("DECRYPTED_DATA", result);
            innerProcessorMock.Verify(x => x.ReadFileAsync(It.IsAny<Stream>()), Times.Once);
        }

        [Fact]
        public async Task WriteFileAsync_ShouldEncryptAndCallInnerProcessor()
        {
            var factoryMock = new Mock<IFileProcessorFactory>();
            var innerProcessorMock = new Mock<IFileProcessor>();
            factoryMock
                .Setup(x => x.GetFileProcessor(FileType.Txt))
                .Returns(innerProcessorMock.Object);

            var decorator = new EncryptedFileProcessorDecorator(factoryMock.Object);

            using var output = new MemoryStream();
            await decorator.WriteFileAsync(output, "PLAINTEXT_DATA");

            innerProcessorMock.Verify(x => x.WriteFileAsync(It.IsAny<Stream>(), "PLAINTEXT_DATA"), Times.Once);
        }
    }
}
