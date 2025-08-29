using Avalonia.Controls;
using Avalonia.Layout;

namespace Nexus.DomainStudio.Avalonia;

public sealed class MainWindow : Window
{
    public MainWindow()
    {
        Title = "Hello World!";
        Width = 600;
        Height = 400;

        var stack = new StackPanel
        {
            Spacing = 8,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        var text = new TextBlock { Text = "Hello world!", FontSize = 24 };
        var button = new Button { Content = "Click me", Width = 120 };

        int count = 0;
        var counter = new TextBlock { Text = "Clicks: 0" };
        button.Click += (_, __) => counter.Text = $"Clicks: {++count}";

        stack.Children.Add(text);
        stack.Children.Add(button);
        stack.Children.Add(counter);

        Content = stack;
    }
}