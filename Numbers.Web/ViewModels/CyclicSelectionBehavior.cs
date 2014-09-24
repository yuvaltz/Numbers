using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Numbers.Web.Generic;

namespace Numbers.Web.ViewModels
{
    public class CyclicSelectionBehavior : IDisposable
    {
        public event EventHandler SelectionChanged;

        private int maximumSelectedCount;
        private IEnumerable<SelectableViewModel> selectables;
        private List<SelectableViewModel> selectionOrder;

        public CyclicSelectionBehavior(IEnumerable<SelectableViewModel> selectables, int maximumSelectedCount)
        {
            if (selectables.Any(selectable => selectable.IsSelected))
            {
                throw new Exception("Selectable items must be unselected on cyclic selection behavior creation");
            }

            this.selectables = selectables;
            this.maximumSelectedCount = maximumSelectedCount;

            selectionOrder = new List<SelectableViewModel>();

            if (selectables is INotifyCollectionChanged)
            {
                (selectables as INotifyCollectionChanged).CollectionChanged += OnCollectionChanged;
            }

            foreach (SelectableViewModel selectable in selectables)
            {
                selectable.IsSelectedChanged += OnIsSelectedChanged;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectableViewModel selectable = e.Item as SelectableViewModel;

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                RemoveSelection(selectable);
                selectable.IsSelectedChanged -= OnIsSelectedChanged;
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (selectable.IsSelected)
                {
                    AddSelection(selectable);
                }

                selectable.IsSelectedChanged += OnIsSelectedChanged;
            }
            else
            {
                throw new Exception("Unsupported collection action");
            }
        }

        private void AddSelection(SelectableViewModel selectable)
        {
            selectionOrder.Add(selectable);

            if (selectionOrder.Count > maximumSelectedCount)
            {
                selectionOrder[0].IsSelected = false;
            }
        }

        private void RemoveSelection(SelectableViewModel selectable)
        {
            selectionOrder.Remove(selectable);
        }

        private void OnIsSelectedChanged(object sender, EventArgs e)
        {
            SelectableViewModel selectable = sender as SelectableViewModel;

            if (selectable.IsSelected)
            {
                AddSelection(selectable);
            }
            else
            {
                RemoveSelection(selectable);
            }

            RaiseSelectionChanged();
        }

        public void Dispose()
        {
            if (selectables is INotifyCollectionChanged)
            {
                (selectables as INotifyCollectionChanged).CollectionChanged -= OnCollectionChanged;
            }

            foreach (SelectableViewModel selectable in selectables)
            {
                selectable.IsSelectedChanged -= OnIsSelectedChanged;
            }
        }

        private void RaiseSelectionChanged()
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, EventArgs.Empty);
            }
        }
    }
}
