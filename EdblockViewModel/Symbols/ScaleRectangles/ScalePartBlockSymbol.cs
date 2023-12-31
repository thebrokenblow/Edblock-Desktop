﻿using System;
using System.Windows.Input;
using EdblockViewModel.ComponentsVM;
using EdblockViewModel.Symbols.Abstraction;
using System.Collections.ObjectModel;

namespace EdblockViewModel.Symbols.ScaleRectangles;

public class ScalePartBlockSymbol
{
    public BlockSymbolVM ScalingBlockSymbol { get; init; }
    public double InitialWidthBlockSymbol { get; init; }
    public double InitialHeigthBlockSymbol { get; init; }
    public double InitialXCoordinateBlockSymbol { get; init; }
    public double InitialYCoordinateBlockSymbol { get; init; }

    private readonly ScaleAllSymbolVM _scaleAllSymbolVM;
    private readonly Cursor _cursorWhenScaling;
    private readonly ObservableCollection<SymbolVM> _symbols;
    private readonly Func<ScalePartBlockSymbol, CanvasSymbolsVM, double>? _getWidthBlockSymbol;
    private readonly Func<ScalePartBlockSymbol, CanvasSymbolsVM, double>? _getHeigthBlockSymbol;

    public ScalePartBlockSymbol(
        BlockSymbolVM scalingBlockSymbol,
        Cursor cursorWhenScaling,
        Func<ScalePartBlockSymbol, CanvasSymbolsVM, double>? getWidthBlockSymbol,
        Func<ScalePartBlockSymbol, CanvasSymbolsVM, double>? getHeigthBlockSymbol,
        ScaleAllSymbolVM scaleAllSymbolVM,
        ObservableCollection<SymbolVM> symbolsVM)
    {
        ScalingBlockSymbol = scalingBlockSymbol;
        InitialWidthBlockSymbol = scalingBlockSymbol.Width;
        InitialHeigthBlockSymbol = scalingBlockSymbol.Height;
        InitialXCoordinateBlockSymbol = scalingBlockSymbol.XCoordinate;
        InitialYCoordinateBlockSymbol = scalingBlockSymbol.YCoordinate;
        _cursorWhenScaling = cursorWhenScaling;
        _getWidthBlockSymbol = getWidthBlockSymbol; 
        _getHeigthBlockSymbol = getHeigthBlockSymbol;
        _scaleAllSymbolVM = scaleAllSymbolVM;
        _symbols = symbolsVM;
    }

    public void SetWidthBlockSymbol(CanvasSymbolsVM canvasSymbolsVM)
    {
        canvasSymbolsVM.SetCurrentRedrawLines(ScalingBlockSymbol);

        if (_getWidthBlockSymbol == null)
        {
            return;
        }

        double width = _getWidthBlockSymbol.Invoke(this, canvasSymbolsVM);

        if (_scaleAllSymbolVM.IsScaleAllSymbolVM)
        {
            foreach (var symbol in _symbols)
            {
                if (symbol is BlockSymbolVM blockSymbolVM)
                {
                    blockSymbolVM.SetWidth(width);
                }
            }
        }
        else
        {
            ScalingBlockSymbol.SetWidth(width);
        }

        ScalingBlockSymbol.TextFieldVM.Cursor = _cursorWhenScaling;
        canvasSymbolsVM.Cursor = _cursorWhenScaling;
    }

    public void SetHeightBlockSymbol(CanvasSymbolsVM canvasSymbolsVM)
    {
        canvasSymbolsVM.SetCurrentRedrawLines(ScalingBlockSymbol);

        if (_getHeigthBlockSymbol == null)
        {
            return;
        }

        double height = _getHeigthBlockSymbol.Invoke(this, canvasSymbolsVM);

        if (_scaleAllSymbolVM.IsScaleAllSymbolVM)
        {
            foreach (var symbol in _symbols)
            {
                if (symbol is BlockSymbolVM blockSymbolVM)
                {
                    blockSymbolVM.SetHeight(height);
                }
            }
        }
        else
        {
            ScalingBlockSymbol.SetHeight(height);
        }

        ScalingBlockSymbol.TextFieldVM.Cursor = _cursorWhenScaling;
        canvasSymbolsVM.Cursor = _cursorWhenScaling;
    }
}