using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;

using Microsoft.EntityFrameworkCore;
using Saludimetro.DataAccess;
using Saludimetro.DTOs;
using Saludimetro.Utilities;
using Saludimetro.Models;

using System.Collections.ObjectModel;
using Saludimetro.Views;

namespace Saludimetro.ViewModels
{
    public partial class PatientListViewModel : ObservableObject
    {
        private readonly PatientDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<PatientDTO> patientList = new ObservableCollection<PatientDTO>();

        public PatientListViewModel(PatientDbContext context)
        {
            _dbContext = context;
            MainThread.BeginInvokeOnMainThread(new Action(async () => await GetPatients()));

            WeakReferenceMessenger.Default.Register<PatientMessenger>(this, (r, m) =>
            {
                PatientReceivedMessage(m.Value);
            });
        }

        public async Task GetPatients()
        {
            var list = await _dbContext.Patients.ToListAsync();
            if (list.Any())
            {
                foreach (var item in list)
                {
                    PatientList.Add(new PatientDTO
                    {
                        PatientID = item.PatientID,
                        Name = item.Name,
                        LastName = item.LastName,
                        Age = item.Age,
                        Height = item.Height,
                        Weight = item.Weight,
                        Sex = item.Sex,
                        ActivityLevel = item.ActivityLevel
                    });
                }
            }
        }

        private void PatientReceivedMessage(PatientMessage patientMessage)
        {
            var patientDto = patientMessage.PatientDto;

            if (patientMessage.IsCreate)
            {
                PatientList.Add(patientDto);
            }
            else
            {
                var found = PatientList.First(p => p.PatientID == patientDto.PatientID);

                found.Name = patientDto.Name;
                found.LastName = patientDto.LastName;
                found.Age = patientDto.Age;
                found.Height = patientDto.Height;
                found.Weight = patientDto.Weight;
                found.Sex = patientDto.Sex;
                found.ActivityLevel = patientDto.ActivityLevel;
            }
        }

        [RelayCommand]
        private async Task Create()
        {
            var uri = $"{nameof(PatientPage)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Edit(PatientDTO patientDto)
        {
            var uri = $"{nameof(PatientPage)}?id={patientDto.PatientID}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Showpatient(PatientDTO patientDto)
        {
            var uri = $"{nameof(PatientShowPage)}?id={patientDto.PatientID}";
            await Shell.Current.GoToAsync(uri);
        }


        [RelayCommand]
        private async Task Delete(PatientDTO patientDto)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "Quieres eliminar el paciente?", "Si", "No");

            if (answer)
            {
                var found = await _dbContext.Patients.FirstAsync(p => p.PatientID == patientDto.PatientID);

                _dbContext.Patients.Remove(found);
                await _dbContext.SaveChangesAsync();
                PatientList.Remove(patientDto);
            }
        }
    }
}
