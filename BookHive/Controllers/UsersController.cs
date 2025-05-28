using BookHive.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BookHive.Controllers
{
    public class UsersController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri baseAddress = new Uri("https://bookhive-heaedbaqfgbacdhw.canadacentral-01.azurewebsites.net");

        public UsersController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        // GET: Listar usuarios
        [HttpGet]
        public IActionResult Index()
        {
            List<UserViewModel> users = new List<UserViewModel>();

            var response = _client.GetAsync(_client.BaseAddress + "/api/User").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<UserViewModel>>(data);
            }
            else
            {
                TempData["errorMessage"] = "Error al obtener la lista de usuarios.";
            }

            return View(users);
        }

        // GET: Mostrar formulario para crear usuario
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Crear usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Asignar contraseña y tipo de usuario fijos
            string fixedPassword = "Dios123";
            int fixedUserTypeId = 1;

            // Construir URL para POST con parámetros query
            string urlCreateUser = $"/api/User?name={Uri.EscapeDataString(model.Name)}" +
                $"&email={Uri.EscapeDataString(model.Email)}" +
                $"&UserName={Uri.EscapeDataString(model.UserName)}" +
                $"&password={Uri.EscapeDataString(fixedPassword)}" +
                $"&userTypeId={fixedUserTypeId}";

            var response = _client.PostAsync(_client.BaseAddress + urlCreateUser, null).Result;

            if (!response.IsSuccessStatusCode)
            {
                TempData["errorMessage"] = "Error al crear el usuario.";
                return View(model);
            }

            TempData["successMessage"] = "Usuario creado con éxito.";
            return RedirectToAction("Index");
        }
    }
}
