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

        public char Sex { get; set; }

        public int ActivityLevel { get; set; }
    }
}

