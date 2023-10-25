using Calories.App.Models;
using Calories.App.Properties;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Calories.App.ViewModels;
public partial class VmMainWindow : ObservableObject
{
    [ObservableProperty]
    private string _title = "Calories";

    [ObservableProperty]
    private int _tdee = 1973;

    [ObservableProperty]
    private int _weight = 48;

    [ObservableProperty]
    private float _protein;

    [ObservableProperty]
    private float _carbohydrate;

    [ObservableProperty]
    private float _fat;

    [ObservableProperty]
    private ObservableCollection<Nutrient> _nutrients = new();

    [ObservableProperty]
    private ObservableCollection<DayDetail> _dayDetails = new();

    [ObservableProperty]
    private DayDetail? _dayDetail;

    private string _filePath = string.Empty;

    public VmMainWindow()
    {
        Task.Run(InitializeAsync);
    }

    public async Task InitializeAsync()
    {
        DayDetails = new ObservableCollection<DayDetail>
        {
            new DayDetail("Day 1"),
            new DayDetail("Day 2"),
            new DayDetail("Day 3"),
            new DayDetail("Day 4"),
            new DayDetail("Day 5"),
            new DayDetail("Day 6"),
            new DayDetail("Day 7")
        };
        Nutrients = await LoadDataAsync();
        var filePath = Settings.Default.FilePath;
        if (!string.IsNullOrEmpty(filePath))
        {
            _filePath = filePath;
            Title = $"Calories - {Path.GetFileName(filePath)}";
            Load();
        }
        Calculate();
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

    partial void OnWeightChanged(int value)
    {
        Calculate();
    }

    partial void OnTdeeChanged(int value)
    {
        Calculate();
    }

    private void Calculate()
    {
        var needCalories = Tdee - 500;
        Protein = 2.6f * Weight;
        Fat = (needCalories - (Protein * 4)) / 15;
        Carbohydrate = 1.5f * Fat;
    }

    [RelayCommand]
    public void Load()
    {
        // Open file dialog to select a file
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "JSON files (*.json)|*.json";
        openFileDialog.Title = "Select JSON File";
        // if _filePath is not empty, set the initial directory to the file's directory
        if (!string.IsNullOrEmpty(_filePath))
        {
            openFileDialog.InitialDirectory = Path.GetDirectoryName(_filePath);
        }
        else
        {
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        if (openFileDialog.ShowDialog() == true)
        {
            _filePath = openFileDialog.FileName;
            Title = $"Calories - {openFileDialog.SafeFileName.Split(".")[0]}";
            // Load JSON data from the selected file
            string jsonData = File.ReadAllText(openFileDialog.FileName);

            // Deserialize JSON to the MealPlan object
            var mealPlan = JsonConvert.DeserializeObject<MealPlan>(jsonData);

            if (mealPlan == null)
            {
                return;
            }
            Tdee = mealPlan.Personal.Tdee;
            Weight = mealPlan.Personal.Weight;
            Calculate();
            DayDetails = new ObservableCollection<DayDetail>();
            foreach (var (key, value) in mealPlan.Meals)
            {
                var dayDetail = new DayDetail(key);
                foreach (var nutrient in value.Breakfast)
                {
                    var item = Nutrients.FirstOrDefault(x => x.Name == nutrient);
                    if (item != null)
                    {
                        dayDetail.Breakfast.Add(item);
                    }
                }
                foreach (var nutrient in value.Lunch)
                {
                    var item = Nutrients.FirstOrDefault(x => x.Name == nutrient);
                    if (item != null)
                    {
                        dayDetail.Lunch.Add(item);
                    }
                }

                foreach (var nutrient in value.Dinner)
                {
                    var item = Nutrients.FirstOrDefault(x => x.Name == nutrient);
                    if (item != null)
                    {
                        dayDetail.Dinner.Add(item);
                    }
                }
                dayDetail.Calculate();
                DayDetails.Add(dayDetail);
            }
        }
        else
        {
            Console.WriteLine("No file selected.");
        }
    }

    [RelayCommand]
    public void Save()
    {
        if (string.IsNullOrEmpty(_filePath))
        {

            // Open folder dialog to select a save directory
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "JSON files (*.json)|*.json";
            saveFileDialog.Title = "Save Meal Plan";
            if (saveFileDialog.ShowDialog() == true)
            {
                PopulateAndSave();
            }
            else
            {
                Console.WriteLine("Save operation cancelled.");
            }
        }
        else
        {
            PopulateAndSave();
        }
    }

    private void PopulateAndSave()
    {
        var mealPlan = new MealPlan()
        {
            Personal = new PersonalData()
            {
                Tdee = Tdee,
                Weight = Weight
            },
            Meals = new Dictionary<string, Meal>()
        };
        foreach (var dayDetail in DayDetails)
        {
            var meal = new Meal()
            {
                Breakfast = dayDetail.Breakfast.Select(x => x.Name).ToArray(),
                Lunch = dayDetail.Lunch.Select(x => x.Name).ToArray(),
                Dinner = dayDetail.Dinner.Select(x => x.Name).ToArray()
            };
            mealPlan.Meals.Add(dayDetail.Name, meal);
        }
        try
        {
            // Serialize the MealPlan object to JSON
            string jsonContent = JsonConvert.SerializeObject(mealPlan, Formatting.Indented);

            // Write the JSON content to the file
            File.WriteAllText(_filePath, jsonContent);
            MessageBox.Show("MealPlan saved successfully.", "Calories", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving MealPlan: {ex.Message}");
        }
    }
}