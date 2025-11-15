using DotNetCoreOutputToConsole.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCoreOutputToConsole.Tests.Config;
public class ConsoleOutputSettingsTests
{
    [Fact]
    public void EnabledInDevelopment()
    {
        ConsoleOutputSettings.AllowedEnvironments = new[] { "Development", "UAT" };
        ConsoleOutputSettings.CurrentEnvironment = "Development";

        Assert.True(ConsoleOutputSettings.IsEnabled);
    }

    [Fact]
    public void EnabledInUat()
    {
        ConsoleOutputSettings.AllowedEnvironments = new[] { "Development", "UAT" };
        ConsoleOutputSettings.CurrentEnvironment = "UAT";

        Assert.True(ConsoleOutputSettings.IsEnabled);
    }

    [Fact]
    public void DisabledInProduction()
    {
        ConsoleOutputSettings.AllowedEnvironments = new[] { "Development", "UAT" };
        ConsoleOutputSettings.CurrentEnvironment = "Production";

        Assert.False(ConsoleOutputSettings.IsEnabled);
    }

    [Fact]
    public void CustomEnvironmentSupported()
    {
        ConsoleOutputSettings.AllowedEnvironments = new[] { "QA" };
        ConsoleOutputSettings.CurrentEnvironment = "QA";

        Assert.True(ConsoleOutputSettings.IsEnabled);

        ConsoleOutputSettings.CurrentEnvironment = "Production";
        Assert.False(ConsoleOutputSettings.IsEnabled);
    }
}
