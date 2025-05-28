using BookHive.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

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
    }
}

