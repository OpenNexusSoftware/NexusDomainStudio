using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nexus.DomainStudio.Presentation.Attributes;

namespace Nexus.DomainStudio.Avalonia.Models;

[ViewModelFor(typeof(Views.MainWindow), ServiceLifetime.Transient)]
public partial class MainWindowViewModel : ObservableObject
{
    /// <summary>
    /// Menu bar view model.
    /// </summary>
    public MenuBarViewModel MenuBar { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
    /// </summary>
    /// <param name="menuBar"></param>
    public MainWindowViewModel(MenuBarViewModel menuBar) => MenuBar = menuBar;

    [ObservableProperty]
    private int count = 0;

    [RelayCommand]
    private void Increment() => Count++;
}