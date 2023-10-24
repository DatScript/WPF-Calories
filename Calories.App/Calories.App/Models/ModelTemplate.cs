using CommunityToolkit.Mvvm.ComponentModel;

namespace Calories.App.Models;
public partial class ModelTemplate : ObservableObject
{
    public int Index { get; set; }

    [ObservableProperty]
    private string _volumeIcon = "Solid_VolumeOff";
}