using System;

namespace Numbers.Web.ViewModels
{
    public class SelectableViewModel
    {
        public event EventHandler IsSelectedChanged;
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    RaiseIsSelectedChanged();
                }
            }
        }

        private void RaiseIsSelectedChanged()
        {
            if (IsSelectedChanged != null)
            {
                IsSelectedChanged(this, EventArgs.Empty);
            }
        }
    }
}
