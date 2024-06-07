﻿using System.Collections.Generic;
using EdblockViewModel.Components.CanvasSymbols.Interfaces;
using EdblockViewModel.Components.TopSettingsMenu.Interfaces;
using EdblockViewModel.Symbols.Abstractions;

namespace EdblockViewModel.Components.TopSettingsMenu;

public class FontFamilyComponentVM(IListCanvasSymbolsComponentVM listCanvasSymbolsVM) : IFontFamilyComponentVM
{
    public List<string> FontFamilys { get; } =
    [
        "Times New Roman"
    ];

    public string? fontFamily;
    public string? FontFamily
    {
        get => fontFamily;
        set
        {
            fontFamily = value;
            SetFontFamily();
        }
    }

    public void SetFontFamily(IHasTextFieldVM selectedSymbolHasTextField)
    {
        selectedSymbolHasTextField.TextFieldSymbolVM.FontFamily = fontFamily;
    }

    private void SetFontFamily()
    {
        foreach (var selectedSymbolHasTextField in listCanvasSymbolsVM.SelectedSymbolsHasTextField)
        {
            SetFontFamily(selectedSymbolHasTextField);
        }
    }
}