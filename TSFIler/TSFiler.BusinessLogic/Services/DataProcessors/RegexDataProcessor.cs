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
        string pattern = @"\d+|[+\-*/()]";
        var tokens = Regex.Matches(input, pattern).Select(m => m.Value).ToArray();
        var result = EvaluateExpression(tokens);
        return result.ToString();
    }

    private int EvaluateExpression(string[] tokens)
    {
        var values = new Stack<int>();
        var operators = new Stack<string>();

        int i = 0;
        while (i < tokens.Length)
        {
            string token = tokens[i];

            if (int.TryParse(token, out int num))
            {
                values.Push(num);
            }
            else if (token == "(")
            {
                operators.Push(token);
            }
            else if (token == ")")
            {
                while (operators.Peek() != "(")
                {
                    values.Push(ApplyOperation(operators.Pop(), values.Pop(), values.Pop()));
                }
                operators.Pop();
            }
            else if (IsOperator(token))
            {
                while (operators.Count > 0 && HasPrecedence(token, operators.Peek()))
                {
                    values.Push(ApplyOperation(operators.Pop(), values.Pop(), values.Pop()));
                }
                operators.Push(token);
            }

            i++;
        }

        while (operators.Count > 0)
        {
            values.Push(ApplyOperation(operators.Pop(), values.Pop(), values.Pop()));
        }

        var value = values.Pop();
        return value;
    }

    private int ApplyOperation(string operation, int b, int a)
    {
        return operation switch
        {
            "+" => a + b,
            "-" => a - b,
            "*" => a * b,
            "/" => b != 0 ? a / b : throw new DivideByZeroException(),
            _ => 0,
        };
    }

    private bool IsOperator(string token)
    {
        var isOperator = token == "+" || token == "-" || token == "*" || token == "/";
        return isOperator;
    }

    private bool HasPrecedence(string op1, string op2)
    {
        if (op2 == "(" || op2 == ")")
        {
            return false;
        }
        if ((op1 == "*" || op1 == "/") && (op2 == "+" || op2 == "-"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
