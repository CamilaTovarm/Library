using BookHive.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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

                // Filtrar solo usuarios activos (State == false)
                users = users.FindAll(u => u.State == false);
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





        // GET: Mostrar vista para eliminar usuarios activos
        [HttpGet]
        public IActionResult Delete()
        {
            List<UserViewModel> users = new List<UserViewModel>();

            var response = _client.GetAsync(_client.BaseAddress + "/api/User").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<UserViewModel>>(data);

                // Filtrar solo usuarios activos (State == false)
                users = users.FindAll(u => u.State == false);
            }
            else
            {
                TempData["errorMessage"] = "Error al obtener la lista de usuarios.";
            }

            return View(users);
        }

        // POST: Eliminar usuario por Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _client.DeleteAsync($"/api/User/Delete/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Usuario eliminado con éxito.";
            }
            else
            {
                TempData["errorMessage"] = $"Error al eliminar usuario: {response.ReasonPhrase}";
            }
            return RedirectToAction("Delete");
        }

        // GET: User/Edit/5
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            UserViewModel user = null;

            var response = _client.GetAsync($"User/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserViewModel>(data);
            }
            else
            {
                TempData["errorMessage"] = "Error al obtener el usuario.";
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: User/Update
        [HttpGet]
        public IActionResult Update()
        {
            return View();
        }

        // PUT: User/Update/5?name=...&email=...&UserName=...
        [HttpPut("Update/{id}")]
        public IActionResult UpdateUser(int id, [FromQuery] string name, [FromQuery] string email, [FromQuery] string userName)
        {
            try
            {
                // Obtener usuario actual
                var responseGet = _client.GetAsync($"User/{id}").Result;
                if (!responseGet.IsSuccessStatusCode)
                    return NotFound("Usuario no encontrado");

                var data = responseGet.Content.ReadAsStringAsync().Result;
                var user = JsonConvert.DeserializeObject<UserViewModel>(data);

                // Actualizar solo los campos visibles
                user.Name = name;
                user.Email = email;
                user.UserName = userName;
                // password y userTypeId se mantienen igual

                var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var responsePut = _client.PutAsync($"User/Update/{id}", jsonContent).Result;

                if (responsePut.IsSuccessStatusCode)
                    return Ok("Usuario actualizado correctamente");
                else
                    return StatusCode((int)responsePut.StatusCode, "Error al actualizar usuario");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}


