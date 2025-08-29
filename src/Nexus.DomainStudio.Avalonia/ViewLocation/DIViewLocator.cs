using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace Nexus.DomainStudio.Avalonia.ViewLocation;

/// <summary>
/// A data template that resolves views using dependency injection.
/// </summary>
public sealed class DIViewLocator : IDataTemplate
{
    /// <summary>
    /// Builds a view for the given data context using dependency injection.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public Control Build(object? data)
    {
        // If the data provided is null, we return a TextBlock indicating no DataContext
        if (data == null) return new TextBlock { Text = $"ERROR: No DataContext provided" };

        // Get the type of the view model and the associated ViewModelForAttribute
        var viewModelType = data.GetType();
        var attr = viewModelType.GetCustomAttribute<Presentation.Attributes.ViewModelForAttribute>();

        // If the attribute is not found, we return a TextBlock indicating the error
        if (attr == null) return new TextBlock { Text = $"ERROR: Attribute has not been set for ViewModel of type: {viewModelType.Name}" };

        // Get the view type from the attribute
        var viewType = attr.ViewType;

        // Get the view instance from the service provider
        var viewObj = App.Services.GetRequiredService(viewType);

        // Check if the resolved view is a Control, if not we return a TextBlock indicating the error
        if (viewObj is not Control view) return new TextBlock { Text = $"ERROR: Resolved view is not a Control for ViewModel of type: {viewModelType.Name}" };

        // Set the DataContext of the view to the provided data and return the view
        view.DataContext = data;

        // Return the resolved view
        return view;
    }

    /// <summary>
    /// Determines whether this template can build a view for the given data context.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool Match(object? data)
    {
        // Check if the data is not null
        if (data == null) return false;

        // Check if the data type has the ViewModelForAttribute
        var viewModelType = data.GetType();
        var attr = viewModelType.GetCustomAttribute<Presentation.Attributes.ViewModelForAttribute>();

        // Return true if the attribute is found, otherwise false
        return attr != null;
    }
}