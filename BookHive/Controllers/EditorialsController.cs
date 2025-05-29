using BookHive.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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

                // Filtrar solo editoriales activas (State == false)
                editorials = editorials.FindAll(e => e.State == false);
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




        // GET: Vista para eliminar editoriales (lista de activas)
        [HttpGet]
        public IActionResult Delete()
        {
            List<EditorialViewModel> editorials = new List<EditorialViewModel>();

            var response = _client.GetAsync(_client.BaseAddress + "/api/Editorial").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                editorials = JsonConvert.DeserializeObject<List<EditorialViewModel>>(data);
                editorials = editorials.FindAll(e => e.State == false);
            }
            else
            {
                TempData["errorMessage"] = "Error al obtener la lista de editoriales.";
            }

            return View(editorials);
        }

        // POST: Eliminar editorial (llamado desde JavaScript)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _client.DeleteAsync($"/api/Editorial/Delete/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Editorial eliminada correctamente";
            }
            else
            {
                TempData["errorMessage"] = $"Error al eliminar: {response.ReasonPhrase}";
            }

            return RedirectToAction("Delete");
        }




        // GET: Vista para actualizar editorial
        [HttpGet]
        public IActionResult Update()
        {
            return View();
        }

        // PUT: Actualizar editorial vía query string (como en el ejemplo JS)
        [HttpPut("/api/Editorial/Update/{id}")]
        public IActionResult UpdateEditorial(int id, [FromQuery] string editorialName)
        {
            try
            {
                // Obtener editorial actual
                var responseGet = _client.GetAsync($"/api/Editorial/{id}").Result;
                if (!responseGet.IsSuccessStatusCode)
                    return NotFound("Editorial no encontrada");

                var data = responseGet.Content.ReadAsStringAsync().Result;
                var editorial = JsonConvert.DeserializeObject<EditorialViewModel>(data);

                // Actualizar nombre
                editorial.EditorialName = editorialName;

                // Serializar y enviar PUT con JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(editorial), Encoding.UTF8, "application/json");
                var responsePut = _client.PutAsync($"/api/Editorial/Update/{id}", jsonContent).Result;

                if (responsePut.IsSuccessStatusCode)
                    return Ok("Editorial actualizada correctamente");
                else
                    return StatusCode((int)responsePut.StatusCode, "Error al actualizar editorial");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

    }
}
