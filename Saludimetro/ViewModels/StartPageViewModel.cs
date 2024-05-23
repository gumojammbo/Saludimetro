using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Saludimetro.ViewModels;

public partial class StartPageViewModel : ObservableObject
{
    [RelayCommand]
    private async Task Enter()
    {
        await Shell.Current.GoToAsync("//patientList");
    }
}
