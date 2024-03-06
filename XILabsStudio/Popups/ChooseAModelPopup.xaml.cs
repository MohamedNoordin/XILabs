using CommunityToolkit.Maui.Views;
using Microsoft.Maui;
using System.Diagnostics;
using XILabsStudio.API.DataModels;

namespace XILabsStudio.Popups;

public partial class ChooseAModelPopup : Popup
{
	public ChooseAModelPopup()
	{
		InitializeComponent();
	}

    private void ModelSelected(object sender, SelectedItemChangedEventArgs e)
    {
		ResultWhenUserTapsOutsideOfPopup = e.SelectedItem as Model;
    }

    private void ModelTapped(object sender, ItemTappedEventArgs e)
    {
        var model = e.Item as Model;
        ResultWhenUserTapsOutsideOfPopup = model;
        Close(model);
    }
}