using Calories.App.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Calories.App.ViewModels;
public partial class VmMainWindow : ObservableObject
{
    public string Title => "Calories";

    [ObservableProperty]
    private float _protein;

    [ObservableProperty]
    private float _carbohydrate;

    [ObservableProperty]
    private float _fat;

    [ObservableProperty]
    private ObservableCollection<Nutrient> _nutrients = new();

    [ObservableProperty]
    private ObservableCollection<Nutrient> _breakfast = new();

    [ObservableProperty]
    private ObservableCollection<Nutrient> _lunch = new();

    [ObservableProperty]
    private ObservableCollection<Nutrient> _dinner = new();

    public VmMainWindow()
    {
        Task.Run(InitializeAsync);
    }

    public async Task InitializeAsync()
    {
        Nutrients = await LoadDataAsync();
    }

    private async Task<ObservableCollection<Nutrient>> LoadDataAsync()
    {
        const string url = "https://raw.githubusercontent.com/DatScript/WPF-Calories/main/Database/Nutrient.csv";
        var nutrients = new ObservableCollection<Nutrient>();

        using var client = new HttpClient();
        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode) return nutrients;
        var csvData = await response.Content.ReadAsStringAsync();
        var lines = csvData.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var values = line.Split(',');

            if (values.Length != 5) continue;
            var nutrient = new Nutrient(values[0])
            {
                Protein = float.Parse(values[1]),
                Carbohydrate = float.Parse(values[2]),
                Fat = float.Parse(values[3]),
                ImageUrl = values[4]
            };
            nutrients.Add(nutrient);
        }
        // sort by protein

        nutrients = nutrients.OrderByDescending(x => x.Protein).ToObservableCollection();
        return nutrients;
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