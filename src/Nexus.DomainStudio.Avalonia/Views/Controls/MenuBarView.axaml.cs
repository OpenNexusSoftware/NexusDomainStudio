using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nexus.DomainStudio.Avalonia.Views.Controls;

/// <summary>
/// Interaction logic for MenuBarView.xaml
/// </summary>
public partial class MenuBarView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MenuBarView"/> class.
    /// </summary>
    public MenuBarView() => AvaloniaXamlLoader.Load(this);
}
