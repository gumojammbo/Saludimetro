using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;

using Microsoft.EntityFrameworkCore;
using Saludimetro.DataAccess;
using Saludimetro.DTOs;
using Saludimetro.Utilities;
using Saludimetro.Models;

namespace Saludimetro.ViewModels
{
    public partial class PatientShowModel : ObservableObject, IQueryAttributable
    {
        private readonly PatientDbContext _dbContext;

        [ObservableProperty]
        private PatientDTO patientDto = new PatientDTO();

        [ObservableProperty]
        private int patientID;

        [ObservableProperty]
        private string pageTitle;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string lastName;

        [ObservableProperty]
        private string activityLevel;

        [ObservableProperty]
        private int age;

        [ObservableProperty]
        private float weight;

        [ObservableProperty]
        private bool isLoadingVisible = true;

        [ObservableProperty]
        private double bmi;

        [ObservableProperty]
        private string sex;

        [ObservableProperty]
        private double bodyfat;

        [ObservableProperty]
        private double idealweight;

        [ObservableProperty]
        private double calorieintake;

        public PatientShowModel(PatientDbContext context)
        {
            _dbContext = context;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id = int.Parse(query["id"].ToString());
            PatientID = id;

            IsLoadingVisible = true;

            var found = await _dbContext.Patients.FirstAsync(p => p.PatientID == PatientID);

            PatientDto.PatientID = found.PatientID;
            PatientDto.Name = found.Name;
            PatientDto.LastName = found.LastName;
            PatientDto.Age = found.Age;
            PatientDto.Height = found.Height;
            PatientDto.Weight = found.Weight;
            PatientDto.Sex = found.Sex;
            PatientDto.ActivityLevel = found.ActivityLevel;

            PageTitle = $"{found.Name} {found.LastName}";
            Name = found.Name;
            LastName = found.LastName;
            Weight = found.Weight;
            ActivityLevel = found.ActivityLevel;
            Age = found.Age;
            Sex = found.Sex;


            CalculateMetrics();
            IsLoadingVisible = false;
        }

        private void CalculateMetrics()
        {
            Bmi = CalculateBMI();
            Bodyfat = CalculateBodyFat();
            Idealweight = CalculateIdealWeight();
            Calorieintake = CalculateDailyCalorieIntake();

        }

        private double CalculateBMI()
        {
            if (PatientDto.Height > 0)
            {
                return PatientDto.Weight / (PatientDto.Height * PatientDto.Height);
            }
            return 0;
        }

        private double CalculateBodyFat()
        {
            int sexCalculation;

            if(PatientDto.Sex.Equals("Masculino"))
            {
                sexCalculation = 1;
            }
            else
            {
                sexCalculation = 0;
            }

            return 1.2 * Bmi + 0.23 * PatientDto.Age - 10.8 * (sexCalculation) - 5.4;
        }

        private double CalculateIdealWeight()
        {
            double sexCalculation;
            double heightInCm = PatientDto.Height * 100;
            Console.WriteLine(heightInCm);

            if (PatientDto.Sex.Equals("Masculino"))
            {
                sexCalculation = 4;
            }
            else
            {
                sexCalculation = 2.5;
            }

            return heightInCm - 100 - ((heightInCm - 150) / sexCalculation);
        }

        private double CalculateDailyCalorieIntake()
        {
            double activityEquivalence =  0;
            double heightInCm = PatientDto.Height * 100;


            switch (PatientDto.activityLevel)
            {
                case "Rara vez":
                    activityEquivalence = 1.200;
                    break;
                case "1 a 3 días por semana":
                    activityEquivalence = 1.375;
                    break;
                case "3 a 5 días por semana":
                    activityEquivalence = 1.550;
                    break;
                case "6 a 7 días por semana":
                    activityEquivalence = 1.725;
                    break;
                case "Diariamente o dos veces al día":
                    activityEquivalence = 1.900;
                    break;
            }


            double bmr = 0;
            if(PatientDto.Sex.Equals("Masculino"))
            {
                bmr = (heightInCm * 6.25) + (PatientDto.Weight * 9.99) - (PatientDto.Age * 4.92) + 5;
            }
            else
            {
                bmr = (heightInCm * 6.25) + (PatientDto.Weight * 9.99) - (PatientDto.Age * 4.92) - 161;
            }

            return bmr * activityEquivalence;

        }

        [ObservableProperty]
        private Color bmiColor;

        partial void OnBmiChanged(double oldValue, double newValue)
        {
            BmiColor = GetBmiColor(newValue);
            BmiDescription = GetBmiDescription(newValue);
        }

        private Color GetBmiColor(double bmi)
        {
            if (bmi < 18.5)
                return new Color(115, 165, 168); // Underweight
            else if (bmi < 24.9)
                return new Color(35, 64, 142); // Normal weight
            else if (bmi < 29.9)
                return new Color(230, 208, 33); // Overweight
            else if (bmi < 34.9)
                return new Color(226, 158, 40); // Obesity Class I
            else if (bmi < 39.9)
                return new Color(215, 87, 59); // Obesity Class II
            else if (bmi > 40)
                return new Color(165, 106, 66);  // Obesity Class III
            else
                return Colors.Red; // ???
        }

        [ObservableProperty]
        private string bmiDescription;

        private string GetBmiDescription(double bmi)
        {
            if (bmi < 18.5)
                return "Bajo peso"; // Underweight
            else if (bmi < 24.9)
                return "Peso normal"; // Normal weight
            else if (bmi < 29.9)
                return "Pre-obesidad o sobrepeso"; // Overweight
            else if (bmi < 34.9)
                return "Obesidad clase I"; // Obesity Class I
            else if (bmi < 39.9)
                return "Obesidad clase II"; // Obesity Class II
            else if (bmi > 40)
                return "Obesidad clase III"; // Obesity Class III
            else
                return ""; // ???
        }


        [ObservableProperty]
        private Color bodyFatColor;

        partial void OnBodyfatChanged(double oldValue, double newValue)
        {
            BodyFatColor = GetBodyFatColor(newValue);
            BodyFatDescription = GetBodyFatDescription(newValue);
        }


        private Color GetBodyFatColor(double bodyfat)
        {
            if(Sex.Equals("Femenino"))
            {
                if (bodyfat < 13.99)
                    return new Color(115, 165, 168); // Underweight
                else if (bodyfat < 20.99)
                    return new Color(35, 64, 142); // Normal weight
                else if (bodyfat < 24.99)
                    return new Color(230, 208, 33); // Overweight
                else if (bodyfat < 31.99)
                    return new Color(226, 158, 40); // Obesity Class I
                else if (bodyfat > 32)
                    return new Color(215, 87, 59); // Obesity Class II
                else
                    return Colors.Red; // ???
            }
            else
            {
                if (bodyfat < 5)
                    return new Color(115, 165, 168); // Underweight
                else if (bodyfat < 13)
                    return new Color(35, 64, 142); // Normal weight
                else if (bodyfat < 17)
                    return new Color(230, 208, 33); // Overweight
                else if (bodyfat < 24.99)
                    return new Color(226, 158, 40); // Obesity Class I
                else if (bodyfat > 25)
                    return new Color(215, 87, 59); // Obesity Class II
                else
                    return Colors.Red; // ???
            }
            
        }

        [ObservableProperty]
        private string bodyFatDescription;

        private string GetBodyFatDescription(double bodyfat)
        {
            if (Sex.Equals("Femenino"))
            {
                if (bodyfat < 13.99)
                    return "Grasa escencial"; // Underweight
                else if (bodyfat < 20.99)
                    return "Atletas"; // Normal weight
                else if (bodyfat < 24.99)
                    return "Fitness"; // Overweight
                else if (bodyfat < 31.99)
                    return "Aceptable"; // Obesity Class I
                else if (bodyfat > 32)
                    return "Obesidad"; // Obesity Class II
                else
                    return ""; // ???
            }
            else
            {
                if (bodyfat < 5)
                    return "Grasa escencial"; // Underweight
                else if (bodyfat < 13)
                    return "Atletas"; // Normal weight
                else if (bodyfat < 17)
                    return "Fitness"; // Overweight
                else if (bodyfat < 24.99)
                    return "Aceptable"; // Obesity Class I
                else if (bodyfat > 25)
                    return "Obesidad"; // Obesity Class II
                else
                    return ""; // ???
            }
        }

    }
}

