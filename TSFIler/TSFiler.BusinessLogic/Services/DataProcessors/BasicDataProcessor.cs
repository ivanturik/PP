using System;
using System.Collections.Generic;
using System.Text;
using TSFiler.BusinessLogic.Services.Interfaces;

namespace TSFiler.BusinessLogic.Services.DataProcessors
{
    public class BasicDataProcessor : IDataProcessor
    {
        public string ProcessData(string data)
        {
            return EvaluateExpressions(data);
        }

        private string EvaluateExpressions(string input)
        {
            List<(int start, int end)> expressions = FindAlgebraicExpressions(input);
            StringBuilder result = new StringBuilder(input);

            for (int i = expressions.Count - 1; i >= 0; i--)
            {
                var (start, end) = expressions[i];
                string expression = input.Substring(start, end - start + 1).Replace(" ", "");
                string evaluatedExpression = CalculateExpression(expression);
                result.Remove(start, end - start + 1).Insert(start, evaluatedExpression);
            }

            return result.ToString();
        }

        private List<(int start, int end)> FindAlgebraicExpressions(string input)
        {
            List<(int start, int end)> expressions = new List<(int start, int end)>();
            int start = -1;
            bool inExpression = false;
            int operandCount = 0;
            bool hasOperator = false;

            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];

                if (char.IsWhiteSpace(ch))
                {
                    continue;
                }

                if (char.IsDigit(ch))
                {
                    if (!inExpression)
                    {
                        start = i;
                        inExpression = true;
                    }
                    operandCount++;
                }
                else if (IsOperator(ch.ToString()) && inExpression)
                {
                    hasOperator = true;
                }
                else if (ch == '(' || ch == ')')
                {
                    if (!inExpression)
                    {
                        start = i;
                        inExpression = true;
                    }
                }
                else
                {
                    if (inExpression)
                    {
                        if (operandCount >= 2 && hasOperator)
                        {
                            expressions.Add((start, i - 1));
                        }
                        inExpression = false;
                        operandCount = 0;
                        hasOperator = false;
                    }
                }
            }

            if (inExpression && operandCount >= 2 && hasOperator)
            {
                expressions.Add((start, input.Length - 1));
            }

            return expressions;
        }



        private string CalculateExpression(string expression)
        {
            List<string> tokens = Tokenize(expression);
            List<string> rpn = ConvertToRPN(tokens);
            return EvaluateRPNPartial(rpn);
        }

        private List<string> Tokenize(string input)
        {
            List<string> tokens = new List<string>();
            StringBuilder currentToken = new StringBuilder();

            foreach (char ch in input)
            {
                if (char.IsWhiteSpace(ch))
                {
                    continue;
                }
                else if (IsOperator(ch.ToString()) || ch == '(' || ch == ')')
                {
                    if (currentToken.Length > 0)
                    {
                        tokens.Add(currentToken.ToString());
                        currentToken.Clear();
                    }
                    tokens.Add(ch.ToString());
                }
                else
                {
                    currentToken.Append(ch);
                }
            }

            if (currentToken.Length > 0)
            {
                tokens.Add(currentToken.ToString());
            }

            return tokens;
        }

        private List<string> ConvertToRPN(List<string> tokens)
        {
            List<string> output = new List<string>();
            Stack<string> operators = new Stack<string>();

            foreach (var token in tokens)
            {
                if (IsNumber(token))
                {
                    output.Add(token);
                }
                else if (IsOperator(token))
                {
                    while (operators.Count > 0 && operators.Peek() != "(" && GetPrecedence(operators.Peek()) >= GetPrecedence(token))
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Push(token);
                }
                else if (token == "(")
                {
                    operators.Push(token);
                }
                else if (token == ")")
                {
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        output.Add(operators.Pop());
                    }
                    if (operators.Count > 0)
                    {
                        operators.Pop();
                    }
                }
            }

            while (operators.Count > 0)
            {
                output.Add(operators.Pop());
            }

            return output;
        }
        private string EvaluateRPNPartial(List<string> rpn)
        {
            Stack<double> stack = new Stack<double>();
            List<string> result = new List<string>();

            for (int i = 0; i < rpn.Count; i++)
            {
                string token = rpn[i];

                if (IsNumber(token))
                {
                    stack.Push(double.Parse(token));
                }
                else if (IsOperator(token))
                {
                    if (stack.Count < 2)
                    {
                        result.Add(token);
                        continue;
                    }

                    double b = stack.Pop();
                    double a = stack.Pop();

                    if (token == "/" && b == 0)
                    {
                        result.Add($"{a}/{b}");
                        continue;
                    }

                    double operationResult = token switch
                    {
                        "+" => a + b,
                        "-" => a - b,
                        "*" => a * b,
                        "/" => a / b,
                        _ => 0
                    };

                    stack.Push(operationResult);
                }
                else
                {
                    result.Add(token);
                }
            }

            if (stack.Count == 1)
            {
                result.Add(stack.Pop().ToString());
            }

            return string.Join(" ", result).Trim();
        }



        private bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/";
        }

        private bool IsNumber(string token)
        {
            return double.TryParse(token, out _);
        }

        private int GetPrecedence(string op)
        {
            return op == "+" || op == "-" ? 1 : 2;
        }
    }
}
