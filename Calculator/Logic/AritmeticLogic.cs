

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculator
{
    public static class ArithmeticLogic
    {
        private static int numericBase = 10; 

        public static void SetBase(int baseValue)
        {
            numericBase = baseValue;
        }

        public static string CalculateExpression(string expression)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(expression)) return "Error";

                string convertedExpression = ConvertExpressionToDecimal(expression);

                double result = EvaluateExpression(convertedExpression);

                return ConvertFromDecimal(result, numericBase);
            }
            catch
            {
                return "Error";
            }
        }

        public static string Square(string value)
        {
            try
            {
                double num = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                return ConvertFromDecimal(num * num, numericBase);
            }
            catch
            {
                return "Error";
            }
        }

        public static string SquareRoot(string value)
        {
            try
            {
                double num = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                if (num < 0) return "Error";
                return ConvertFromDecimal(Math.Sqrt(num), numericBase);
            }
            catch
            {
                return "Error";
            }
        }

        public static string Inversion(string value)
        {
            try
            {
                double num = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                if (num == 0) return "Error";
                return ConvertFromDecimal(1 / num, numericBase);
            }
            catch
            {
                return "Error";
            }
        }

        private static string ConvertExpressionToDecimal(string expression)
        {
            expression = expression.Replace("×", "*").Replace("÷", "/");

            string convertedExpression = "";
            string currentNumber = "";
            bool isNegative = false;

            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];

                if (c == '-' && (i == 0 || "+-*/()".Contains(expression[i - 1])))
                {
                    isNegative = true;
                }
                else if (char.IsDigit(c) || (numericBase == 16 && "ABCDEF".Contains(char.ToUpper(c))))
                {
                    currentNumber += c;
                }
                else
                {
                    if (!string.IsNullOrEmpty(currentNumber))
                    {
                        double number = ConvertToDecimal(currentNumber, numericBase);
                        convertedExpression += isNegative ? (-number).ToString() : number.ToString();
                        currentNumber = "";
                        isNegative = false;
                    }
                    convertedExpression += c;
                }
            }

            if (!string.IsNullOrEmpty(currentNumber))
            {
                double number = ConvertToDecimal(currentNumber, numericBase);
                convertedExpression += isNegative ? (-number).ToString() : number.ToString();
            }

            return convertedExpression;
        }

        private static double ConvertToDecimal(string value, int baseValue)
        {
            if (double.TryParse(value, out double result))
                return result;

            return Convert.ToInt64(value, baseValue);
        }

        private static double EvaluateExpression(string expression)
        {
            var tokens = Regex.Split(expression, @"([\+\-\*/%\(\)])").Where(t => t.Trim() != "").ToList();
            var values = new Stack<double>();
            var operators = new Stack<char>();

            Dictionary<char, int> precedence = new Dictionary<char, int>
    {
        { '+', 1 }, { '-', 1 }, { '*', 2 }, { '/', 2 }, { '%', 2 }
    };

            bool expectNumber = true;

            foreach (var token in tokens)
            {
                if (double.TryParse(token, out double num))
                {
                    values.Push(num);
                    expectNumber = false;
                }
                else if (token == "-" && expectNumber)
                {
                    values.Push(0);
                    operators.Push('-');
                }
                else if (precedence.ContainsKey(token[0]))
                {
                    while (operators.Count > 0 && precedence[operators.Peek()] >= precedence[token[0]])
                        ApplyOperator(values, operators.Pop());

                    operators.Push(token[0]);
                    expectNumber = true;
                }
                else if (token == "(")
                {
                    operators.Push('(');
                    expectNumber = true;
                }
                else if (token == ")")
                {
                    while (operators.Peek() != '(')
                        ApplyOperator(values, operators.Pop());

                    operators.Pop();
                    expectNumber = false;
                }
            }

            while (operators.Count > 0)
                ApplyOperator(values, operators.Pop());

            return values.Pop();
        }

        private static void ApplyOperator(Stack<double> values, char op)
        {
            if (values.Count < 2) return;

            double b = values.Pop();
            double a = values.Pop();

            switch (op)
            {
                case '+': values.Push(a + b); break;
                case '-': values.Push(a - b); break;
                case '*': values.Push(a * b); break;
                case '/':
                    if (b == 0) throw new DivideByZeroException();
                    values.Push(a / b);
                    break;
                case '%':
                    if (b == 0) throw new DivideByZeroException();
                    values.Push(a % b);
                    break;
            }
        }

        private static string ConvertFromDecimal(double value, int baseValue)
        {
            if (double.IsInfinity(value) || double.IsNaN(value)) return "Error";

            if (baseValue == 10) return value.ToString();
            if (baseValue == 2) return Convert.ToString((long)value, 2);
            if (baseValue == 8) return Convert.ToString((long)value, 8);
            if (baseValue == 16) return Convert.ToString((long)value, 16).ToUpper();

            return value.ToString();
        }
    }
}
