using Saludimetro.ViewModels;

namespace Saludimetro.Views;

public partial class PatientShowPage : ContentPage
{
	public PatientShowPage(PatientShowModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
