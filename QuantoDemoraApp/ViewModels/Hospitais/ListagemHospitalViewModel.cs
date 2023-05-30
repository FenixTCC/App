﻿using Microsoft.Maui.Devices.Sensors;
using QuantoDemoraApp.Models;
using QuantoDemoraApp.Services.Hospitais;
using QuantoDemoraApp.Services.Usuarios;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuantoDemoraApp.ViewModels.Hospitais
{
    public class ListagemHospitalViewModel : BaseViewModel
    {
        private HospitalService hService;
        private UsuarioService uService;
        public ObservableCollection<Hospital> Hospitais { get; set; }
        public ListagemHospitalViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            hService = new HospitalService(token);
            uService = new UsuarioService(token);
            Hospitais = new ObservableCollection<Hospital>();
            _ = ObterHospital(); 
        }

        public async Task ObterHospital()
        {
            try
            {
                Hospitais = await hService.GetHospitaisAsync();
                List<Hospital> listaHospitais = new List<Hospital>(Hospitais);

                Usuario usuario = new Usuario();
                var usuarioId = Preferences.Get("UsuarioId", usuario.IdUsuario);

                Usuario u = await
                    uService.GetUsuarioAsync(usuarioId);

                double uLati = (double)-23.5193200840959;
                double uLong = (double)-46.59555019558195;
                Location usuarioLoc = new Location(uLati, uLong);

                foreach (Hospital h in listaHospitais)
                {
                    if (h.Latitude != null && h.Longitude != null)
                    {
                        double hLati = (double)h.Latitude;
                        double hLong = (double)h.Longitude;
                        Location hospitalLoc = new Location(hLati, hLong);

                        this.distanciaKm = Location.CalculateDistance(usuarioLoc, hospitalLoc, DistanceUnits.Kilometers);
                    }
                }

                OnPropertyChanged(nameof(Hospitais));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ExibirInformacoesHospital()
        {
            try
            {
                await Shell.Current.GoToAsync("informacoesHospitalView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private Hospital hospitalSelecionado;
        public Hospital HospitalSelecionado
        {
            get 
            { 
                return hospitalSelecionado; 
            }
            set
            {
                if (value != null)
                {
                    hospitalSelecionado = value;

                    Shell.Current
                        .GoToAsync($"informacoesHospitalView?hIdHospital={hospitalSelecionado.IdHospital}");
                }
            }
        }

        private double distanciaKm;
        public double DistanciaKm
        {
            get => distanciaKm;
            set
            {
                distanciaKm = value;
                OnPropertyChanged();
            }
        }
    }
}
