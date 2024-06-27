﻿using System;
using System.Windows.Input;
using EdblockModel.SymbolsModel;
using EdblockViewModel.Core;
using EdblockViewModel.Symbols.ComponentsSymbolsVM.ScaleRectangles;
using EdblockViewModel.Symbols.ComponentsSymbolsVM.ConnectionPoints;
using EdblockViewModel.Components.CanvasSymbols.Interfaces;
using EdblockViewModel.Components.TopSettingsMenu.Interfaces;
using EdblockViewModel.Components.TopSettingsMenu.PopupBoxMenu.Interfaces;
using Prism.Commands;

namespace EdblockViewModel.Symbols.Abstractions;

public abstract class BlockSymbolVM : ObservableObject
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    protected string color = string.Empty;
    public virtual string Color
    {
        get => color;
        set
        {
            color = value;
            BlockSymbolModel.Color = color;

            OnPropertyChanged();
        }
    }

    protected double width;
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

    protected double height;
    public double Height
    {
        get => height;
        set
        {
            height = value;
            BlockSymbolModel.Height = height;

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

    public DelegateCommand MouseEnter { get; set; }
    public DelegateCommand MouseLeave { get; set; }
    public DelegateCommand MouseLeftButtonDown { get; set; }
    public BlockSymbolModel BlockSymbolModel { get; } = null!;

    protected readonly IScaleAllSymbolComponentVM scaleAllSymbolComponentVM;
    protected readonly ILineStateStandardComponentVM lineStateStandardComponentVM;
    protected readonly ICanvasSymbolsComponentVM _canvasSymbolsComponentVM;

    private readonly IListCanvasSymbolsComponentVM _listCanvasSymbolsComponentVM;
    private readonly ITopSettingsMenuComponentVM _topSettingsMenuComponentVM;

    private readonly IHasConnectionPoint? _symbolHasConnectionPoint;
    private readonly IHasScaleRectangles? _symbolHasScaleRectangles;
    private readonly IHasTextFieldVM? _symbolHasTextField;

    public BlockSymbolVM(
        ICanvasSymbolsComponentVM canvasSymbolsComponentVM, 
        IListCanvasSymbolsComponentVM listCanvasSymbolsComponentVM,
        ITopSettingsMenuComponentVM topSettingsMenuComponentVM, 
        IPopupBoxMenuComponentVM popupBoxMenuComponentVM)
    {
        lineStateStandardComponentVM = popupBoxMenuComponentVM.LineStateStandardComponentVM;
        scaleAllSymbolComponentVM = popupBoxMenuComponentVM.ScaleAllSymbolComponentVM;

        _canvasSymbolsComponentVM = canvasSymbolsComponentVM;
        _topSettingsMenuComponentVM = topSettingsMenuComponentVM;
        _listCanvasSymbolsComponentVM = listCanvasSymbolsComponentVM;

        if (this is IHasConnectionPoint symbolHasConnectionPoint)
        {
            _symbolHasConnectionPoint = symbolHasConnectionPoint;
        }

        if (this is IHasScaleRectangles symbolHasScaleRectangles)
        {
            _symbolHasScaleRectangles = symbolHasScaleRectangles;
        }

        if (this is IHasTextFieldVM symbolHasTextField)
        {
            _symbolHasTextField = symbolHasTextField;
        }

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
        _canvasSymbolsComponentVM.Cursor = Cursors.SizeAll;

        var movableSymbol = _listCanvasSymbolsComponentVM.MovableBlockSymbol;
        var scalePartBlockSymbolVM = _canvasSymbolsComponentVM.ScalePartBlockSymbol;

        if (movableSymbol is not null || scalePartBlockSymbolVM is not null)
        {
            return;
        }

        SetDisplayScaleRectangles(true);
        SetDisplayConnectionPoints(true);

        if (this is IHasTextFieldVM symbolHasTextField)
        {
            symbolHasTextField.TextFieldSymbolVM.Cursor = Cursors.SizeAll;
        }
    }

    public void HideAuxiliaryElements()
    {
        SetDisplayScaleRectangles(false);
        SetDisplayConnectionPoints(false);

        _canvasSymbolsComponentVM.Cursor = Cursors.Arrow;
    }

    private void SetDisplayConnectionPoints(bool status)
    {
        if (_symbolHasConnectionPoint is null)
        {
            return;
        }

        ConnectionPointVM.SetDisplayConnectionPoints(_symbolHasConnectionPoint.ConnectionPointsVM, status);
    }

    private void SetDisplayScaleRectangles(bool status)
    {
        if (_symbolHasScaleRectangles is null)
        {
            return;
        }

        ScaleRectangle.SetStateDisplay(_symbolHasScaleRectangles.ScaleRectangles, status);
    }

    protected void ChangeCoordinateScaleRectangle()
    {
        if (_symbolHasScaleRectangles is null)
        {
            return;
        }

        foreach (var scaleRectangle in _symbolHasScaleRectangles.ScaleRectangles)
        {
            scaleRectangle.ChangeCoordination();
        }
    }

    public void SetMovableSymbol()
    {
        SetDisplayScaleRectangles(false);
        SetDisplayConnectionPoints(false);

        _canvasSymbolsComponentVM.Cursor = Cursors.SizeAll;

        _listCanvasSymbolsComponentVM.MovableBlockSymbol = this;
    }

    public void SetSelectedProperties()
    {
        IsSelected = true;

        if (_symbolHasTextField is null)
        {
            return;
        }

        _topSettingsMenuComponentVM.FontSizeComponentVM.SetFontSize(_symbolHasTextField);
        _topSettingsMenuComponentVM.FontFamilyComponentVM.SetFontFamily(_symbolHasTextField);
        _topSettingsMenuComponentVM.FormatTextComponentVM.SetFontText(_symbolHasTextField);
        _topSettingsMenuComponentVM.TextAlignmentComponentVM.SetFormatAlignment(_symbolHasTextField);
        _topSettingsMenuComponentVM.FormatVerticalAlignComponentVM.SetFormatVerticalAlignment(_symbolHasTextField);

        _listCanvasSymbolsComponentVM.SelectedBlockSymbols.Add(this);
        _listCanvasSymbolsComponentVM.SelectedSymbolsHasTextField.Add(_symbolHasTextField);
    }

    public virtual void MiddleTop(double height)
    {
        SetHeight(height);
    }


    public virtual void RightTop(double width, double height)
    {
        SetWidth(width);
        SetHeight(height);
    }

    public virtual void RightMiddle(double width)
    {
        SetWidth(width);
    }

    public virtual void RightBottom(double width, double height)
    {
        SetWidth(width);
        SetHeight(height);
    }

    public virtual void MiddleBottom(double height)
    {
        SetHeight(height);
    }

    public virtual void LeftBottom(double width, double height)
    {
        SetWidth(width);
        SetHeight(height);
    }

    public virtual void LeftMiddle(double width)
    {
        SetWidth(width);
    }

    public virtual void LeftTop(double width, double height)
    {
        SetWidth(width);
        SetHeight(height);
    }
}