using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nexus.DomainStudio.Avalonia.Views.Controls;

/// <summary>
/// Interaction logic for ExplorerView.xaml
/// </summary>
public partial class ExplorerView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExplorerView"/> class.
    /// </summary>
    public ExplorerView() => AvaloniaXamlLoader.Load(this);
}