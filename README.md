# DotNetCoreOutputToConsole  

A lightweight ASP.NET Core helper library that allows developers to output logs, variables, and errors directly into the **browser console** using inline `<script>` injection.

Ideal for **Development**, **Debug**, and **UAT** environments.

---

# 📥 Download

You can download the full source code from your GitHub repository once uploaded:

`https://github.com/<your-account>/DotNetCoreOutputToConsole`

If you need a GitHub release ZIP template, just tell me.

---

# ✨ Features

- Write messages directly to **browser DevTools console**
- Supports multiple console API functions:
  - `console.log`
  - `console.info`
  - `console.warn`
  - `console.error`
  - `console.trace`
  - `console.table`
- Automatically JSON-encodes objects (safe for XSS)
- Easy integration with ASP.NET Core
- Only runs in:
  - Development  
  - Staging  
  - UAT
- Works with:
  - **Minimal API**
  - **MVC**
  - **Razor Pages**

---

# ❗ Not Supported

This library **does NOT support Blazor Web App, Blazor Server, or Blazor WebAssembly**.

Blazor does NOT execute inline `<script>` inside its rendering pipeline.  
To support Blazor, a JSInterop-based version would be required (future extension).

---

# 📦 Installation

Add the library project reference:

```xml
<ProjectReference Include="..\DotNetCoreOutputToConsole\DotNetCoreOutputToConsole.csproj" />
```

Or include the source project directly in your solution.

---

# ⚙ Environment Setup

Set the environment name so console output only appears in safe environments:

```csharp
ConsoleOutputSettings.CurrentEnvironment = builder.Environment.EnvironmentName;
```

The library will ONLY output console messages when the environment is:

- `Development`
- `Staging`
- `UAT`

In `Production`, all calls become **no-ops** (silently ignored).

---

# 🚀 Usage Examples

## ⭐ Minimal API Example

```csharp
app.MapGet("/test", async ctx =>
{
    await BrowserConsole.Log(ctx.Response, "Hello from Minimal API");
    await BrowserConsole.Info(ctx.Response, new { User = "livecode", Time = DateTime.UtcNow });
    await BrowserConsole.Warn(ctx.Response, "This is a warning");
    await BrowserConsole.Error(ctx.Response, "This is an error");
    await BrowserConsole.Trace(ctx.Response, "Trace message");
    await BrowserConsole.Table(ctx.Response, new[] {
        new { Id = 1, Name = "Item A" },
        new { Id = 2, Name = "Item B" }
    });

    await ctx.Response.WriteAsync("<h3>Console test complete – open DevTools.</h3>");
});
```

---

## ⭐ MVC Example

### Controller

```csharp
public class HomeController : Controller
{
    public IActionResult Index()
    {
        BrowserConsole.Log(Response, "MVC HomeController loaded");
        BrowserConsole.Info(Response, new { Page = "Home/Index", Time = DateTime.UtcNow });
        BrowserConsole.Warn(Response, "This is a test warning from MVC");
        BrowserConsole.Error(Response, new { Error = "Something failed", Code = 500 });

        return View();
    }
}
```

### View (`Views/Home/Index.cshtml`)

```html
<h2>MVC Console Test Page</h2>
<p>Check your browser console.</p>
```

---

## ⭐ Razor Pages Example

### `Pages/Index.cshtml.cs`

```csharp
public class IndexModel : PageModel
{
    public void OnGet()
    {
        BrowserConsole.Info(Response, "Razor Page Loaded");
        BrowserConsole.Table(Response, new[] {
            new { Id = 1, Description = "Razor Row 1" },
            new { Id = 2, Description = "Razor Row 2" }
        });
    }
}
```

### `Pages/Index.cshtml`

```html
@page
@model IndexModel

<h3>Razor Pages Console Test</h3>
<p>Open browser DevTools → Console.</p>
```

---

# 🧩 Console API Supported

| Function           | Method in Library           | Example Usage                                |
|-------------------|-----------------------------|----------------------------------------------|
| `console.log`     | `BrowserConsole.Log()`      | `await BrowserConsole.Log(Response, "msg");` |
| `console.info`    | `BrowserConsole.Info()`     | `await BrowserConsole.Info(Response, obj);`  |
| `console.warn`    | `BrowserConsole.Warn()`     | `await BrowserConsole.Warn(Response, "warn");` |
| `console.error`   | `BrowserConsole.Error()`    | `await BrowserConsole.Error(Response, ex);`  |
| `console.trace`   | `BrowserConsole.Trace()`    | `await BrowserConsole.Trace(Response, "trace");` |
| `console.table`   | `BrowserConsole.Table()`    | `await BrowserConsole.Table(Response, list);` |

All methods serialize objects to JSON and safely embed them into `<script>` blocks, which are then executed by the browser.

---

# 🔐 Security Notes

- Objects are JSON-encoded before being written into the script.
- Output is restricted to non-production environments via `ConsoleOutputSettings`.
- If writing to the response fails (stream closed, headers sent, etc.), the library will silently ignore the error so it never breaks your application behavior.

---

# 📁 Project Structure

A typical solution layout:

```text
DotNetCoreOutputToConsole/
│
├── src/
│   ├── DotNetCoreOutputToConsole/
│   │   ├── DotNetCoreOutputToConsole.csproj
│   │   ├── BrowserConsole.cs
│   │   └── Config/
│   │       └── ConsoleOutputSettings.cs
│   │
│   ├── DotNetCoreOutputToConsole.Tests/
│   │   ├── DotNetCoreOutputToConsole.Tests.csproj
│   │   ├── BrowserConsoleTests.cs
│   │   └── Config/
│   │       └── ConsoleOutputSettingsTests.cs
│   │
│   ├── MinimalApiConsoleTestApp/
│   │   ├── MinimalApiConsoleTestApp.csproj
│   │   └── Program.cs
│   │
│   ├── MvcConsoleTestApp/
│   │   ├── MvcConsoleTestApp.csproj
│   │   ├── Program.cs
│   │   ├── Controllers/
│   │   │   └── HomeController.cs
│   │   └── Views/
│   │       └── Home/
│   │           └── Index.cshtml
│   │
│   └── RazorPagesConsoleTestApp/
│       ├── RazorPagesConsoleTestApp.csproj
│       ├── Program.cs
│       └── Pages/
│           ├── Index.cshtml
│           └── Index.cshtml.cs
│
└── README.md
```

---

# 🧪 Unit Tests

Unit tests (in `DotNetCoreOutputToConsole.Tests`) may include:

- Verifying console script injection content contains the correct function (log/info/warn/error/trace/table).
- Ensuring no content is written when environment is Production.
- Verifying that different message types (string, object, collections) serialize correctly to JSON.

Example snippet:

```csharp
[Fact]
public async Task Log_WritesConsoleLogScriptInDev()
{
    ConsoleOutputSettings.CurrentEnvironment = "Development";

    var ctx = new DefaultHttpContext();
    ctx.Response.Body = new MemoryStream();

    await BrowserConsole.Log(ctx.Response, "Test message");

    ctx.Response.Body.Seek(0, SeekOrigin.Begin);
    var html = new StreamReader(ctx.Response.Body).ReadToEnd();

    Assert.Contains("console.log", html);
    Assert.Contains("Test message", html);
}
```

---

# 👤 Author

**livecode**

---

# 📝 License

MIT License — see `LICENSE`.

---

# ⭐ Support This Project

If this library makes your ASP.NET Core debugging easier, consider giving it a ⭐ on GitHub!
