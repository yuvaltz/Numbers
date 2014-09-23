using System;

namespace Numbers.Web.Transitions
{
    public class DoubleValueBounds : IValueBounds
    {
        public string FormattedStartValue { get; private set; }
        public string FormattedEndValue { get; private set; }

        private double startValue;
        private double endValue;

        public DoubleValueBounds(double startValue, double endValue)
        {
            this.startValue = startValue;
            this.endValue = endValue;

            this.FormattedStartValue = startValue.ToString();
            this.FormattedEndValue = endValue.ToString();
        }

        public double GetProgress(string formattedValue)
        {
            double value = Double.Parse(formattedValue);
            return (value - startValue) / (endValue - startValue);
        }
    }
}
