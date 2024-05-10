﻿using Edblock.PagesViewModel.Core;
using Edblock.PagesViewModel.Service;

namespace Edblock.PagesViewModel.Pages;

public class EditorViewModel(INavigationService navigationService) : BaseViewModel
{
    public RelayCommand NavigateToBack { get; } =
        FactoryNavigateService<RegistrationViewModel>.Create(navigationService, true);

    public void LoadProject(string filePath)
    {
    }

    public void SaveProject(string filePath)
    {

    }
}