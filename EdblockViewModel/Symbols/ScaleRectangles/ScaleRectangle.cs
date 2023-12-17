﻿using System;
using Prism.Commands;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EdblockViewModel.Symbols.Abstraction;
using EdblockViewModel.ComponentsVM;

namespace EdblockViewModel.Symbols.ScaleRectangles;

public class ScaleRectangle : INotifyPropertyChanged
{

    private int xCoordinate;
    public int XCoordinate
    {
        get => xCoordinate;
        set
        {
            xCoordinate = value;
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
            OnPropertyChanged();
        }
    }

    private bool isShow = false;
    public bool IsShow
    {
        get => isShow;
        set
        {
            isShow = value;
            OnPropertyChanged();
        }
    }

    public DelegateCommand EnterCursor { get; init; }
    public DelegateCommand LeaveCursor { get; init; }
    public DelegateCommand ClickScaleRectangle { get; init; }
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly CanvasSymbolsVM _canvasSymbolsVM;
    private readonly ScaleAllSymbolVM _scaleAllSymbolVM;
    private readonly BlockSymbolVM _blockSymbolVM;
    private readonly Cursor _cursorScaling;
    private readonly Func<(int, int)> _getCoordinateScaleRectangle;
    private readonly Func<ScalePartBlockSymbol, CanvasSymbolsVM, int>? _getWidthSymbol;
    private readonly Func<ScalePartBlockSymbol, CanvasSymbolsVM, int>? _getHeightSymbol;

    public ScaleRectangle(
        CanvasSymbolsVM canvasSymbolsVM,
        ScaleAllSymbolVM scaleAllSymbolVM,
        BlockSymbolVM blockSymbolVM,
        Cursor cursorScaling,
        Func<ScalePartBlockSymbol, CanvasSymbolsVM, int>? getWidthSymbol,
        Func<ScalePartBlockSymbol, CanvasSymbolsVM, int>? getHeightSymbol,
        Func<(int, int)> getCoordinateScaleRectangle
        )
    {
        _blockSymbolVM = blockSymbolVM;
        _cursorScaling = cursorScaling;
        _canvasSymbolsVM = canvasSymbolsVM;
        _scaleAllSymbolVM = scaleAllSymbolVM;
        _getWidthSymbol = getWidthSymbol;
        _getHeightSymbol = getHeightSymbol;

        _getCoordinateScaleRectangle = getCoordinateScaleRectangle;
        (XCoordinate, YCoordinate) = getCoordinateScaleRectangle.Invoke();

        EnterCursor = new(ShowScaleRectangles);
        LeaveCursor = new(HideScaleRectangles);
        ClickScaleRectangle = new(SaveScaleRectangle);
    }

    public void ChangeCoordination()
    {
        (XCoordinate, YCoordinate) = _getCoordinateScaleRectangle.Invoke();
    }

    public static void SetStateDisplay(List<ScaleRectangle> scaleRectangles, bool isShowScaleRectangle)
    {
        foreach (var scaleRectangle in scaleRectangles)
        {
            scaleRectangle.IsShow = isShowScaleRectangle;
        }
    }

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    private void ShowScaleRectangles()
    {
        if (_canvasSymbolsVM.ScalePartBlockSymbol == null)
        {
            _canvasSymbolsVM.Cursor = _cursorScaling;
            SetStateDisplay(_blockSymbolVM.ScaleRectangles, true);
        }
    }

    private void HideScaleRectangles()
    {
        _canvasSymbolsVM.Cursor = Cursors.Arrow;
        SetStateDisplay(_blockSymbolVM.ScaleRectangles, false);
    }

    private void SaveScaleRectangle()
    {
        _canvasSymbolsVM.ScalePartBlockSymbol = new(_blockSymbolVM, _cursorScaling, _getWidthSymbol, _getHeightSymbol, _scaleAllSymbolVM, _canvasSymbolsVM.SymbolsVM);
    }
}