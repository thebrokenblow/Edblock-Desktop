﻿using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Commands;
using EdblockModel.EnumsModel;
using EdblockModel.SymbolsModel;
using Edblock.PagesViewModel.Pages;

namespace Edblock.SymbolsViewModel.Core;

public abstract class BlockSymbolViewModel : INotifyPropertyChanged
{
    private string id = string.Empty;
    public string Id
    {
        get => id;
        set
        {
            id = value;
        }
    }

    private string color = string.Empty;
    public string Color
    {
        get => color;
        set
        {
            color = value;
            BlockSymbolModel.Color = color;

            OnPropertyChanged();
        }
    }

    private double width;
    public double Width
    {
        get => width;
        set
        {
            width = value;
            BlockSymbolModel.Width = width;

            OnPropertyChanged();
        }
    }

    private double heigth;
    public double Height
    {
        get => heigth;
        set
        {
            heigth = value;
            BlockSymbolModel.Height = heigth;

            OnPropertyChanged();
        }
    }

    private double xCoordinate;
    public double XCoordinate
    {
        get => xCoordinate;
        set
        {
            xCoordinate = value;
            BlockSymbolModel.XCoordinate = xCoordinate;

            OnPropertyChanged();
        }
    }

    private double yCoordinate;
    public double YCoordinate
    {
        get => yCoordinate;
        set
        {
            yCoordinate = value;
            BlockSymbolModel.YCoordinate = yCoordinate;

            OnPropertyChanged();
        }
    }

    private bool isSelected;
    public bool IsSelected
    {
        get => isSelected;
        set
        {
            isSelected = value;
            OnPropertyChanged();
        }
    }

    public bool MoveMiddle { get; set; }

    public EditorViewModel EditorViewModel { get; init; }
    public DelegateCommand MouseEnter { get; set; }
    public DelegateCommand MouseLeave { get; set; }
    public DelegateCommand MouseLeftButtonDown { get; set; }
    public BlockSymbolModel BlockSymbolModel { get; init; } = null!;
    public CanvasSymbolsVM CanvasSymbolsVM { get; init; }

    protected readonly CheckBoxLineGostVM _checkBoxLineGostVM;
    protected readonly ScaleAllSymbolVM _scaleAllSymbolVM;

    private readonly FontFamilyControlVM _fontFamilyControlVM;
    private readonly FontSizeControlVM _fontSizeControlVM;
    private readonly TextAlignmentControlVM _textAlignmentControlVM;
    private readonly FormatTextControlVM _formatTextControlVM;

    public BlockSymbolViewModel(EditorViewModel editorViewModel)
    {
        EditorViewModel = editorViewModel;

        CanvasSymbolsVM = editorViewModel.CanvasSymbolsVM;

        _fontSizeControlVM = editorViewModel.FontSizeControlVM;
        _fontFamilyControlVM = editorViewModel.FontFamilyControlVM;
        _formatTextControlVM = editorViewModel.FormatTextControlVM;
        _textAlignmentControlVM = editorViewModel.TextAlignmentControlVM;
        _scaleAllSymbolVM = editorViewModel.PopupBoxMenuVM.ScaleAllSymbolVM;
        _checkBoxLineGostVM = editorViewModel.PopupBoxMenuVM.CheckBoxLineGostVM;

        Id = Guid.NewGuid().ToString();

        BlockSymbolModel = CreateBlockSymbolModel();

        MouseEnter = new(ShowAuxiliaryElements);
        MouseLeave = new(HideAuxiliaryElements);
        MouseLeftButtonDown = new(SetMovableSymbol);
    }

    public abstract void SetWidth(double width);
    public abstract void SetHeight(double height);

    public virtual BlockSymbolModel CreateBlockSymbolModel()
    {
        var nameSymbol = GetType().Name.ToString();

        var blockSymbolModel = new BlockSymbolModel()
        {
            Id = Id,
            NameSymbol = nameSymbol,
        };

        return blockSymbolModel;
    }

    public void SetCoordinate((int x, int y) currentCoordinate, (int x, int y) previousCoordinate)
    {
        var currentDrawnLineSymbol = CanvasSymbolsVM.CurrentDrawnLineSymbol;

        if (currentDrawnLineSymbol != null)
        {
            return;
        }

        CanvasSymbolsVM.RemoveSelectDrawnLine();

        if (MoveMiddle)
        {
            XCoordinate = currentCoordinate.x - Width / 2;
            YCoordinate = currentCoordinate.y - Height / 2;
        }
        else
        {
            if (previousCoordinate.x != 0 && previousCoordinate.y != 0)
            {
                XCoordinate = currentCoordinate.x - (previousCoordinate.x - XCoordinate);
                YCoordinate = currentCoordinate.y - (previousCoordinate.y - YCoordinate);
            }
        }
    }

    public void ShowAuxiliaryElements()
    {
        CanvasSymbolsVM.Cursor = Cursors.SizeAll;

        var movableSymbol = CanvasSymbolsVM.MovableBlockSymbol;
        var scalePartBlockSymbolVM = CanvasSymbolsVM.ScalePartBlockSymbol;

        if (movableSymbol is not null || scalePartBlockSymbolVM is not null)
        {
            return;
        }

        SetDisplayScaleRectangles(true);
        SetDisplayConnectionPoints(true);

        if (this is IHasTextField symbolHasTextField)
        {
            symbolHasTextField.TextFieldSymbolVM.Cursor = Cursors.SizeAll;
        }
    }

    public void HideAuxiliaryElements()
    {
        SetDisplayScaleRectangles(false);
        SetDisplayConnectionPoints(false);

        CanvasSymbolsVM.Cursor = Cursors.Arrow;
    }

    private void SetDisplayConnectionPoints(bool status)
    {
        if (this is IHasConnection symbolHasConnectionPoint)
        {
            var connectionPoints = symbolHasConnectionPoint.ConnectionPointsVM;

            ConnectionPointVM.SetDisplayConnectionPoints(connectionPoints, status);
        }
    }

    private void SetDisplayScaleRectangles(bool status)
    {
        if (this is IHasScaleRectangles symbolHasScaleRectangles)
        {
            var scaleRectangles = symbolHasScaleRectangles.ScaleRectangles;

            ScaleRectangle.SetStateDisplay(scaleRectangles, status);
        }
    }

    protected void ChangeCoordinateScaleRectangle()
    {
        if (this is IHasScaleRectangles symbolHasScaleRectangles)
        {
            var scaleRectangles = symbolHasScaleRectangles.ScaleRectangles;

            foreach (var scaleRectangle in scaleRectangles)
            {
                scaleRectangle.ChangeCoordination();
            }
        }
    }

    public void SetMovableSymbol()
    {
        SetDisplayScaleRectangles(false);
        SetDisplayConnectionPoints(false);

        CanvasSymbolsVM.Cursor = Cursors.SizeAll;

        CanvasSymbolsVM.MovableBlockSymbol = this;
    }

    public void Select()
    {
        IsSelected = true;

        _fontSizeControlVM.SetFontSize(this);
        _formatTextControlVM.SetFontText(this);
        _fontFamilyControlVM.SetFontFamily(this);
        _textAlignmentControlVM.SetFormatAlignment(this);

        CanvasSymbolsVM.SelectedBlockSymbols.Add(this);
    }

    internal ConnectionPointVM GetConnectionPoint(SideSymbol? incomingPosition)
    {
        if (this is SwitchCaseSymbolVM switchCaseSymbolVM)
        {
            var connectionPointsSwitchCaseVM = switchCaseSymbolVM.ConnectionPointsSwitchCaseVM;

            foreach (var connectionPointSwitchCaseVM in connectionPointsSwitchCaseVM)
            {
                if (!connectionPointSwitchCaseVM.IsHasConnectingLine)
                {
                    return connectionPointSwitchCaseVM;
                }
            }
        }

        if (this is IHasConnection blockSymbolHasConnectionPoint)
        {
            var connectionPoints = blockSymbolHasConnectionPoint.ConnectionPointsVM;

            foreach (var connectionPoint in connectionPoints)
            {
                if (connectionPoint.Position == incomingPosition && !connectionPoint.IsHasConnectingLine)
                {
                    return connectionPoint;
                }
            }
        }

        throw new Exception("Данный символ не содержит точек соединения");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}