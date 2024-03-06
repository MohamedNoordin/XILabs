using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XILabsStudio.API;
using XILabsStudio.API.DataModels;

namespace XILabsStudio.ViewModels
{
    [ObservableObject]
    public partial class ChooseAModelViewModel
    {
        private XIOpenAPI xi;

        [ObservableProperty]
        private List<Model> models;

        [ObservableProperty]
        private Model selectedModel;

        public ChooseAModelViewModel()
        {
        }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            xi = await XIOpenAPI.InitializeAsync();
            Models = await xi.GetModelsAsync();
            SelectedModel = Models.FirstOrDefault();
        }
    }
}
