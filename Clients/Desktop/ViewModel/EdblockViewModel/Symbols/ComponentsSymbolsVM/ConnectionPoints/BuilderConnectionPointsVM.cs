﻿using System.Collections.Generic;
using EdblockModel.EnumsModel;
using EdblockViewModel.Components.CanvasSymbols.Interfaces;
using EdblockViewModel.Components.TopSettingsMenu.PopupBoxMenu.Interfaces;
using EdblockViewModel.Symbols.Abstractions;

namespace EdblockViewModel.Symbols.ComponentsSymbolsVM.ConnectionPoints;

public class BuilderConnectionPointsVM(ICanvasSymbolsComponentVM canvasSymbolsVM, BlockSymbolVM blockSymbolVM, ILineStateStandardComponentVM lineStateStandardComponentVM)
{
    private readonly FactoryConnectionPoint _factoryConnectionPoint = new(canvasSymbolsVM, lineStateStandardComponentVM, blockSymbolVM);
    private readonly List<ConnectionPointVM> connectionPointsVM = [];

    public BuilderConnectionPointsVM AddTopConnectionPoint()
    {
        var topConnectionPointVM = _factoryConnectionPoint.Create(SideSymbol.Top);

        connectionPointsVM.Add(topConnectionPointVM);

        return this;
    }

    public BuilderConnectionPointsVM AddRightConnectionPoint()
    {
        var rightConnectionPointVM = _factoryConnectionPoint.Create(SideSymbol.Right);

        connectionPointsVM.Add(rightConnectionPointVM);

        return this;
    }

    public BuilderConnectionPointsVM AddBottomConnectionPoint()
    {
        var bottomConnectionPointVM = _factoryConnectionPoint.Create(SideSymbol.Bottom);

        connectionPointsVM.Add(bottomConnectionPointVM);

        return this;
    }

    public BuilderConnectionPointsVM AddLeftConnectionPoint()
    {
        var leftConnectionPointVM = _factoryConnectionPoint.Create(SideSymbol.Left);

        connectionPointsVM.Add(leftConnectionPointVM);

        return this;
    }

    public List<ConnectionPointVM> Build() 
    {
        return connectionPointsVM;
    }
}