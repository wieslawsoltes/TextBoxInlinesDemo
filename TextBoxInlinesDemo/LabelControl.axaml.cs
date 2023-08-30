using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace TextBoxInlinesDemo;

public class LabelControl : TemplatedControl
{
    public static readonly StyledProperty<string?> TextProperty = 
        AvaloniaProperty.Register<LabelControl, string?>(nameof(Text));

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}

