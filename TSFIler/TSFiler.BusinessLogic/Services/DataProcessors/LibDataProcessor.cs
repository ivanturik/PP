using NCalc;
using TSFiler.BusinessLogic.Services.Interfaces;
using System.Text.RegularExpressions;

namespace TSFiler.BusinessLogic.Services.DataProcessors
{
    public class LibDataProcessor : IDataProcessor
    {
        public string ProcessData(string data)
        {
            return EvaluateExpressions(data);
        }

        private string EvaluateExpressions(string input)
        {

            var regex = new Regex(@"\b\d+(\s*[-+*/]\s*\d+)+\b");

            return regex.Replace(input, match =>
            {
                try
                {
                    var expression = new Expression(match.Value.Trim());

                    if (expression.HasErrors())
                    {
                        return match.Value; 
                    }

                    return expression.Evaluate().ToString();
                }
                catch
                {
                    return match.Value;
                }
            });
        }
    }
}
