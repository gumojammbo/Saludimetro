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
        private bool isLoadingVisible = true;

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

            IsLoadingVisible = false;
        }
    }
}

