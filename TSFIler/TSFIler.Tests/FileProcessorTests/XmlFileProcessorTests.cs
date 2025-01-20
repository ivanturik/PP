using System.Text;
using System.Xml;
using System.Xml.Linq;
using TSFiler.BusinessLogic.Services.FileProcessors;
using Xunit;

namespace TSFIler.Tests.FileProcessorTests
{
    public class XmlFileProcessorTests
    {
        [Fact]
        public async Task ReadFileAsync_ValidXml_ReturnsSameContent()
        {
            var processor = new XmlFileProcessor();
            var inputXml = "<root><child>value</child></root>";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(inputXml));
            var result = await processor.ReadFileAsync(stream);

            Assert.Equal(inputXml, result);
        }

        [Fact]
        public async Task ReadFileAsync_EmptyStream_ReturnsEmptyString()
        {
            var processor = new XmlFileProcessor();

            using var stream = new MemoryStream();
            var result = await processor.ReadFileAsync(stream);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task WriteFileAsync_ValidXml_WritesSameContent()
        {
            var processor = new XmlFileProcessor();
            var inputXml = "<root><child>value</child></root>";

            using var memoryStream = new MemoryStream();
            await processor.WriteFileAsync(memoryStream, inputXml);

            memoryStream.Position = 0;
            var writtenContent = new StreamReader(memoryStream).ReadToEnd();

            var originalDoc = XDocument.Parse(inputXml);
            var writtenDoc = XDocument.Parse(writtenContent);

            Assert.True(XNode.DeepEquals(originalDoc, writtenDoc));
        }

        [Fact]
        public async Task WriteFileAsync_InvalidXml_ThrowsException()
        {
            var processor = new XmlFileProcessor();
            var invalidXml = "<root><child>value</child>";

            using var memoryStream = new MemoryStream();

            await Assert.ThrowsAsync<XmlException>(async () =>
            {
                await processor.WriteFileAsync(memoryStream, invalidXml);
            });
        }
    }
}
