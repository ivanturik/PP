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
    private readonly ILogger<FileController> _logger;

    public FileController(FileService fileService, ILogger<FileController> logger)
    {
        _fileService = fileService;
        _logger = logger;
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessFile([FromQuery] string outputFileName, [FromQuery] FileType fileType, [FromQuery] ProcessType processType, IFormFile file)
    {
        _logger.LogInformation("Получен файл: {FileName}, Имя выходного файла: {OutputFileName}, Тип файла: {FileType}, Тип обработки: {ProcessType}",
            file.FileName, outputFileName, fileType, processType);

        if (file == null || file.Length == 0)
        {
            return BadRequest("Файл не был загружен.");
        }

        var fileInfo = new FileInfoModel
        {
            OutputFileName = outputFileName,
            FileType = fileType,
            ProcessType = processType
        };

        using var inputStream = file.OpenReadStream();
        using var outputStream = new MemoryStream();

        await _fileService.ProcessFileAsync(fileInfo, inputStream, outputStream);

        var mimeType = fileType switch
        {
            FileType.Json => "application/json",
            FileType.Xml => "application/xml",
            FileType.Yaml => "application/x-yaml",
            _ => "text/plain"
        };

        return File(outputStream.ToArray(), mimeType, $"{fileInfo.OutputFileName}.{fileInfo.FileType.ToString().ToLower()}");
    }
}
