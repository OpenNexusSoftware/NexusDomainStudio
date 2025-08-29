using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace Nexus.DomainStudio.Avalonia;

public sealed class NexusDomainStudioApp : Application
{
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow();

        base.OnFrameworkInitializationCompleted();
    }
}