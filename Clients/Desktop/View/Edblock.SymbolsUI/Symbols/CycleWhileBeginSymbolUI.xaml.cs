﻿using Edblock.SymbolsUI.Factories;
using EdblockViewModel.Abstractions;
using EdblockViewModel.Pages;
using EdblockViewModel.Symbols;
using System.Windows.Controls;

namespace Edblock.SymbolsUI.Symbols;

/// <summary>
/// Логика взаимодействия для CycleWhileBeginSymbolUI.xaml
/// </summary>
public partial class CycleWhileBeginSymbolUI : UserControl, IFactorySymbolViewModel
{
    public CycleWhileBeginSymbolUI() =>
        InitializeComponent();

    public BlockSymbolVM CreateBlockSymbolViewModel(EditorVM editorVM) =>
        new CycleWhileBeginSymbolVM(editorVM);
}