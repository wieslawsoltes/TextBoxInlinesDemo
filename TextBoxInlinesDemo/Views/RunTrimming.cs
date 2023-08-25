using Avalonia.Media;
using Avalonia.Media.TextFormatting;

namespace TextBoxInlinesDemo.Views;

public sealed class RunTrimming : TextTrimming
{
    private readonly string _ellipsis;
    
    public RunTrimming(string ellipsis)
    {
        _ellipsis = ellipsis;
    }

    public override TextCollapsingProperties CreateCollapsingProperties(TextCollapsingCreateInfo createInfo)
    {
        return new RunEllipsis(_ellipsis, createInfo.Width, createInfo.TextRunProperties, createInfo.FlowDirection);
    }

    public override string ToString()
    {
        return nameof(RunEllipsis);
    }
}
