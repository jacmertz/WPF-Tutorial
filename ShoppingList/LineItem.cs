using System.ComponentModel;

namespace ShoppingList
{
    /// <summary>
    /// A class representing an item on a shopping list, 
    /// with a quantity and a flag to indicate if it has
    /// been found yet while shopping at the store.
    /// </summary>
    public class LineItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// The name of the item needed
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The quantity of the item needed
        /// </summary>
        public uint Quantity { get; set; }

        /// <summary>
        /// Contains whether the item has been found or not
        /// </summary>
        private bool _isFound = false;

        /// <summary>
        /// If the item has been found
        /// </summary>
        public bool IsFound
        {
            get => _isFound;
            set
            {
                _isFound = value;

                //IsFound just changed! Invoke event handler

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFound)));
            }
        }

        /// <summary>
        /// returns the name of the item on the list
        /// </summary>
        /// <returns>The item's name</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
