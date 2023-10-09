﻿using EdblockModel;
using System.Windows;
using Prism.Commands;
using System.Windows.Input;
using System.ComponentModel;
using EdblockViewModel.Symbols;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using EdblockViewModel.Symbols.Abstraction;
using EdblockViewModel.Symbols.ScaleRectangles;
using EdblockViewModel.Symbols.ConnectionPoints;
using EdblockModel.Symbols.LineSymbols;
using EdblockViewModel.Symbols.LineSymbols;
using EdblockModel.Symbols.ConnectionPoints;

namespace EdblockViewModel;

public class CanvasSymbolsVM : INotifyPropertyChanged
{
    public Rect Grid { get; init; }

    private int x;
    public int X
    {
        get => x;
        set
        {
            CoordinateBlockSymbol.SetXCoordinate(DraggableSymbol, value, x);
            x = CanvasSymbols.СorrectionCoordinateSymbol(value);

            if (CurrentLines != null)
            {
                CurrentLines.ChangeCoordination(x, y);
            }

            if (ScaleData != null)
            {
                SizeBlockSymbol.SetSize(ScaleData, this, ScaleData?.GetWidthSymbol, ScaleData!.BlockSymbol.SetWidth);
                Cursor = ScaleData.Cursor;
                ScaleData.BlockSymbol.TextField.Cursor = ScaleData.Cursor;
            }
        }
    }

    private int y;
    public int Y
    {
        get => y;
        set
        {
            CoordinateBlockSymbol.SetYCoordinate(DraggableSymbol, value, y);
            y = CanvasSymbols.СorrectionCoordinateSymbol(value);

            if (CurrentLines != null)
            {
                CurrentLines.ChangeCoordination(x, y);
            }

            if (ScaleData != null)
            {
                SizeBlockSymbol.SetSize(ScaleData, this, ScaleData?.GetHeigthSymbol, ScaleData!.BlockSymbol.SetHeight);
                Cursor = ScaleData.Cursor;
                ScaleData.BlockSymbol.TextField.Cursor = ScaleData.Cursor;
            }
        }
    }

    private Cursor cursor = Cursors.Arrow;
    public Cursor Cursor
    {
        get => cursor;
        set
        {

            cursor = value;
            OnPropertyChanged();
        }
    }
   
    public ObservableCollection<Symbol> Symbols { get; init; }
    public DelegateCommand<string> ClickSymbol { get; init; }
    public DelegateCommand<BlockSymbol> MouseMoveSymbol { get; init; }
    public DelegateCommand MouseUpSymbol { get; init; }
    public DelegateCommand ClickCanvasSymbols { get; init; }
    public DelegateCommand ClickEsc { get; init; }
    public BlockSymbol? DraggableSymbol { get; set; }
    public ScaleData? ScaleData { get; set; }
    public ListLineSymbol? CurrentLines { get; set; }
    private readonly FactoryBlockSymbol factoryBlockSymbol;
    private readonly SerializableSymbols serializableSymbols = new();

    public CanvasSymbolsVM()
    {
        Symbols = new();
        ClickSymbol = new(CreateSymbol);
        MouseMoveSymbol = new(MoveSymbol);
        MouseUpSymbol = new(RemoveSymbol);
        ClickCanvasSymbols = new(ClickCanvas);
        ClickEsc = new(DeleteLine);
        factoryBlockSymbol = new(this);
        var lengthGrid = CanvasSymbols.LengthGrid;
        Grid = new Rect(-lengthGrid, -lengthGrid, lengthGrid, lengthGrid);
    }

    private void DeleteLine()
    {
        if (CurrentLines != null)
        {
            Symbols.Remove(CurrentLines);
            CurrentLines = null;
        }
    }

    private void CreateSymbol(string nameBlockSymbol)
    {
        var currentSymbol = factoryBlockSymbol.Create(nameBlockSymbol);

        currentSymbol.TextField.Cursor = Cursors.SizeAll;
        Cursor = Cursors.SizeAll;

        DraggableSymbol = currentSymbol;
        serializableSymbols.blocksSymbolModel.Add(currentSymbol.BlockSymbolModel);
        Symbols.Add(currentSymbol);
    }

    private void MoveSymbol(BlockSymbol currentSymbol)
    {
        if (!currentSymbol.TextField.Focus)
        {
            ColorConnectionPoint.Hide(currentSymbol.ConnectionPoints);
            ColorScaleRectangle.Hide(currentSymbol.ScaleRectangles);
            currentSymbol.TextField.Cursor = Cursors.SizeAll;
            Cursor = Cursors.SizeAll;
        }

        DraggableSymbol = currentSymbol;
    }

    private void RemoveSymbol()
    {
        DraggableSymbol = null;
        ScaleData = null;
        
        Cursor = Cursors.Arrow;
    }

    public void ClickCanvas()
    {
        if (CurrentLines != null && CurrentLines.LineSymbols.Count > 1)
        {
            var newLineSymbolModel = CurrentLines.LineSymbolModel.GetNewLine();
            var newLineSymbol = FactoryLineSymbol.CreateNewLine(newLineSymbolModel);

            CurrentLines.LineSymbols.Add(newLineSymbol);
        }
        TextField.ChangeFocus(Symbols);
    }

    public void DrawLine(LineSymbolModel lineSymbolModel, BlockSymbol blockSymbol, PositionConnectionPoint positionConnectionPoint)
    {
        var startLine = FactoryLineSymbol.CreateStartLine(lineSymbolModel);
       
        CurrentLines ??= new(lineSymbolModel, positionConnectionPoint);
        CurrentLines.SymbolOutgoingLine = blockSymbol;
        CurrentLines.LineSymbols.Add(startLine);

        Symbols.Add(CurrentLines);
        serializableSymbols.linesSymbolModel.Add(CurrentLines.LineSymbolModel);
    }


    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}