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
	public partial class PatientViewModel : ObservableObject, IQueryAttributable
	{
        private readonly PatientDbContext _dbContext;

        [ObservableProperty]
        private PatientDTO patientDto = new PatientDTO();

        [ObservableProperty]
        private int patientID;

        [ObservableProperty]
        private string pageTitle;

        [ObservableProperty]
        private bool isLoadingVisible = false;

        [ObservableProperty]
        private string validationErrors;

        public PatientViewModel(PatientDbContext context)
        {
            _dbContext = context;
            //PatientDto.
        }


        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id = int.Parse(query["id"].ToString());
            PatientID = id;

            if(PatientID == 0)
            {
                PageTitle = "Nuevo paciente";
            }
            else
            {
                PageTitle = "Editar paciente";
                IsLoadingVisible = true;
                await Task.Run(async () =>
                {
                    var found = await _dbContext.Patients.FirstAsync(p => p.PatientID == PatientID);
                    PatientDto.PatientID = found.PatientID;
                    PatientDto.Name = found.Name;
                    PatientDto.LastName = found.LastName;
                    PatientDto.Age = found.Age;
                    PatientDto.Height = found.Height;
                    PatientDto.Weight = found.Weight;
                    PatientDto.Sex = found.Sex;
                    PatientDto.ActivityLevel = found.ActivityLevel;

                    MainThread.BeginInvokeOnMainThread(() => { IsLoadingVisible = false; });
                });
            }
        }

        [RelayCommand]
        private async Task Save()
        {

            ValidatePatient();

            if (!string.IsNullOrEmpty(ValidationErrors))
            {
                await Shell.Current.DisplayAlert("Error al guardar", ValidationErrors, "OK");
                return;
            }


            IsLoadingVisible = true;
            PatientMessage message = new PatientMessage();

            await Task.Run(async () =>
            {
                if(PatientID == 0)
                {
                    var tbPatient = new Patient
                    {
                        Name = PatientDto.Name,
                        LastName = PatientDto.LastName,
                        Age = PatientDto.Age,
                        Height = PatientDto.Height,
                        Weight = PatientDto.Weight,
                        Sex = PatientDto.Sex,
                        ActivityLevel = PatientDto.ActivityLevel
                    };

                    _dbContext.Patients.Add(tbPatient);
                    await _dbContext.SaveChangesAsync();
                    
                    PatientDto.PatientID = tbPatient.PatientID;
                    message = new PatientMessage()
                    {
                        IsCreate = true,
                        PatientDto = PatientDto
                    };
                }
                else
                {
                    var found = await _dbContext.Patients.FirstAsync(p => p.PatientID == PatientID);
                    found.Name = PatientDto.Name;
                    found.LastName = PatientDto.LastName;
                    found.Age = PatientDto.Age;
                    found.Height = PatientDto.Height;
                    found.Weight = PatientDto.Weight;
                    found.Sex = PatientDto.Sex;
                    found.ActivityLevel = PatientDto.ActivityLevel;

                    await _dbContext.SaveChangesAsync();

                    message = new PatientMessage()
                    {
                        IsCreate = false ,
                        PatientDto = PatientDto
                    };

                }

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    IsLoadingVisible = false;
                    WeakReferenceMessenger.Default.Send(new PatientMessenger(message));
                    await Shell.Current.Navigation.PopAsync();
                });
            });
        }

        [RelayCommand]
        private void ValidatePatient()
        {
            PatientDto.Validate();

            if (PatientDto.HasErrors)
            {
                ValidationErrors = string.Join("\n", PatientDto.GetErrors().Select(e => e.ErrorMessage));
            }
            else
            {
                ValidationErrors = string.Empty;
            }

        }

    }
}

 