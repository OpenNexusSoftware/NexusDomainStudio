namespace Nexus.DomainStudio.Avalonia.Menu;

/// <summary>
/// Provides the menu structure for the application.
/// </summary>
public sealed class MenuProvider : IMenuProvider
{
    /// <summary>
    /// Gets the menu structure as a collection of <see cref="MenuNode"/> objects.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEnumerable<MenuNode> GetMenu()
    {
        return [
            new MenuNode
            {
                Title = "_File",
                Children = [
                    new MenuNode
                    {
                        Title = "_New",
                    },
                    new MenuNode
                    {
                        Title = "_Open",
                    },
                    new MenuNode
                    {
                        Title = "_Save",
                    },
                    new MenuNode
                    {
                        Title = "Save _As",
                    },
                    new MenuNode
                    {
                        Title = "_Exit",
                    },
                ],
            },
            new MenuNode
            {
                Title = "_Edit",
                Children = [
                    new MenuNode
                    {
                        Title = "_Undo",
                    },
                    new MenuNode
                    {
                        Title = "_Redo",
                    },
                    new MenuNode
                    {
                        Title = "_Cut",
                    },
                    new MenuNode
                    {
                        Title = "_Copy",
                    },
                    new MenuNode
                    {
                        Title = "_Paste",
                    },
                ],
            },
            new MenuNode
            {
                Title = "_View",
            },
            new MenuNode
            {
                Title = "_Help",
            },
        ];
    }
}