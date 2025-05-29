using BookHive.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace BookHive.Controllers
{
    public class EditorialsController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri baseAddress = new Uri("https://bookhive-heaedbaqfgbacdhw.canadacentral-01.azurewebsites.net");

        public EditorialsController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        // GET: Listar editoriales
        [HttpGet]
        public IActionResult Index()
        {
            List<EditorialViewModel> editorials = new List<EditorialViewModel>();

            var response = _client.GetAsync(_client.BaseAddress + "/api/Editorial").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                editorials = JsonConvert.DeserializeObject<List<EditorialViewModel>>(data);
            }
            else
            {
                TempData["errorMessage"] = "Error al obtener la lista de editoriales.";
            }

            return View(editorials);
        }

        // GET: Mostrar formulario para crear editorial
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // GET: Mostrar vista para eliminar editoriales activas
        [HttpGet]
        public IActionResult Delete()
        {
            List<EditorialViewModel> editorials = new List<EditorialViewModel>();

            var response = _client.GetAsync(_client.BaseAddress + "/api/Editorial").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                editorials = JsonConvert.DeserializeObject<List<EditorialViewModel>>(data);

                // Filtrar solo editoriales activas (State == false)
                editorials = editorials.FindAll(e => e.State == false);
            }
            else
            {
                TempData["errorMessage"] = "Error al obtener la lista de editoriales.";
            }

            return View(editorials);
        }

        // POST: Eliminar editorial por Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _client.DeleteAsync($"/api/Editorial/Delete/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Editorial eliminada con éxito.";
            }
            else
            {
                TempData["errorMessage"] = $"Error al eliminar editorial: {response.ReasonPhrase}";
            }
            return RedirectToAction("Delete");
        }
    }
}
