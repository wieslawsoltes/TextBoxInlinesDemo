using Avalonia.Controls;

namespace TextBoxInlinesDemo.Views;

public partial class MainWindow : Window
{
    internal const string DefaultEllipsisChar = "\u2026";

    public static RunTrimming RunTrimming = new RunTrimming(DefaultEllipsisChar);

    public MainWindow()
    {
        InitializeComponent();
    }
}
