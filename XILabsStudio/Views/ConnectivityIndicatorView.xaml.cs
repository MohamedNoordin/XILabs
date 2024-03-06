namespace XILabsStudio.Views;

public partial class ConnectivityIndicatorView : ContentView
{
	public ConnectivityIndicatorView()
	{
		InitializeComponent();
	}

    private void NotifyNoInternetConnection(object sender, EventArgs e)
    {
		SemanticScreenReader.Default.Announce("No internet connection.");
    }
}