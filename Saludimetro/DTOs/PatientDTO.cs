using CommunityToolkit.Mvvm.ComponentModel;

namespace Saludimetro.DTOs
{
	public partial class PatientDTO : ObservableObject
	{
        [ObservableProperty]
        public int patientID;

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string lastName;

        [ObservableProperty]
        public int age;

        [ObservableProperty]
        public float weight;

        [ObservableProperty]
        public float height;

        [ObservableProperty]
        public char sex;

        [ObservableProperty]
        public int activityLevel;
    }
}

