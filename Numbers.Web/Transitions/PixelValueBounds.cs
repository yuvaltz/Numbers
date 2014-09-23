using System;

namespace Numbers.Web.Transitions
{
    public class PixelValueBounds : IValueBounds
    {
        public string FormattedStartValue { get; private set; }
        public string FormattedEndValue { get; private set; }

        private double startValue;
        private double endValue;

        public PixelValueBounds(double startValue, double endValue)
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
            return String.Format("{0}px", value);
        }

        private static double GetValue(string formattedValue)
        {
            if (formattedValue.EndsWith("px"))
            {
                formattedValue = formattedValue.Substring(0, formattedValue.Length - 2);
            }

            return Double.Parse(formattedValue);
        }
    }
}
