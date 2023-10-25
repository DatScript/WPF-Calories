using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Calories.App.Models;
public partial class DayDetail : ObservableObject
{
    public string Name { get; set; }

    [ObservableProperty]
    private float _protein;

    [ObservableProperty]
    private float _carbohydrate;

    [ObservableProperty]
    private float _fat;


    [ObservableProperty]
    private ObservableCollection<Nutrient> _breakfast = new();

    [ObservableProperty]
    private ObservableCollection<Nutrient> _lunch = new();

    [ObservableProperty]
    private ObservableCollection<Nutrient> _dinner = new();

    public DayDetail(string name)
    {
        Name = name;
    }

    public void Clear()
    {
        Breakfast.Clear();
        Lunch.Clear();
        Dinner.Clear();
        Calculate();
    }

    public void Calculate()
    {
        Protein = 0;
        Carbohydrate = 0;
        Fat = 0;

        foreach (var nutrient in Breakfast)
        {
            Protein += nutrient.Protein;
            Carbohydrate += nutrient.Carbohydrate;
            Fat += nutrient.Fat;
        }

        foreach (var nutrient in Lunch)
        {
            Protein += nutrient.Protein;
            Carbohydrate += nutrient.Carbohydrate;
            Fat += nutrient.Fat;
        }

        foreach (var nutrient in Dinner)
        {
            Protein += nutrient.Protein;
            Carbohydrate += nutrient.Carbohydrate;
            Fat += nutrient.Fat;
        }
    }
}
