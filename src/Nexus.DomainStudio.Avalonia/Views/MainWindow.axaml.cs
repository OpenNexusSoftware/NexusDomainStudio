using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nexus.DomainStudio.Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        // Load the XAML components
        InitializeComponent();

        // Set the data context using the service provider
        if (App.Services is null) throw new InvalidOperationException("Service provider is not initialized.");

        // Resolve and set the DataContext to the MainWindowViewModel
        DataContext = App.Services.GetService(typeof(Models.MainWindowViewModel));
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
