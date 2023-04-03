
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TabletopSystems.Helper_Methods
{
    public static class MathParser
    {
        public static double MathExpressionParser(string expression)
        {
            Stack<double> numbers = new Stack<double>();
            Stack<char> operators = new Stack<char>();
            expression = Regex.Replace(expression, @"\s+", "");
            double number;
            char nextStackOp;
            string numberToAdd = "";
            for (int i = 0; i < expression.Length; i++)
            {
                if (double.TryParse(expression[i].ToString(), out number) || expression[i] == '.')
                {
                    numberToAdd = numberToAdd + expression[i];
                    continue;
                }
                else if (IsOperator(expression[i]))
                {
                    numbers.Push(double.Parse(numberToAdd));
                    numberToAdd = "";
                    while (operators.TryPeek(out nextStackOp) && StackHigherPrecedence(nextStackOp, expression[i]))
                    {
                        double rightNumber = numbers.Pop();
                        double leftNumber = numbers.Pop();
                        operators.Pop();
                        switch (nextStackOp)
                        {
                            case ('+'):
                                leftNumber += rightNumber;
                                break;
                            case ('-'):
                                leftNumber -= rightNumber;
                                break;
                            case ('*'):
                                leftNumber *= rightNumber;
                                break;
                            case ('/'):
                                leftNumber /= rightNumber;
                                break;
                            case ('^'):
                                //NOTE: Does not work with using powers and decimals
                                if (rightNumber == 0)
                                {
                                    leftNumber = 0;
                                    break;
                                }
                                double originalLeftNum = leftNumber;
                                for (int j = 0; j < rightNumber - 1; j++)
                                {
                                    leftNumber *= originalLeftNum;
                                }
                                break;
                            default:
                                break;
                        }
                        numbers.Push(leftNumber);
                    }
                    operators.Push(expression[i]);
                }
                else if (expression[i] == '(')
                {
                    numbers.Push(double.Parse(numberToAdd));
                    numberToAdd = "";
                    operators.Push(expression[i]);
                }
                else if (expression[i] == ')')
                {
                    numbers.Push(double.Parse(numberToAdd));
                    numberToAdd = "";
                    while (operators.TryPeek(out nextStackOp))
                    {
                        double rightNumber = numbers.Pop();
                        double leftNumber = numbers.Pop();
                        operators.Pop();
                        switch (nextStackOp)
                        {
                            case ('+'):
                                leftNumber += rightNumber;
                                break;
                            case ('-'):
                                leftNumber -= rightNumber;
                                break;
                            case ('*'):
                                leftNumber *= rightNumber;
                                break;
                            case ('/'):
                                leftNumber /= rightNumber;
                                break;
                            case ('^'):
                                //NOTE: Does not work with using powers and decimals
                                if (rightNumber == 0)
                                {
                                    leftNumber = 0;
                                    break;
                                }
                                double originalLeftNum = leftNumber;
                                for (int j = 0; j < rightNumber - 1; j++)
                                {
                                    leftNumber *= originalLeftNum;
                                }
                                break;
                            default:
                                break;
                        }
                        numbers.Push(leftNumber);

                    }
                    operators.Pop();
                }
            }
            numbers.Push(double.Parse(numberToAdd));
            numberToAdd = "";
            while (operators.Count > 0)
            {
                while (operators.TryPeek(out nextStackOp))
                {
                    double rightNumber = numbers.Pop();
                    double leftNumber = numbers.Pop();
                    operators.Pop();
                    switch (nextStackOp)
                    {
                        case ('+'):
                            leftNumber += rightNumber;
                            break;
                        case ('-'):
                            leftNumber -= rightNumber;
                            break;
                        case ('*'):
                            leftNumber *= rightNumber;
                            break;
                        case ('/'):
                            leftNumber /= rightNumber;
                            break;
                        case ('^'):
                            //NOTE: Does not work with using powers and decimals
                            if (rightNumber == 0)
                            {
                                leftNumber = 0;
                                break;
                            }
                            double originalLeftNum = leftNumber;
                            for (int j = 0; j < rightNumber - 1; j++)
                            {
                                leftNumber *= originalLeftNum;
                            }
                            break;
                        default:
                            break;
                    }
                    numbers.Push(leftNumber);

                }
            }
            return numbers.Pop();
        }

        public static bool IsOperator(char c)
        {
            if (c == '-' || c == '+' || c == '*' || c == '/' || c == '^')
            {
                return true;
            }
            return false;
        }

        public static bool StackHigherPrecedence(char opOnStack, char opBeingRead)
        {
            if (opOnStack == opBeingRead) return true;
            switch (opOnStack)
            {
                case ('+'):
                    if (opBeingRead == '+' || opBeingRead == '-')
                    {
                        return true;
                    }
                    return false;
                case ('-'):
                    if (opBeingRead == '+' || opBeingRead == '-')
                    {
                        return true;
                    }
                    return false;
                case ('*'):
                    if (opBeingRead == '+' || opBeingRead == '-' || opBeingRead == '*' || opBeingRead == '/')
                    {
                        return true;
                    }
                    return true;
                case ('/'):
                    if (opBeingRead == '+' || opBeingRead == '-' || opBeingRead == '*' || opBeingRead == '/')
                    {
                        return true;
                    }
                    return true;
                case ('^'):
                    return true;
                default: return false;
            }

        }
    }
}
