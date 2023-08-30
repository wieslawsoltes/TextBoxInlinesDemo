using System;
using System.Collections.Generic;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;

namespace TextBoxInlinesDemo.Views;

public sealed class RunEllipsis : TextCollapsingProperties
{
    public RunEllipsis(
        string ellipsis,
        double width,
        TextRunProperties textRunProperties,
        FlowDirection flowDirection)
    {
        Width = width;
        Symbol = new TextCharacters(ellipsis, textRunProperties);
        FlowDirection = flowDirection;
    }

    internal static ShapedTextRun CreateSymbol(TextRun textRun, FlowDirection flowDirection)
    {
        var textShaper = TextShaper.Current;

        var glyphTypeface = textRun.Properties!.Typeface.GlyphTypeface;

        var fontRenderingEmSize = textRun.Properties.FontRenderingEmSize;

        var cultureInfo = textRun.Properties.CultureInfo;

        var shaperOptions = new TextShaperOptions(glyphTypeface, fontRenderingEmSize, (sbyte)flowDirection, cultureInfo);

        var shapedBuffer = textShaper.ShapeText(textRun.Text, shaperOptions);

        return new ShapedTextRun(shapedBuffer, textRun.Properties);
    }
    
    public override TextRun[]? Collapse(TextLine textLine)
    {
        var shapedSymbol = CreateSymbol(Symbol, FlowDirection);

        //var lastArray = textLine.TextRuns.TakeLast(1).ToArray();
        //return lastArray;
        
        if (Width < shapedSymbol.GlyphRun.Bounds.Width)
        {
            //Not enough space to fit in the symbol
            return Array.Empty<TextRun>();
        }

        var currentWidth = 0.0;
        var availableWidth = Width - shapedSymbol.Size.Width;
        var textRuns = new List<TextRun>();

        // Console.WriteLine($"availableWidth: {availableWidth}, Width: {Width}, shapedSymbol.Size.Width: {shapedSymbol.Size.Width}");
        
        for (var i = 0; i < textLine.TextRuns.Count; i++)
        {
            var currentRun = textLine.TextRuns[i];

            // Console.WriteLine($"currentRun[{i}]: {currentRun.GetType()}");
            switch (currentRun)
            {
                case ShapedTextRun shapedRun:
                {
                    currentWidth = shapedRun.Size.Width;

                    if (currentWidth > availableWidth)
                    {
                        // Console.WriteLine($"Collapse ShapedTextRun[{i}]: {currentRun.GetType()} '{currentRun.Text}', availableWidth: {availableWidth}, currentWidth: {currentWidth}");
                        textRuns.Add(shapedSymbol);
                        return textRuns.ToArray();
                    }

                    // Console.WriteLine($"Add ShapedTextRun[{i}]: {currentRun.GetType()} '{currentRun.Text}', availableWidth: {availableWidth}, currentWidth: {currentWidth}");
                    textRuns.Add(shapedRun);

                    availableWidth -= shapedRun.Size.Width;

                    break;
                }

                case DrawableTextRun drawableRun:
                {
                    currentWidth = drawableRun.Size.Width;

                    if (currentWidth > availableWidth)
                    {
                        // Console.WriteLine($"Collapse DrawableTextRun[{i}]: {currentRun.GetType()} '{currentRun.Text}', availableWidth: {availableWidth}, currentWidth: {currentWidth}");
                        textRuns.Add(shapedSymbol);
                        return textRuns.ToArray();
                    }

                    // Console.WriteLine($"Add DrawableTextRun[{i}]: {currentRun.GetType()} '{currentRun.Text}', availableWidth: {availableWidth}, currentWidth: {currentWidth}");
                    textRuns.Add(drawableRun);

                    availableWidth -= drawableRun.Size.Width;

                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        //return textRuns.ToArray();
        //return textLine.TextRuns.Take(3).ToArray();
        return Array.Empty<TextRun>();
    }

    public override double Width { get; }
    public override TextRun Symbol { get; }
    public override FlowDirection FlowDirection { get; }
}
