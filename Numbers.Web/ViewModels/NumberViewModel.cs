using System;
namespace Numbers.Web.ViewModels
{
    public enum CreationSource { Initial, Result, Undo };

    public class NumberViewModel : SelectableViewModel
    {
        public Number Model { get; private set; }

        public int Value { get { return Model.Value; } }
        public int Level { get { return Model.Level; }  }

        public bool IsTarget { get; private set; }

        public CreationSource Source { get; private set; }

        public NumberViewModel(Number model, bool isTarget = false, CreationSource source = CreationSource.Initial)
        {
            this.Model = model;
            this.IsTarget = isTarget;
            this.Source = source;
        }
    }
}
