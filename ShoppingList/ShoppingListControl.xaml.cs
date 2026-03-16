using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ShoppingList
{
    /// <summary>
    /// Interaction logic for ShoppingListControl.xaml
    /// </summary>
    public partial class ShoppingListControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the ShoppingListControl class.
        /// </summary>
        public ShoppingListControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the button click event adding a new item to our list
        /// </summary>
        /// <param name="sender">sender of this event</param>
        /// <param name="e">Metadata for the event</param>
        public void AddItemClick(object sender, RoutedEventArgs e)
        {
            var item = new LineItem()
            {
                Name = Input.Text
            };

            if(DataContext is ICollection<LineItem> list)
            {
                list.Add(item);
            }
        }
    }
}
