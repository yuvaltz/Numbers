using System;
namespace Numbers.Web.Transitions
{
    public interface IValueBounds
    {
        string FormattedStartValue { get; }
        string FormattedEndValue { get; }
        double GetProgress(string formattedValue);
    }

    public static class ValueBoundsExtensions
    {
        private class ReversedValueBounds : IValueBounds
        {
            private IValueBounds source;

            public ReversedValueBounds(IValueBounds source)
	        {
                this.source = source;
	        }

            public string FormattedStartValue { get { return source.FormattedEndValue; } }
            public string FormattedEndValue { get { return source.FormattedStartValue; } }

            public double GetProgress(string formattedValue)
            {
                return 1 - source.GetProgress(formattedValue);
            }
        }

        public static IValueBounds Reverse(this IValueBounds valueBounds)
        {
            return new ReversedValueBounds(valueBounds);
        }
    }

    public class ValueBounds : IValueBounds
    {
        public string FormattedStartValue { get; private set; }
        public string FormattedEndValue { get; private set; }

        private Func<ValueBounds, string, double> getProgress;

        public ValueBounds(string startValue, string endValue, Func<ValueBounds, string, double> getProgress = null)
        {
            this.FormattedStartValue = startValue;
            this.FormattedEndValue = endValue;

            this.getProgress = getProgress;
        }

        public double GetProgress(string formattedValue)
        {
            return getProgress == null ? 0 : getProgress(this, formattedValue);
        }
    }
}
