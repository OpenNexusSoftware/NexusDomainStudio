using Nexus.DomainStudio.Avalonia.Menu;
using Nexus.DomainStudio.Presentation.Attributes;

namespace Nexus.DomainStudio.Avalonia.Models;

[ViewModelFor(typeof(Views.Controls.MenuBarView), ServiceLifetime.Transient)]
public class MenuBarViewModel
{
    private readonly IEnumerable<MenuNode> _menuItems;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuBarViewModel"/> class.
    /// </summary>
    /// <param name="menuProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public MenuBarViewModel(IMenuProvider menuProvider)
    {
        _menuItems = menuProvider?.GetMenu() ?? throw new ArgumentNullException(nameof(menuProvider));
    }

    /// <summary>
    /// Gets the menu items to be displayed in the menu bar.
    /// </summary>
    public IEnumerable<MenuNode> Items => _menuItems;
}
