using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Interfaces;

public interface IDataProcessor
{
    bool SupportsProcessType(ProcessType processType);
    string ProcessData(string input);
}
