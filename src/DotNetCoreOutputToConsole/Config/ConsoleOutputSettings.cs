
namespace DotNetCoreOutputToConsole.Config;
public static class ConsoleOutputSettings
{
    /// <summary>
    /// Environments where console output is allowed.
    /// Default: Development, UAT.
    /// </summary>
    public static string[] AllowedEnvironments { get; set; } =
    {
            "Development",
            "UAT"
        };

    /// <summary>
    /// Current ASP.NET Core environment name (set in Program.cs).
    /// </summary>
    public static string CurrentEnvironment { get; set; } = "Production";

    /// <summary>
    /// True when current environment is in AllowedEnvironments.
    /// </summary>
    public static bool IsEnabled =>
        AllowedEnvironments.Contains(CurrentEnvironment);
}
