using System.Text;
using TSFiler.BusinessLogic.Services.FileProcessors;
using Xunit;

namespace TSFIler.Tests.FileProcessorTests
{
    public class YamlFileProcessorTests
    {
        [Fact]
        public async Task ReadFileAsync_ValidYaml_ReturnsSameContent()
        {
            var processor = new YamlFileProcessor();
            var yamlContent = "name: Test\nvalue: 123";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(yamlContent));
            var result = await processor.ReadFileAsync(stream);

            Assert.Equal(yamlContent, result);
        }

        [Fact]
        public async Task ReadFileAsync_EmptyStream_ReturnsEmptyString()
        {
            var processor = new YamlFileProcessor();

            using var stream = new MemoryStream();
            var result = await processor.ReadFileAsync(stream);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task WriteFileAsync_ValidYaml_WritesSameContent()
        {
            var processor = new YamlFileProcessor();
            var yamlContent = "root:\n  child: value";

            using var memoryStream = new MemoryStream();
            await processor.WriteFileAsync(memoryStream, yamlContent);

            memoryStream.Position = 0;
            var writtenContent = new StreamReader(memoryStream).ReadToEnd();

            Assert.Equal(yamlContent, writtenContent);
        }

        [Fact]
        public async Task WriteFileAsync_EmptyString_WritesNothing()
        {
            var processor = new YamlFileProcessor();
            var yamlContent = string.Empty;

            using var memoryStream = new MemoryStream();
            await processor.WriteFileAsync(memoryStream, yamlContent);

            memoryStream.Position = 0;
            var writtenContent = new StreamReader(memoryStream).ReadToEnd();

            Assert.Equal(yamlContent, writtenContent);
            Assert.Equal(0, memoryStream.Length);
        }
    }
}
