using Calories.App.Models;
using Calories.App.ViewModels;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.Windows.Shared;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Calories.App.Views;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : ChromelessWindow
{
    public VmMainWindow ViewModel { get; }

    public MainWindow(VmMainWindow viewModel)
    {
        ViewModel = viewModel;
        this.DataContext = this;

        InitializeComponent();

        GridBreakfast.RowDragDropController.Drop += RowDragDropController_Drop;
        GridBreakfast.CellDoubleTapped += GridOnCellDoubleTapped;
        GridBreakfast.Drop += GridBreakfast_Drop;

        GridLunch.RowDragDropController.Drop += RowDragDropController_Drop;
        GridLunch.CellDoubleTapped += GridOnCellDoubleTapped;
        GridLunch.Drop += GridBreakfast_Drop;

        GridDinner.RowDragDropController.Drop += RowDragDropController_Drop;
        GridDinner.CellDoubleTapped += GridOnCellDoubleTapped;
        GridDinner.Drop += GridBreakfast_Drop;

        GridDay.CellDoubleTapped += GridDayOnCellDoubleTapped;

        ListBoxNutrients.PreviewMouseLeftButtonDown += ListBox_PreviewMouseLeftButtonDown;
        ListBoxNutrients.PreviewMouseMove += ListBox_PreviewMouseMove;

        //GridNutrients.RowDragDropController.Dropped += RowDragDropController_Dropped;
        //GridNutrients.RowDragDropController.Drop += RowDragDropController_Drop;
        //GridNutrients.RowDragDropController.DragLeave += RowDragDropController_DragLeave;
    }

    private void GridDayOnCellDoubleTapped(object? sender, GridCellDoubleTappedEventArgs e)
    {
        if (e.Record is not DayDetail dayDetail) return;
        dayDetail.Clear();
    }

    private void GridOnCellDoubleTapped(object? sender, GridCellDoubleTappedEventArgs e)
    {
        if (e.Record is not Models.Nutrient nutrient) return;
        var itemsSource = (sender as SfDataGrid)?.ItemsSource as ObservableCollection<Models.Nutrient>;
        if (itemsSource == null) return;
        itemsSource.Remove(nutrient);
        ViewModel.DayDetail?.Calculate();
    }

    private void GridBreakfast_Drop(object sender, System.Windows.DragEventArgs e)
    {
        // Loop all data

        if (e.Data.GetData("NutrientItem") is not Models.Nutrient nutrient)
            return;
        ((sender as SfDataGrid)?.ItemsSource as ObservableCollection<Models.Nutrient>)?.Add(nutrient);
        ViewModel.DayDetail?.Calculate();
    }

    private void RowDragDropController_Drop(object? sender, GridRowDropEventArgs e)
    {
        e.Handled = true;
    }

    private bool isDrag = false;

    private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        ListBox parent = (ListBox)sender;
        var nutrientItem = FindAncestor<ListBoxItem>(e.OriginalSource as DependencyObject);
        if (nutrientItem == null) return;
        var data = new DataObject("NutrientItem", nutrientItem.DataContext);
        DragDrop.DoDragDrop(nutrientItem, data, DragDropEffects.Move);
    }

    private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
    {
    }

    private static object GetDataFromListBox(ListBox source, Point point)
    {
        if (source.InputHitTest(point) is UIElement element)
        {
            object data = DependencyProperty.UnsetValue;
            while (data == DependencyProperty.UnsetValue)
            {
                data = source.ItemContainerGenerator.ItemFromContainer(element);

                if (data == DependencyProperty.UnsetValue)
                {
                    element = VisualTreeHelper.GetParent(element) as UIElement;
                }

                if (element is Border border)
                {
                    if (border.Name == "BorderNutrient")
                        return border.DataContext;
                }

                //if (element == source)
                //{
                //    return null;
                //}
            }

            if (data != DependencyProperty.UnsetValue)
            {
                return data;
            }
        }

        return null;
    }

    private static T? FindAncestor<T>(DependencyObject? current)
        where T : DependencyObject
    {
        do
        {
            if (current is T)
            {
                return (T)current;
            }
            current = VisualTreeHelper.GetParent(current);
        }
        while (current != null);
        return null;
    }
}