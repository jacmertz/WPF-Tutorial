using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;


namespace ShoppingList
{
    /// <summary>
    /// Represents a collection of shopping list line items that supports change notification and provides aggregate
    /// information about found and not found items.
    /// </summary>
    public class ShoppingListItems : ICollection<LineItem>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        /// <summary>
        /// Contains the list of line items in the shopping list.
        /// </summary>
        private List<LineItem> _items = new List<LineItem>();

        /// <summary>
        /// Occurs when the collection changes, such as when items are added, removed, or the entire list is refreshed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event to notify listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. This value is passed to event subscribers in the
        /// PropertyChangedEventArgs.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Handles the PropertyChanged event for an item and raises PropertyChanged notifications for related
        /// properties when the item's state changes.
        /// </summary>
        /// <param name="sender">The source of the PropertyChanged event, typically the item whose property has changed.</param>
        /// <param name="e">An object that contains the event data, including the name of the property that changed.</param>
        private void HandleItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsFound")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotFoundItemCount)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FoundItemCount)));
            }
        }

        /// <summary>
        /// Gets the number of items that have been marked as found.
        /// </summary>
        public int FoundItemCount
        {
            get
            {
                int count = 0;
                foreach (LineItem item in _items)
                {
                    if (item.IsFound) count++;
                }

                return count;
            }
        }

        /// <summary>
        /// Gets the number of items that were not found during the operation.
        /// </summary>
        public int NotFoundItemCount
        {
            get
            {
                return Count - FoundItemCount;
            }
        }

        #region collectionImplenetation

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly => ((ICollection<LineItem>)_items).IsReadOnly;

        /// <summary>
        /// Adds the specified line item to the collection and subscribes to its property change notifications.
        /// </summary>
        /// <param name="item">The line item to add to the collection. Cannot be null.</param>
        public void Add(LineItem item)
        {
            _items.Add(item);

            item.PropertyChanged += HandleItemPropertyChanged;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FoundItemCount)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotFoundItemCount)));

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        /// <summary>
        /// Removes all items from the collection and resets related counts.
        /// </summary>
        public void Clear()
        {
            foreach (LineItem item in _items)
            {
                item.PropertyChanged -= HandleItemPropertyChanged;
            }
            
            _items.Clear();

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(FoundItemCount));
            OnPropertyChanged(nameof(NotFoundItemCount));

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Determines whether the collection contains the specified line item.
        /// </summary>
        /// <param name="item">The line item to locate in the collection. Can be null if the collection supports null values.</param>
        /// <returns>true if the specified line item is found in the collection; otherwise, false.</returns>
        public bool Contains(LineItem item)
        {
            return _items.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the collection to the specified array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the collection. The array must
        /// have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in the destination array at which copying begins.</param>
        public void CopyTo(LineItem[] array, int arrayIndex)
        {
            ((ICollection<LineItem>)_items).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection of line items.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection of <see cref="LineItem"/> objects.</returns>
        public IEnumerator<LineItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Removes the specified line item from the collection.
        /// </summary>
        /// <param name="item">The line item to remove from the collection. Cannot be null.</param>
        /// <returns>true if the item was successfully removed; otherwise, false.</returns>
        public bool Remove(LineItem item)
        {
            int index = _items.IndexOf(item);
            if (index != -1)
            {
                _items.Remove(item);

                item.PropertyChanged -= HandleItemPropertyChanged;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FoundItemCount)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotFoundItemCount)));

                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion
    }
}
