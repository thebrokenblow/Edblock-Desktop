﻿using Edblock.SymbolsUI.Factories;
using EdblockViewModel.AbstractionsVM;
using EdblockViewModel.PagesVM;
using EdblockViewModel.Symbols;
using System.Windows.Controls;

namespace Edblock.SymbolsUI.Symbols;

/// <summary>
/// Логика взаимодействия для InputOutputSymbolUI.xaml
/// </summary>
public partial class InputOutputSymbolUI : UserControl, IFactorySymbolViewModel
{
    public InputOutputSymbolUI() =>
        InitializeComponent();

    public BlockSymbolVM CreateBlockSymbolViewModel(EditorVM editorVM) =>
        new InputOutputSymbolVM(editorVM);
}