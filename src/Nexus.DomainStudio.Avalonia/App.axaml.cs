using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Nexus.DomainStudio.Avalonia.Menu;
using Nexus.DomainStudio.Avalonia.Views;
using Nexus.DomainStudio.Avalonia.DependencyInjection;

namespace Nexus.DomainStudio.Avalonia;

/// <summary>
/// 
/// </summary>
public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    /// <summary>
    /// 
    /// </summary>
    public override void OnFrameworkInitializationCompleted()
    {
        // Create a service collection
        var services = new ServiceCollection();

        // Add the menu provider
        services.AddSingleton<IMenuProvider, MenuProvider>();

        // Add all the view models from the current assembly
        services.AddViewsWithModelsFrom(typeof(App).Assembly);

        // Build the service provider
        var provider = services.BuildServiceProvider();

        // Store the services for later use
        Services = provider;

        // Set the main window
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow();

        // Call the base function
        base.OnFrameworkInitializationCompleted();
    }
}