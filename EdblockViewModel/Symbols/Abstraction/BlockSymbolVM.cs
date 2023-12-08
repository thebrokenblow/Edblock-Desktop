﻿using System;
using Prism.Commands;
using System.Windows.Input;
using EdblockModel.Symbols;
using System.Collections.Generic;
using EdblockModel.Symbols.Abstraction;
using EdblockViewModel.Symbols.ScaleRectangles;
using EdblockViewModel.Symbols.ConnectionPoints;

namespace EdblockViewModel.Symbols.Abstraction;

public abstract class BlockSymbolVM : SymbolVM
{
    public List<ConnectionPoint> ConnectionPoints { get; init; }
    public List<ScaleRectangle> ScaleRectangles { get; init; }

    private int width;
    public int Width
    {
        get => width;
        set
        {
            width = value;
            BlockSymbolModel.Width = width;

            OnPropertyChanged();
        }
    }

    private int heigth;
    public int Height
    {
        get => heigth;
        set
        {
            heigth = value;
            BlockSymbolModel.Height = heigth;

            OnPropertyChanged();
        }
    }

    private int xCoordinate; 
    public int XCoordinate
    {
        get => xCoordinate;
        set
        {
            xCoordinate = value;
            BlockSymbolModel.XCoordinate = xCoordinate;

            OnPropertyChanged();
        }
    }

    private int yCoordinate;
    public int YCoordinate
    {
        get => yCoordinate;
        set
        {
            yCoordinate = value;
            BlockSymbolModel.YCoordinate = yCoordinate;

            OnPropertyChanged();
        }
    }

    public TextField TextField { get; init; }
    public BlockSymbolModel BlockSymbolModel { get; init; }
    public DelegateCommand EnterCursor { get; set; }
    public DelegateCommand LeaveCursor { get; set; }

    private readonly CanvasSymbolsVM _canvasSymbolsVM;
    private readonly FactoryBlockSymbolModel _factoryBlockSymbolModel = new();

    private readonly string _id;

    protected const int defaultWidth = 140;
    protected const int defaultHeigth = 60;
    public BlockSymbolVM(CanvasSymbolsVM canvasSymbolsVM)
    {
        _canvasSymbolsVM = canvasSymbolsVM;

        _id = Guid.NewGuid().ToString();
        var nameBlockSymbol = GetType().Name?.ToString();

        BlockSymbolModel = _factoryBlockSymbolModel.Create(nameBlockSymbol, _id);

        TextField = new(canvasSymbolsVM, BlockSymbolModel);

        Width = defaultWidth;
        Height = defaultHeigth;
        Color = BlockSymbolModel.HexColor;

        var factoryConnectionPoints = new FactoryConnectionPoints(_canvasSymbolsVM, this);
        ConnectionPoints = factoryConnectionPoints.Create();

        var factoryScaleRectangles = new FactoryScaleRectangles(_canvasSymbolsVM, this);
        ScaleRectangles = factoryScaleRectangles.Create();

        EnterCursor = new(ShowStroke);
        LeaveCursor = new(HideStroke);
    }

    public abstract void SetWidth(int width);
    public abstract void SetHeight(int height);

    public void SetCoordinate((int x, int y) currentCoordinate, (int x, int y) previousCoordinate)
    {
        int roundedXCoordinate = CanvasSymbolsVM.RoundCoordinate(currentCoordinate.x);
        int roundedYCoordinate = CanvasSymbolsVM.RoundCoordinate(currentCoordinate.y);

        if (XCoordinate == 0 && YCoordinate == 0)
        {
            XCoordinate = roundedXCoordinate - Width / 2;
            YCoordinate = roundedYCoordinate - Height / 2;
        }
        else
        {
            XCoordinate = roundedXCoordinate - (previousCoordinate.x - XCoordinate);
            YCoordinate = roundedYCoordinate - (previousCoordinate.y - YCoordinate);
        }
    }

    public void ShowStroke()
    {
        // Условие истино, когда символ не перемещается и не масштабируется (просто навёл курсор)

        var movableSymbol = _canvasSymbolsVM.MovableSymbol;
        var scalePartBlockSymbolVM = _canvasSymbolsVM.ScalePartBlockSymbolVM;

        if (movableSymbol == null && scalePartBlockSymbolVM == null)
        {
            ConnectionPoint.SetDisplayConnectionPoints(ConnectionPoints, true);
            ScaleRectangle.SetStateDisplay(ScaleRectangles, true);
            TextField.Cursor = Cursors.SizeAll;
        }
    }

    public void HideStroke()
    {
        ConnectionPoint.SetDisplayConnectionPoints(ConnectionPoints, false);
        ScaleRectangle.SetStateDisplay(ScaleRectangles, false);
    }

    protected void ChangeCoordinateAuxiliaryElements()
    {
        foreach (var connectionPoint in ConnectionPoints)
        {
            connectionPoint.ChangeCoordination();
        }

        foreach (var scaleRectangle in ScaleRectangles)
        {
            scaleRectangle.ChangeCoordination();
        }
    }
}