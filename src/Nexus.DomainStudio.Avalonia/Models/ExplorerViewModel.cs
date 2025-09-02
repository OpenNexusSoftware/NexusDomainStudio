using System.Collections.ObjectModel;
using Nexus.DomainStudio.Presentation.Attributes;

namespace Nexus.DomainStudio.Avalonia.Models;

public class ExplorerNode
{
    public string Title { get; }
    public ObservableCollection<ExplorerNode>? Children { get; }

    public ExplorerNode(string title) : this(title, null) {}

    public ExplorerNode(string title, ObservableCollection<ExplorerNode>? subNodes)
    {
        Title = title;
        Children = subNodes;
    }
}

/// <summary>
/// ViewModel for the <see cref="Views.Controls.ExplorerView"/>.
/// </summary>
[ViewModelFor(typeof(Views.Controls.ExplorerView), ServiceLifetime.Transient)]
public class ExplorerViewModel
{
    public ObservableCollection<ExplorerNode> Nodes { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExplorerViewModel"/> class.
    /// </summary>
    public ExplorerViewModel()
    {
        Nodes = [
            new ExplorerNode("Depots", [
                new ExplorerNode("Aggregates"),
                new ExplorerNode("Entities"),
                new ExplorerNode("Enums"),
                new ExplorerNode("Events"),
                new ExplorerNode("Operations"),
                new ExplorerNode("Value Objects")
            ]),
            new ExplorerNode("Logistics", [
                new ExplorerNode("Aggregates"),
                new ExplorerNode("Entities"),
                new ExplorerNode("Enums"),
                new ExplorerNode("Events"),
                new ExplorerNode("Operations"),
                new ExplorerNode("Value Objects")
            ]),
            new ExplorerNode("Products", [
                new ExplorerNode("Aggregates"),
                new ExplorerNode("Entities"),
                new ExplorerNode("Enums"),
                new ExplorerNode("Events"),
                new ExplorerNode("Operations"),
                new ExplorerNode("Value Objects")
            ]),
            new ExplorerNode("Users", [
                new ExplorerNode("Aggregates"),
                new ExplorerNode("Entities"),
                new ExplorerNode("Enums"),
                new ExplorerNode("Events"),
                new ExplorerNode("Operations"),
                new ExplorerNode("Value Objects")
            ]),
        ];
    }
}
