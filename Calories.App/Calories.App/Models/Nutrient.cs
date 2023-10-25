namespace Calories.App.Models;
public class Nutrient
{
    public string Name { get; set; }
    public float Protein { get; set; }
    public float Carbohydrate { get; set; }
    public float Fat { get; set; }
    public float Amount { get; set; } = 1;
    public string? ImageUrl { get; set; }
    public string? Meal { get; set; }

    public Nutrient(string name)
    {
        Name = name;
    }

}
