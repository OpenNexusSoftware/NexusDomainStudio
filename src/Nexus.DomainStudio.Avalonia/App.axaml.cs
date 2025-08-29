using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Nexus.DomainStudio.Avalonia.Views;

namespace Nexus.DomainStudio.Avalonia;

/// <summary>
/// 
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// 
    /// </summary>
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    /// <summary>
    /// 
    /// </summary>
    public override void OnFrameworkInitializationCompleted()
    {
        //
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow();

        // Call the base function
        base.OnFrameworkInitializationCompleted();
    }
}