# Shopping List — WPF Application

A WPF desktop application for managing a shopping list. Built with C# and XAML using data binding, `INotifyPropertyChanged`, and a custom observable collection to keep the UI automatically in sync with the underlying data.

---

## Overview

This application lets users build a shopping list, check off items as they find them in the store, and see a live count of remaining items. The UI updates automatically through WPF's data binding system — no manual refresh logic needed.

---

## Features

- Add items to the list via a text input and button
- Check off items with a checkbox as you find them in the store
- Live "Items to Find" counter that updates instantly when items are checked
- Custom observable collection with full `INotifyCollectionChanged` and `INotifyPropertyChanged` support
- Seeded with an initial item (`Eggs`) on startup

---

## File Structure

```
ShoppingList/
├── App.xaml / App.xaml.cs                          # Application entry point
├── MainWindow.xaml / MainWindow.xaml.cs            # Main window, sets up DataContext
├── ShoppingListControl.xaml / .xaml.cs             # UserControl — list UI and add-item logic
├── LineItem.cs                                     # Model — single shopping list entry
├── ShoppingListItems.cs                            # Collection — observable list of LineItems
├── AssemblyInfo.cs                                 # Assembly theme configuration
└── README.md
```

---

## Architecture

The project follows a data binding pattern with a clear separation between the UI and data layers.

### `LineItem` (Model)
Represents a single item on the list.

- Properties: `Name` (string), `Quantity` (uint), `IsFound` (bool)
- Implements `INotifyPropertyChanged` — fires `PropertyChanged` when `IsFound` toggles, so the collection can react immediately

### `ShoppingListItems` (Collection / ViewModel)
A custom collection that wraps `List<LineItem>` and implements:

- `ICollection<LineItem>` — standard add, remove, clear, contains
- `INotifyCollectionChanged` — notifies the `ListView` when items are added or removed
- `INotifyPropertyChanged` — notifies the UI when `Count`, `FoundItemCount`, or `NotFoundItemCount` changes

Subscribes to each `LineItem`'s `PropertyChanged` event on add and unsubscribes on remove, so the aggregate counts stay accurate when individual items are checked off.

| Property | Description |
|----------|-------------|
| `Count` | Total number of items in the list |
| `FoundItemCount` | Number of items marked as found |
| `NotFoundItemCount` | `Count - FoundItemCount` |

### `ShoppingListControl` (View)
A reusable `UserControl` containing the full list UI.

- `ListView` bound to the `DataContext` (`ShoppingListItems`) — renders each `LineItem` as a checkbox + label
- `TextBox` + `Add To List` button — creates a new `LineItem` from the input and adds it to the collection
- `NotFoundItemCount` label bound at the top — updates live as items are checked

### `MainWindow`
Sets `DataContext` to a new `ShoppingListItems` instance and seeds the list with an initial `Eggs` entry.

---

## Data Binding Summary

| Binding | Source | Target |
|---------|--------|--------|
| `{Binding}` | `ShoppingListItems` | `ListView.ItemsSource` |
| `{Binding Path=IsFound}` | `LineItem.IsFound` | `CheckBox.IsChecked` |
| `{Binding Path=Name}` | `LineItem.Name` | `TextBlock.Text` |
| `{Binding Path=NotFoundItemCount}` | `ShoppingListItems` | Items-to-find counter |

---

## Requirements

- .NET 6+ (Windows)
- Visual Studio with WPF workload

## How to Run

Open `ShoppingList.sln` in Visual Studio and press **F5**, or build and run via the CLI:

```bash
dotnet run
```