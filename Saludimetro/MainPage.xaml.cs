using Saludimetro.ViewModels;
using Saludimetro.Views;

namespace Saludimetro;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage(PatientListViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = viewModel;
    }

}


