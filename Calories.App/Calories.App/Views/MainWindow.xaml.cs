using Calories.App.ViewModels;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.Windows.Shared;
using System.Collections.ObjectModel;
using System.Linq;

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

        //GridNutrients.RowDragDropController.Dropped += RowDragDropController_Dropped;
        //GridNutrients.RowDragDropController.Drop += RowDragDropController_Drop;
        //GridNutrients.RowDragDropController.DragLeave += RowDragDropController_DragLeave;
    }

    private void GridOnCellDoubleTapped(object? sender, GridCellDoubleTappedEventArgs e)
    {
        if (e.Record is not Models.Nutrient nutrient) return;
        var itemsSource = (sender as SfDataGrid)?.ItemsSource as ObservableCollection<Models.Nutrient>;
        if (itemsSource == null) return;
        itemsSource.Remove(nutrient);
        ViewModel.Calculate();
    }

    private void GridBreakfast_Drop(object sender, System.Windows.DragEventArgs e)
    {
        if (e.Data.GetData("Records") is not ObservableCollection<object> records) return;
        if (records.FirstOrDefault() is not Models.Nutrient first) return;
        ((sender as SfDataGrid)?.ItemsSource as ObservableCollection<Models.Nutrient>)?.Add(first);
        ViewModel.Calculate();
    }

    private void RowDragDropController_Drop(object? sender, GridRowDropEventArgs e)
    {
        e.Handled = true;
    }
}