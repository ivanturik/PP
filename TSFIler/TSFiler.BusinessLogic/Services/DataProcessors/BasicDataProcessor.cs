using TSFiler.BusinessLogic.Interfaces;
using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Services.DataProcessors;

public class BasicDataProcessor : IDataProcessor
{
    public bool SupportsProcessType(ProcessType processType)
    {
        return processType == ProcessType.Default;
    }

    public string ProcessData(string input)
    {
        var tokens = input.Split(' ');
        var result = new List<string>();

        for (int i = 0; i < tokens.Length; i++)
        {
            if (int.TryParse(tokens[i], out int num1) &&
                i + 2 < tokens.Length &&
                int.TryParse(tokens[i + 2], out int num2))
            {
                string operation = tokens[i + 1];
                int operationResult = operation switch
                {
                    "+" => num1 + num2,
                    "-" => num1 - num2,
                    "*" => num1 * num2,
                    "/" when num2 != 0 => num1 / num2,
                    _ => 0
                };

                result.Add(operationResult.ToString());
                i += 2;
            }
            else
            {
                result.Add(tokens[i]);
            }
        }

        return string.Join(' ', result);
    }
}
