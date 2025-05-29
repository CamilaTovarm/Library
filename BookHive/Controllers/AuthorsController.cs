using BookHive.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BookHive.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri baseAddress = new Uri("https://bookhive-heaedbaqfgbacdhw.canadacentral-01.azurewebsites.net");

        public AuthorsController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        // GET: Listar autores
        [HttpGet]
        public IActionResult Index()
        {
            List<AuthorViewModel> authors = new List<AuthorViewModel>();

            var response = _client.GetAsync(_client.BaseAddress + "/api/Author").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                authors = JsonConvert.DeserializeObject<List<AuthorViewModel>>(data);

                // Filtrar solo autores activos (State == false)
                authors = authors.FindAll(a => a.State == false);
            }
            else
            {
                TempData["errorMessage"] = "Error al obtener la lista de autores.";
            }

            return View(authors);
        }
        // GET: Mostrar formulario para crear autor
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // GET: Mostrar vista para eliminar autores activos
        [HttpGet]
        public IActionResult Delete()
        {
            List<AuthorViewModel> authors = new List<AuthorViewModel>();

            var response = _client.GetAsync(_client.BaseAddress + "/api/Author").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                authors = JsonConvert.DeserializeObject<List<AuthorViewModel>>(data);

                // Filtrar solo autores activos (State == false)
                authors = authors.FindAll(a => a.State == false);
            }
            else
            {
                TempData["errorMessage"] = "Error al obtener la lista de autores.";
            }

            return View(authors);
        }

        // POST: Eliminar autor por Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _client.DeleteAsync($"/api/Author/Delete/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Autor eliminado con éxito.";
            }
            else
            {
                TempData["errorMessage"] = $"Error al eliminar autor: {response.ReasonPhrase}";
            }
            return RedirectToAction("Delete");
        }


        [HttpGet]
        public IActionResult Edit(int idAuthor)
        {
            AuthorViewModel author = null;

            var response = _client.GetAsync($"/Author/{idAuthor}").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                author = JsonConvert.DeserializeObject<AuthorViewModel>(data);
            }
            else
            {
                TempData["errorMessage"] = "Error al obtener el Autor.";
                return RedirectToAction("Index");
            }

            return View(Index);
        }


        [HttpGet]
        public IActionResult Update()
        {
            return View();
        }


        // PUT: Actualizar autor
        [HttpPut]
        public IActionResult UpdateAuthor(int id, [FromQuery] string authorName)
        {
            try
            {
                var responseGet = _client.GetAsync($"/api/Author/{id}").Result;
                if (!responseGet.IsSuccessStatusCode)
                    return NotFound("Autor no encontrado");

                var data = responseGet.Content.ReadAsStringAsync().Result;
                var author = JsonConvert.DeserializeObject<AuthorViewModel>(data);

                author.AuthorName = authorName;

                var jsonContent = new StringContent(JsonConvert.SerializeObject(author), Encoding.UTF8, "application/json");
                var responsePut = _client.PutAsync($"/api/Author/Update/{id}", jsonContent).Result;

                if (responsePut.IsSuccessStatusCode)
                    return Ok("Autor actualizado correctamente");
                else
                    return StatusCode((int)responsePut.StatusCode, "Error al actualizar autor");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
