using System;

namespace Calories.App.Helper;
public static class Utils
{
    public static string AppName => "Calories.App";
    public static string AppFolder => $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}//{AppName}";
}