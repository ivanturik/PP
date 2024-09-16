using Microsoft.AspNetCore.Mvc;
using TSFiler.BusinessLogic.Models;
using TSFiler.BusinessLogic.Services;
using TSFiler.Common.Enums;

namespace TSFiler.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly FileService _fileService;

    public FileController(FileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessFile([FromQuery] string outputFileName, [FromQuery] FileType FileType, [FromQuery] ProcessType ProcessType, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Файл не был загружен.");
        }

        FileInfoModel fileInfo = new FileInfoModel
        {
            OutputFileName = outputFileName,
            FileType = FileType,
            ProcessType = ProcessType,
        };

        using var inputStream = file.OpenReadStream();
        using var outputStream = new MemoryStream();

        await _fileService.ProcessFileAsync(fileInfo, inputStream, outputStream);

        var mimeType = fileInfo.FileType switch
        {
            FileType.Json => "application/json",
            FileType.Xml => "application/xml",
            FileType.Yaml => "application/x-yaml",
            _ => "text/plain"
        };

        return File(outputStream.ToArray(), mimeType, $"{fileInfo.OutputFileName}.{fileInfo.FileType.ToString().ToLower()}");
    }
}
