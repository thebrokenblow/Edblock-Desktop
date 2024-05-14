﻿using Edblock.SymbolsUI.Factories;
using EdblockViewModel.AbstractionsVM;
using EdblockViewModel.PagesVM;
using EdblockViewModel.Symbols;
using System.Windows.Controls;

namespace Edblock.SymbolsUI.Symbols;

/// <summary>
/// Логика взаимодействия для ConditionSymbolUI.xaml
/// </summary>
public partial class ConditionSymbolUI : UserControl, IFactorySymbolViewModel
{
    public ConditionSymbolUI() =>
         InitializeComponent();

    public BlockSymbolVM CreateBlockSymbolViewModel(EditorVM editorVM) =>
        new ConditionSymbolVM(editorVM);
}