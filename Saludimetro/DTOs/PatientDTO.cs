using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Saludimetro.DTOs
{
    public partial class PatientDTO : ObservableValidator
    {
        [ObservableProperty]
        public int patientID;

        [ObservableProperty]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres.")]
        [MaxLength(32, ErrorMessage = "El nombre no puede tener más de 32 caracteres.")]
        public string name;

        [ObservableProperty]
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [MinLength(2, ErrorMessage = "El apellido debe tener al menos 2 caracteres.")]
        [MaxLength(64, ErrorMessage = "El apellido no puede tener más de 64 caracteres.")]
        public string lastName;

        [ObservableProperty]
        [Range(0, 120, ErrorMessage = "La edad debe estar entre 0 y 120 años.")]
        public int age;

        [ObservableProperty]
        [Range(1, 999, ErrorMessage = "El peso debe estar entre 1 y 999 kg.")]

        public float weight;

        [ObservableProperty]
        [Range(0.5, 3.0, ErrorMessage = "La altura debe estar entre 0.5 y 3.0 metros.")]
        public float height;

        [ObservableProperty]
        [Required(ErrorMessage = "El sexo es obligatorio.")]
        [RegularExpression("Masculino|Femenino", ErrorMessage = "El sexo debe ser Masculino o Femenino")]
        public string sex;

        [ObservableProperty]
        [Required(ErrorMessage = "Debes elegir un nivel de actividad")]
        public string activityLevel;

        public void Validate()
        {
            ValidateAllProperties();
        }


    }
}

