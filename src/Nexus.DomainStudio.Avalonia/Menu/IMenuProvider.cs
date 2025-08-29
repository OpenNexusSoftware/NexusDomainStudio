namespace Nexus.DomainStudio.Avalonia.Menu;

/// <summary>
/// Provides menu structure for the application.
/// </summary>
public interface IMenuProvider
{
    /// <summary>
    /// Gets the menu structure as a collection of <see cref="MenuNode"/> objects.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<MenuNode> GetMenu();
}