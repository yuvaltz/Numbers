using System;

namespace Numbers.Web.ViewModels
{
    public class OperatorViewModel : SelectableViewModel
    {
        public string Header { get; private set; }

        private Func<Number, Number, Number> calculation;

        public OperatorViewModel(string header, Func<Number, Number, Number> calculation)
        {
            this.Header = header;
            this.calculation = calculation;
        }

        public Number Calculate(NumberViewModel a, NumberViewModel b)
        {
            return calculation(a.Model, b.Model);
        }
    }
}
