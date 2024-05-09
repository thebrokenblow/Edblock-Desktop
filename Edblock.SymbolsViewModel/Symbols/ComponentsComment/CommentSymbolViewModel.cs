﻿using Edblock.SymbolsViewModel.Attributes;
using Edblock.SymbolsViewModel.Core;
using EdblockViewModel.Symbols.ComponentsSymbolsVM;
using EdblockViewModel.Symbols.ComponentsSymbolsVM.ConnectionPoints;

namespace EdblockViewModel.Symbols.ComponentsCommentSymbolVM;

[SymbolType("CommentSymbolVM")]
public class CommentSymbolViewModel : BlockSymbolViewModel, IHasTextField
{
    public List<CommentLine> HorizontalLines { get; set; } = [];
    public CommentLine VerticalBaseline { get; set; } = new();
    public CommentLine UpperHorizontalBaseline { get; set; } = new();
    public CommentLine LowerHorizontalBaseline { get; set; } = new();
    public BlockSymbolViewModel? BlockSymbolVM { get; set; }
    public ConnectionPointVM? ConnectionPointVM { get; set; }

    private readonly CanvasSymbolsVM _canvasSymbolsVM;

    private double heightTextField;
    public double HeightTextField
    {
        get => heightTextField;
        set
        {
            heightTextField = GetTextFieldHeight(value);

            SetHeight(heightTextField);
        }
    }

    private double widthTextField;
    public double WidthTextField
    {
        get => widthTextField;
        set
        {
            widthTextField = value;
            SetWidth(widthTextField);
        }
    }

    public TextFieldSymbol TextFieldSymbol { get; init; }

    private const string defaultText = "Комментарий";

    private const int defaultHeight = 60;
    private const int countHorizontalLine = 3;
    private const int lengthHorizontalLine = 20;
    private const int spaceBetweenHorizontalLines = 10;
    public CommentSymbolViewModel(EditorVM edblockVM) : base(edblockVM)
    {
        _canvasSymbolsVM = edblockVM.CanvasSymbolsVM;

        HeightTextField = defaultHeight;

        InitHorizontalLine();

        double xCoordinateHorizontalLines = SetCoordinateHorizontalLines();

        SetCoordinateVerticalBaseline(xCoordinateHorizontalLines);
        SetCoordinateUpperHorizontalBaseline(xCoordinateHorizontalLines);
        SetCoordinateLowerHorizontalBaseline(xCoordinateHorizontalLines);

        TextFieldSymbol = new(_canvasSymbolsVM, this)
        {
            Text = defaultText,
            LeftOffset = UpperHorizontalBaseline.X1,
        };

        Width = UpperHorizontalBaseline.X2 + widthTextField;
        Height = heightTextField;
    }

    private void InitHorizontalLine()
    {
        for (int i = 0; i < countHorizontalLine; i++)
        {
            var сommentHorizontalLine = new CommentLine();
            HorizontalLines.Add(сommentHorizontalLine);
        }
    }

    private double SetCoordinateHorizontalLines()
    {
        double xCoordinateHorizontalLines = 0;

        foreach (var horizontalLine in HorizontalLines)
        {
            horizontalLine.X1 = xCoordinateHorizontalLines;
            horizontalLine.Y1 = heightTextField / 2;
            horizontalLine.X2 = xCoordinateHorizontalLines + lengthHorizontalLine;
            horizontalLine.Y2 = heightTextField / 2;

            xCoordinateHorizontalLines = horizontalLine.X2 + spaceBetweenHorizontalLines;
        }

        xCoordinateHorizontalLines -= spaceBetweenHorizontalLines;

        return xCoordinateHorizontalLines;
    }

    private void SetCoordinateVerticalBaseline(double xCoordinateHorizontalLines)
    {
        SetWidthVerticalBaseline(xCoordinateHorizontalLines);
        SetHeightVerticalBaseline();
    }

    private void SetWidthVerticalBaseline(double xCoordinateHorizontalLines)
    {
        VerticalBaseline.X1 = xCoordinateHorizontalLines;
        VerticalBaseline.X2 = xCoordinateHorizontalLines;
    }

    private void SetHeightVerticalBaseline()
    {
        VerticalBaseline.Y1 = 0;
        VerticalBaseline.Y2 = heightTextField;
    }

    private void SetCoordinateUpperHorizontalBaseline(double xCoordinateHorizontalLines)
    {
        SetWidthUpperHorizontalBaseline(xCoordinateHorizontalLines);
        SetHeightUpperHorizontalBaseline();
    }

    private void SetWidthUpperHorizontalBaseline(double xCoordinateHorizontalLines)
    {
        UpperHorizontalBaseline.X1 = xCoordinateHorizontalLines;
        UpperHorizontalBaseline.X2 = xCoordinateHorizontalLines + lengthHorizontalLine;
    }

    private void SetHeightUpperHorizontalBaseline()
    {
        UpperHorizontalBaseline.Y1 = 0;
        UpperHorizontalBaseline.Y2 = 0;
    }

    private void SetCoordinateLowerHorizontalBaseline(double xCoordinateHorizontalLines)
    {
        SetWidthLowerHorizontalBaseline(xCoordinateHorizontalLines);
        SetHeightLowerHorizontalBaseline();
    }

    private void SetWidthLowerHorizontalBaseline(double xCoordinateHorizontalLines)
    {
        LowerHorizontalBaseline.X1 = xCoordinateHorizontalLines;
        LowerHorizontalBaseline.X2 = xCoordinateHorizontalLines + lengthHorizontalLine;
    }

    private void SetHeightLowerHorizontalBaseline()
    {
        LowerHorizontalBaseline.Y1 = heightTextField;
        LowerHorizontalBaseline.Y2 = heightTextField;
    }

    public override void SetWidth(double width)
    {
        Width = UpperHorizontalBaseline.X1 + width;
    }

    public override void SetHeight(double height)
    {
        SetCoordinateHorizontalLines();
        SetHeightVerticalBaseline();
        SetHeightLowerHorizontalBaseline();

        YCoordinate -= (height - Height) / 2;

        Height = height;
    }

    private static double GetTextFieldHeight(double heightTextField)
    {
        if (heightTextField % 10 > 0)
        {
            heightTextField = heightTextField - heightTextField % 10 + 10;
        }

        return heightTextField;
    }
}