﻿using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using EdblockViewModel.AttributeVM;
using EdblockViewModel.AbstractionsVM;
using EdblockViewModel.Symbols.ComponentsSymbolsVM;
using EdblockViewModel.Symbols.ComponentsSymbolsVM.ScaleRectangles;
using EdblockViewModel.Symbols.ComponentsSymbolsVM.ConnectionPoints;

namespace EdblockViewModel.Symbols;

[SymbolType("SubroutineSymbolVM")]

public class SubroutineSymbolVM : BlockSymbolVM, IHasTextFieldVM, IHasConnectionPoint, IHasScaleRectangles
{
    public TextFieldSymbolVM TextFieldSymbolVM { get; init; }
    public List<ScaleRectangle> ScaleRectangles { get; set; } = null!;
    public List<ConnectionPointVM> ConnectionPointsVM { get; set; } = null!;

    private readonly CoordinateConnectionPointVM coordinateConnectionPointVM;

    private PointCollection? points;
    public PointCollection? Points
    {
        get => points;
        set
        {
            points = value;
            OnPropertyChanged();
        }
    }

    private double widthBorder;
    public double WidthBorder
    {
        get => widthBorder;
        set
        {
            widthBorder = value;
            OnPropertyChanged();
        }
    }

    private double heightBorder;
    public double HeightBorder
    {
        get => heightBorder;
        set
        {
            heightBorder = value;
            OnPropertyChanged();
        }
    }

    public const int leftOffsetBorder = 20;
    public static int LeftOffsetBorder
    {
        get => leftOffsetBorder;
    }

    private const int defaultWidth = 140;
    private const int defaultHeigth = 60;

    private const string defaultText = "Подпрограмма";
    private const string defaultColor = "#FFBA64C8";

    public SubroutineSymbolVM(EdblockVM edblockVM) : base(edblockVM)
    {
        TextFieldSymbolVM = new(CanvasSymbolsVM, this)
        {
            Text = defaultText,
            LeftOffset = leftOffsetBorder
        };

        Color = defaultColor;

        AddScaleRectangles();
        AddConnectionPoints();

        coordinateConnectionPointVM = new(this);

        SetWidth(defaultWidth);
        SetHeight(defaultHeigth);
    }

    public override void SetWidth(double width)
    {
        Width = width;
        WidthBorder = width - leftOffsetBorder * 2;
        TextFieldSymbolVM.Width = WidthBorder;

        SetCoordinatePolygonPoints();
        ChangeCoordinateScaleRectangle();
        coordinateConnectionPointVM.SetCoordinate();
    }

    public override void SetHeight(double height)
    {
        Height = height;
        HeightBorder = height;
        TextFieldSymbolVM.Height = height;

        SetCoordinatePolygonPoints();
        ChangeCoordinateScaleRectangle();
        coordinateConnectionPointVM.SetCoordinate();
    }

    public void SetCoordinatePolygonPoints()
    {
        Points = new()
        {
            new Point(0, 0),
            new Point(0, Height),
            new Point(Width, Height),
            new Point(Width, 0)
        };
    }

    private void AddConnectionPoints()
    {
        var builderConnectionPointsVM = new BuilderConnectionPointsVM(
            CanvasSymbolsVM,
            this,
            _checkBoxLineGostVM);

        ConnectionPointsVM = builderConnectionPointsVM
            .AddTopConnectionPoint()
            .AddRightConnectionPoint()
            .AddBottomConnectionPoint()
            .AddLeftConnectionPoint()
            .Build();
    }

    private void AddScaleRectangles()
    {
        var builderScaleRectangles = new BuilderScaleRectangles(
            CanvasSymbolsVM,
           _scaleAllSymbolVM,
           this);

        ScaleRectangles =
            builderScaleRectangles
                        .AddMiddleTopRectangle()
                        .AddRightTopRectangle()
                        .AddRightMiddleRectangle()
                        .AddRightBottomRectangle()
                        .AddMiddleBottomRectangle()
                        .AddLeftBottomRectangle()
                        .AddLeftMiddleRectangle()
                        .AddLeftTopRectangle()
                        .Build();
    }
}