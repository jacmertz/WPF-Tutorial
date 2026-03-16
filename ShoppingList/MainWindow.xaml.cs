using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace ShoppingList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class and sets up the initial data context for the shopping
        /// list UI.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ShoppingListItems();
            
            if (DataContext is ICollection<LineItem> list)
            {
                list.Add(new LineItem() { Name = "Eggs" });
            }
        }
    }
}
