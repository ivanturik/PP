using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSFiler.BusinessLogic.Services.Interfaces;

namespace TSFiler.BusinessLogic.Services.DataProcessors;

    public class BasicDataProcessor : IDataProcessor
    {
        public string ProcessData(string data)
        {
            return EvaluateExpressions(data);
        }

        private string EvaluateExpressions(string input)
        {
            StringBuilder result = new StringBuilder();
            int index = 0;

            while (index < input.Length)
            {
                if (char.IsDigit(input[index]) || input[index] == '(')
                {
                    var (expression, end) = ExtractExpression(input, index);
                    string evaluatedExpression = CalculateExpression(expression);
                    result.Append(evaluatedExpression);
                    index = end + 1;
                }
                else
                {
                    result.Append(input[index]);
                    index++;
                }
            }

            return result.ToString();
        }

        private (string, int) ExtractExpression(string input, int start)
        {
            StringBuilder expression = new StringBuilder();
            int i = start;
            int openBrackets = 0;

            while (i < input.Length)
            {
                char ch = input[i];

                if (char.IsDigit(ch) || ch == '.' || (ch == '-' && (i == start || input[i - 1] == '(')))
                {
                    expression.Append(ch);
                }
                else if (IsOperator(ch.ToString()) && expression.Length > 0)
                {
                    expression.Append(ch);
                }
                else if (ch == '(')
                {
                    openBrackets++;
                    expression.Append(ch);
                }
                else if (ch == ')')
                {
                    openBrackets--;
                    expression.Append(ch);
                    if (openBrackets == 0)
                    {
                        break;
                    }
                }
                else if (openBrackets == 0)
                {
                    break;
                }

                i++;
            }

            return (expression.ToString(), i);
        }

        private string CalculateExpression(string expression)
        {
            try
            {
                var result = new DataTable().Compute(expression, null);
                return result.ToString();
            }
            catch
            {
                return expression;
            }
        }

        private bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/";
        }
    }

