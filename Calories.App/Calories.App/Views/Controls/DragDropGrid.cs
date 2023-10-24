using Calories.App.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Calories.App.Views.Controls;
public class DragDropGrid : Control
{
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.RegisterAttached(
        nameof(ItemsSource), typeof(ObservableCollection<Nutrient>), typeof(DragDropGrid), new PropertyMetadata());

    public ObservableCollection<Nutrient> ItemsSource
    {
        get => (ObservableCollection<Nutrient>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    static DragDropGrid()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DragDropGrid), new FrameworkPropertyMetadata(typeof(DragDropGrid)));
    }

    public DragDropGrid()
    {
        AllowDrop = true;
        Drop += OnDrop;
        DragEnter += DragDropGrid_DragEnter;
    }

    private void DragDropGrid_DragEnter(object sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.Copy;
        e.Handled = true;
    }

    private void OnDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(object)))
        {
            var droppedItem = e.Data.GetData(typeof(object));
            //ItemsSource.Add(droppedItem);
        }
    }

}
