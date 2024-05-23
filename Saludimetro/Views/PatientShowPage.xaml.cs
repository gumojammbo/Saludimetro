using Saludimetro.ViewModels;

namespace Saludimetro.Views;

public partial class PatientShowPage : TabbedPage
{
    public PatientShowPage(PatientShowModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}