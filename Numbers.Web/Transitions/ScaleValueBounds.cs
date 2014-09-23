using System;
using System.Text.RegularExpressions;

namespace Numbers.Web.Transitions
{
    public class ScaleValueBounds : IValueBounds
    {
        private static readonly Regex scaleRegex = new Regex("scale\\((.*)\\)");
        private static readonly Regex matrixRegex = new Regex("matrix\\( *(.*), *0, *0, *(.*),.*,.*\\)");

        public string FormattedStartValue { get; private set; }
        public string FormattedEndValue { get; private set; }

        private double startValue;
        private double endValue;

        public ScaleValueBounds(double startValue, double endValue)
        {
            this.startValue = startValue;
            this.endValue = endValue;

            this.FormattedStartValue = FormatValue(startValue);
            this.FormattedEndValue = FormatValue(endValue);
        }

        public double GetProgress(string formattedValue)
        {
            double value = GetValue(formattedValue);
            return (value - startValue) / (endValue - startValue);
        }

        private static string FormatValue(double value)
        {
            return String.Format("scale({0})", value);
        }

        private static double GetValue(string formattedValue)
        {
            RegexMatch match = scaleRegex.Exec(formattedValue);

            if (match != null)
            {
                formattedValue = match[1];
            }

            match = matrixRegex.Exec(formattedValue);

            if (match != null && match[1] == match[2])
            {
                return Double.Parse(match[1]);
            }

            return Double.Parse(formattedValue);
        }
    }
}
