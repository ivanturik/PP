using System.Text;
using TSFiler.BusinessLogic.Services.FileProcessors;
using Xunit;

namespace TSFIler.Tests.FileProcessorTests
{
    public class PlainTextFileProcessorTests
    {
        [Fact]
        public async Task ReadFileAsync_ValidText_ReturnsSameContent()
        {
            var processor = new PlainTextFileProcessor();
            var expectedText = "Some test content";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(expectedText));
            var result = await processor.ReadFileAsync(stream);

            Assert.Equal(expectedText, result);
        }

        [Fact]
        public async Task ReadFileAsync_EmptyStream_ReturnsEmptyString()
        {
            var processor = new PlainTextFileProcessor();

            using var stream = new MemoryStream();
            var result = await processor.ReadFileAsync(stream);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task WriteFileAsync_ValidText_WritesSameContent()
        {
            var processor = new PlainTextFileProcessor();
            var inputText = "Another test content";

            using var memoryStream = new MemoryStream();
            await processor.WriteFileAsync(memoryStream, inputText);

            memoryStream.Position = 0;
            var writtenContent = new StreamReader(memoryStream).ReadToEnd();

            Assert.Equal(inputText, writtenContent);
        }

        [Fact]
        public async Task WriteFileAsync_EmptyString_WritesNothing()
        {
            var processor = new PlainTextFileProcessor();
            var inputText = string.Empty;

            using var memoryStream = new MemoryStream();
            await processor.WriteFileAsync(memoryStream, inputText);

            memoryStream.Position = 0;
            var writtenContent = new StreamReader(memoryStream).ReadToEnd();

            Assert.Equal(inputText, writtenContent);
            Assert.Equal(0, memoryStream.Length);
        }
    }
}
