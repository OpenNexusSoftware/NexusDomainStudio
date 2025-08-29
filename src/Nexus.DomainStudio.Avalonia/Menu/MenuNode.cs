using System.Windows.Input;

namespace Nexus.DomainStudio.Avalonia.Menu;

public sealed class MenuNode
{
    /// <summary>
    /// Menu item title.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Menu item command.
    /// </summary>
    public ICommand Command { get; init; } = null!;

    /// <summary>
    /// Child menu nodes.
    /// </summary>
    public IReadOnlyList<MenuNode> Children { get; init; } = Array.Empty<MenuNode>();
}
