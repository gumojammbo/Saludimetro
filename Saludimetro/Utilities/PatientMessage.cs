using Saludimetro.DTOs;

namespace Saludimetro.Utilities
{
    public class PatientMessage
	{
		public bool IsCreate { get; set; }

		public PatientDTO PatientDto { get; set; }
	}
}

