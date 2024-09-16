using System.Text.RegularExpressions;
using TSFiler.BusinessLogic.Interfaces;
using TSFiler.Common.Enums;

namespace TSFiler.BusinessLogic.Services.DataProcessors;

public class RegexDataProcessor : IDataProcessor
{
    public bool SupportsProcessType(ProcessType processType)
    {
        return processType == ProcessType.Regex;
    }

    public string ProcessData(string input)
    {
        string pattern = @"(\d+)([+\-*/])(\d+)";
        return Regex.Replace(input, pattern, match =>
        {
            int num1 = int.Parse(match.Groups[1].Value);
            string operation = match.Groups[2].Value;
            int num2 = int.Parse(match.Groups[3].Value);

            int result = operation switch
            {
                "+" => num1 + num2,
                "-" => num1 - num2,
                "*" => num1 * num2,
                "/" when num2 != 0 => num1 / num2,
                _ => 0
            };

            return result.ToString();
        });
    }
}
