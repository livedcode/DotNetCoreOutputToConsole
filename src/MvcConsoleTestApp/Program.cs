using DotNetCoreOutputToConsole.Config;

var builder = WebApplication.CreateBuilder(args);

ConsoleOutputSettings.CurrentEnvironment = builder.Environment.EnvironmentName;

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapDefaultControllerRoute();
app.Run();
