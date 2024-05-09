﻿using EdblockViewModel.PagesVM;

namespace Edblock.PagesViewModel.ComponentsViewModel;

public class PopupBoxMenuVM
{
    public EditorVM EdblockVM { get; set; }
    public ScaleAllSymbolVM ScaleAllSymbolVM { get; set; }
    public CheckBoxLineGostVM CheckBoxLineGostVM { get; set; }

    public PopupBoxMenuVM(EditorVM edblockVM)
    {
        EdblockVM = edblockVM;
        ScaleAllSymbolVM = new();
        CheckBoxLineGostVM = new();
    }
}