using Calories.App.ViewModels;
using Syncfusion.Windows.Shared;

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
    }
}