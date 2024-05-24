using System;
using System.ComponentModel.DataAnnotations;

namespace Saludimetro.Models
{
	public class Patient
	{
        [Key]
        public int PatientID { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public float Weight { get; set; }

        public float Height { get; set; }

        public string Sex { get; set; }

        public string ActivityLevel { get; set; }
    }
}

