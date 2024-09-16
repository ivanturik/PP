using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Models;

public class FileInfoModel
{
    public string OutputFileName { get; set; }
    public FileType FileType { get; set; }
    public ProcessType ProcessType { get; set; }
}
