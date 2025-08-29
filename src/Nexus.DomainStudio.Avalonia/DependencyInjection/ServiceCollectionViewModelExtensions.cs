using System.Reflection;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nexus.DomainStudio.Avalonia.DependencyInjection;

/// <summary>
/// Extension methods for registering view models and views in the service collection.
/// </summary>
public static class ServiceCollectionViewModelExtensions
{
    /// <summary>
    /// Registers all view models from the specified assemblies into the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddViewsWithModelsFrom(this IServiceCollection services, params Assembly[] assemblies)
    {
        // Get all assemblies to scan
        var allAssemblies = assemblies.Length == 0 ? AppDomain.CurrentDomain.GetAssemblies() : assemblies;

        // Loop trough all the assemblies
        foreach (var asm in allAssemblies)
        {
            // Loop trough all the types in the assembly
            foreach (var type in asm.GetTypes())
            {
                // Check if the type is a not a class or is abstract, if so we skip it
                if(!type.IsClass || type.IsAbstract) continue;

                // Get the custom attribute type
                var attr = type.GetCustomAttribute<Presentation.Attributes.ViewModelForAttribute>();

                // If the attribute is not found, we skip it
                if(attr == null) continue;

                // Map the view model type and view type
                var viewModelType = type;
                var viewType = attr.ViewType;

                // Register the type based on the specified lifetime
                switch(attr.ServiceLifetime)
                {
                    // Add the type as transient in case of transient lifetime
                    case Presentation.Attributes.ServiceLifetime.Transient:
                        services.TryAddTransient(viewModelType);
                        break;
                    // Add the type as scoped in case of scoped lifetime
                    case Presentation.Attributes.ServiceLifetime.Scoped:
                        services.TryAddScoped(viewModelType);
                        break;
                    // Add the type as singleton in case of singleton lifetime
                    case Presentation.Attributes.ServiceLifetime.Singleton:
                        services.TryAddSingleton(viewModelType);
                        break;
                }

                // Register the view type as transient
                services.AddTransient(viewType);

                // Register the view type as transient and set its DataContext to the resolved ViewModel
                // services.AddTransient(viewType, sp =>
                // {
                //     var view = Activator.CreateInstance(viewType);
                //     if (view == null) throw new InvalidOperationException($"Could not create instance of type {viewType.FullName}");

                //     // Get the view model from the service provider
                //     var viewModel = sp.GetRequiredService(viewModelType);
                //     (view as Control)!.DataContext = viewModel;
                //     return view;
                // });
            }
        }

        // Return the modified service collection
        return services;
    }
}