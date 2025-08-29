using Avalonia;

namespace Nexus.DomainStudio.Avalonia;

/// <summary>
/// Entrypoint for the Avalonia UI app for Nexus Domain Studio
/// </summary>
internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        // Build and configure the app
        var appBuilder = AppBuilder.Configure<App>();
        appBuilder.UsePlatformDetect();
        appBuilder.LogToTrace();

        // Start the app
        appBuilder.StartWithClassicDesktopLifetime(args);
    }
}