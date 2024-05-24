using Saludimetro.ViewModels;


namespace Saludimetro.Views;

public partial class PatientListPage : ContentPage
{
    public PatientListPage(PatientListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

    }
}
