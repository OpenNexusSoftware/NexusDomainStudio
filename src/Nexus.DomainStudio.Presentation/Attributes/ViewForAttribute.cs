namespace Nexus.DomainStudio.Presentation.Attributes;

/// <summary>
/// Defines the lifetime of a service in dependency injection.
/// </summary>
public enum ServiceLifetime { Transient, Scoped, Singleton }

/// <summary>
/// Attribute to associate a view with a specific ViewModel type.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ViewModelForAttribute : Attribute
{
    /// <summary>
    /// The type of the ViewModel that this view is associated with.
    /// </summary>
    public Type ViewType { get; }

    /// <summary>
    /// The lifetime of the ViewModel in dependency injection.
    /// </summary>
    public ServiceLifetime ServiceLifetime { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewForAttribute"/> class.
    /// </summary>
    /// <param name="viewModelType"></param>
    public ViewModelForAttribute(Type viewType, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        ViewType = viewType;
        ServiceLifetime = lifetime;
    }
}
