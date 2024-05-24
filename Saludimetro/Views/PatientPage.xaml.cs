using Saludimetro.ViewModels;

namespace Saludimetro.Views;


public partial class PatientPage : ContentPage
{
	public PatientPage(PatientViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}


}
