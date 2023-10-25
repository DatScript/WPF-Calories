using Newtonsoft.Json;
using System.Collections.Generic;

namespace Calories.App.Models;

public class PersonalData
{
    [JsonProperty("tdee")]
    public int Tdee { get; set; }

    [JsonProperty("weight")]
    public int Weight { get; set; }
}

public class Meal
{
    [JsonProperty("breakfast")]
    public string[] Breakfast { get; set; }

    [JsonProperty("lunch")]
    public string[] Lunch { get; set; }

    [JsonProperty("dinner")]
    public string[] Dinner { get; set; }
}

public class MealPlan
{
    [JsonProperty("Personal")]
    public PersonalData Personal { get; set; }

    [JsonProperty("Meals")]
    public Dictionary<string, Meal> Meals { get; set; }
}
