using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using TSFiler.BusinessLogic.Services.FileProcessors;
using Xunit;

namespace TSFIler.Tests.FileProcessorTests
{
    public class JsonFileProcessorTests
    {
        [Fact]
        public async Task ReadFileAsync_ValidJson_ReturnsSameContent()
        {
            var processor = new JsonFileProcessor();
            var expectedJson = "{\"name\":\"Test\",\"value\":123}";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(expectedJson));
            var result = await processor.ReadFileAsync(stream);

            Assert.Equal(expectedJson, result);
        }

        [Fact]
        public async Task ReadFileAsync_EmptyStream_ReturnsEmptyString()
        {
            var processor = new JsonFileProcessor();

            using var stream = new MemoryStream();
            var result = await processor.ReadFileAsync(stream);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task WriteFileAsync_ValidJson_WritesIndentedJson()
        {
            var processor = new JsonFileProcessor();
            var inputJson = "{\"name\":\"Test\",\"items\":[1,2,3]}";

            using var outputStream = new MemoryStream();
            await processor.WriteFileAsync(outputStream, inputJson);

            outputStream.Position = 0;
            var writtenContent = new StreamReader(outputStream).ReadToEnd();

            using var doc = JsonDocument.Parse(writtenContent);
            Assert.NotNull(doc.RootElement);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var expectedIndented = JsonSerializer.Serialize(JsonSerializer.Deserialize<JsonElement>(inputJson), options);

            Assert.Equal(expectedIndented, writtenContent);
        }

        [Fact]
        public async Task WriteFileAsync_InvalidJson_ThrowsJsonException()
        {
            var processor = new JsonFileProcessor();
            var invalidJson = "{\"name\":\"Test\", \"invalid\"}";

            using var outputStream = new MemoryStream();
            await Assert.ThrowsAsync<JsonException>(() => processor.WriteFileAsync(outputStream, invalidJson));
        }
    }
}
