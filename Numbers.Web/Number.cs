using System;
using System.Text;

namespace Numbers.Web
{
    public enum Operator
    {
        Create = 0,
        Add = 1,
        Subtract = 2,
        Multiply = 3,
        Divide = 4,
    }

    public class Number : IComparable<Number>
    {
        public int Value { get; private set; }
        public int Level { get; private set; }

        public Number Operand1 { get; private set; }
        public Number Operand2 { get; private set; }
        public Operator Operator { get; private set; }

        private Number(int value, int level, Number operand1, Number operand2, Operator @operator)
        {
            this.Value = value;
            this.Level = level;
            this.Operand1 = operand1;
            this.Operand2 = operand2;
            this.Operator = @operator;
        }

        public override string ToString()
        {
            return ToString(false, true);
        }

        public string ToString(bool includeValue, bool reduceParentheses)
        {
            if (Operator == Operator.Create)
            {
                return Value.ToString();
            }

            StringBuilder stringBuilder = new StringBuilder();

            if (includeValue)
            {
                stringBuilder.Append(Value);
                stringBuilder.Append("=");
            }

            if (Operand1.Operator == Operator.Create ||
                !includeValue && reduceParentheses && (Operator == Operator.Add || Operator == Operator.Subtract || (Operator == Operator.Multiply || Operator == Operator.Divide) && (Operand1.Operator == Operator.Multiply || Operand1.Operator == Operator.Divide)))
            {
                stringBuilder.Append(Operand1.ToString(includeValue, reduceParentheses));
            }
            else
            {
                stringBuilder.Append(String.Format("({0})", Operand1.ToString(includeValue, reduceParentheses)));
            }

            stringBuilder.Append(GetOperatorString(Operator));

            if (Operand2.Operator == Operator.Create ||
                !includeValue && reduceParentheses && (Operator == Operator.Add || (Operator == Operator.Subtract || Operator == Operator.Multiply || Operator == Operator.Divide) && (Operand2.Operator == Operator.Multiply || Operand2.Operator == Operator.Divide)))
            {
                stringBuilder.Append(Operand2.ToString(includeValue, reduceParentheses));
            }
            else
            {
                stringBuilder.Append(String.Format("({0})", Operand2.ToString(includeValue, reduceParentheses)));
            }

            return stringBuilder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            Number number = obj as Number;

            return this.Level.Equals(number.Level) && this.Value.Equals(number.Value) && this.Operator.Equals(number.Operator) &&
                (ReferenceEquals(this.Operand1, null) && ReferenceEquals(number.Operand1, null) || !ReferenceEquals(this.Operand1, null) && this.Operand1.Equals(number.Operand1)) &&
                (ReferenceEquals(this.Operand2, null) && ReferenceEquals(number.Operand2, null) || !ReferenceEquals(this.Operand2, null) && this.Operand2.Equals(number.Operand2));
        }

        public override int GetHashCode()
        {
            return Level ^ Value ^ (Operand1 != null ? Operand1.GetHashCode() : 0) ^ (Operand2 != null ? Operand2.GetHashCode() : 0);
        }

        public static bool operator ==(Number a, Number b)
        {
            return ReferenceEquals(a, null) && ReferenceEquals(b, null) || !ReferenceEquals(a, null) && a.Equals(b);
        }

        public static bool operator !=(Number a, Number b)
        {
            return !(a == b);
        }

        public static Number Create(int value)
        {
            return new Number(value, 1, null, null, Operator.Create);
        }

        public static Number Add(Number a, Number b)
        {
            return new Number(a.Value + b.Value, a.Level + b.Level, a, b, Operator.Add);
        }

        public static Number Subtract(Number a, Number b)
        {
            if (a.Value < b.Value)
            {
                Number c = a;
                a = b;
                b = c;
            }

            return new Number(a.Value - b.Value, a.Level + b.Level, a, b, Operator.Subtract);
        }

        public static Number Multiply(Number a, Number b)
        {
            return new Number(a.Value * b.Value, a.Level + b.Level, a, b, Operator.Multiply);
        }

        public static Number Divide(Number a, Number b)
        {
            if (a.Value < b.Value)
            {
                Number c = a;
                a = b;
                b = c;
            }

            if (b.Value == 0 || a.Value % b.Value != 0)
            {
                return null;
            }

            return new Number(a.Value > b.Value ? a.Value / b.Value : b.Value / a.Value, a.Level + b.Level, a, b, Operator.Divide);
        }

        private static string GetOperatorString(Operator value)
        {
            switch (value)
            {
                case Operator.Create: return "Create";
                case Operator.Add: return "+";
                case Operator.Subtract: return "-";
                case Operator.Multiply: return "*";
                case Operator.Divide: return "/";
                default: throw new Exception("Unrecognized Operator");
            }
        }

        public int CompareTo(Number other)
        {
            int result = Value.CompareTo(other.Value);

            if (result != 0 || Operator == Operator.Create && other.Operator == Operator.Create)
            {
                return result;
            }

            if (Operator == Operator.Create)
            {
                return -1;
            }

            if (other.Operator == Operator.Create)
            {
                return 1;
            }

            result = Operand1.CompareTo(other.Operand1);

            if (result != 0)
            {
                return result;
            }

            return Operand2.CompareTo(other.Operand2);
        }
    }
}
