using System.Data;
using System.Text.RegularExpressions;
using TSFiler.BusinessLogic.Services.Interfaces;

namespace TSFiler.BusinessLogic.Services.DataProcessors;

public class RegexDataProcessor : IDataProcessor
{
    public string ProcessData(string data)
    {
        return EvaluateExpressions(data);
    }

    private string EvaluateExpressions(string input)
    {
        var output = Regex.Replace(input, @"\d+(\s*[\+\-\*\/]\s*\d+)+", match =>
        {
            try
            {
                var expression = match.Value;
                var result = new DataTable().Compute(expression, null);
                return result.ToString();
            }
            catch
            {
                return match.Value;
            }
        });

        return output;
    }
}
