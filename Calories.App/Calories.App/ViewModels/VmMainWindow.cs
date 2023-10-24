using Calories.App.Helper;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace Calories.App.ViewModels;
public partial class VmMainWindow : ObservableObject
{
    [ObservableProperty]
    private string _segoeUI = "SegoeUI";

    public VmMainWindow()
    {
        var a = ConfigHelper.Instance.Config.ProgramDataPath;
    }

    private void Close()
    {
        foreach (Window item in Application.Current.Windows)
        {
            if (item.DataContext == this) item.Close();
        }
    }
}