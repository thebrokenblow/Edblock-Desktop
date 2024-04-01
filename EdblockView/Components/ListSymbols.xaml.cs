﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using EdblockView.Abstractions;
using EdblockViewModel.ComponentsVM;

namespace EdblockView.Components;

/// <summary>
/// Логика взаимодействия для ListSymbols.xaml
/// </summary>
public partial class ListSymbols : UserControl
{
    private ListSymbolsVM? listSymbolsVM;
    public ListSymbols() =>
        InitializeComponent();

    private void AddSymbolView(object sender, MouseButtonEventArgs e)
    {
        if (sender is IFactorySymbolVM factorySymbolVM)
        {
            listSymbolsVM ??= (ListSymbolsVM)DataContext;

            try
            {
                if (listSymbolsVM is null)
                {
                    return;
                }

                var blockSymbolVM = factorySymbolVM.CreateBlockSymbolVM(listSymbolsVM.EditorVM);
                listSymbolsVM.AddBlockSymbol(blockSymbolVM);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}