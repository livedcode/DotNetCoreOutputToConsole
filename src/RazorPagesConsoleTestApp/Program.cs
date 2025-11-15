using DotNetCoreOutputToConsole.Config;

var builder = WebApplication.CreateBuilder(args);

ConsoleOutputSettings.CurrentEnvironment = builder.Environment.EnvironmentName;

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
