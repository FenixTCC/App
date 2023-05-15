﻿using QuantoDemoraApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantoDemoraApp.Services.Usuarios
{
    public class UsuarioService : Request
    {
        private readonly Request _request;

        private const string apiUrlBase = "http://quantodemora.somee.com/api/usuarios";
        
        private string _token;
        public UsuarioService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public UsuarioService()
        {
            _request = new Request();
        }

        public async Task<Usuario> PostCadastrarAsync(Usuario u)
        {
            string urlComplementar = "/Cadastrar";
            u.IdUsuario = await _request.PostReturnIntAsync(apiUrlBase + urlComplementar, u);
            return u;
        }

        public async Task<Usuario> PostAutenticarAsync(Usuario u)
        {
            string urlComplementar = "/Autenticar";
            u = await _request.PostAsync(apiUrlBase + urlComplementar, u, string.Empty);

            return u;
        }

        public async Task<int> PutFotoUsuarioAsync(Usuario u)
        {
            string urlComplementar = "/AtualizarFoto";
            var result = await _request.PutAsync(apiUrlBase + urlComplementar, u, _token);
            return result;
        }

        public async Task<Usuario> GetUsuarioAsync(int usuarioId)
        {
            string urlComplementar = string.Format("/{0}", usuarioId);
            var usuario = await
            _request.GetAsync<Models.Usuario>(apiUrlBase + urlComplementar, _token);
            return usuario;
        }
    }
}