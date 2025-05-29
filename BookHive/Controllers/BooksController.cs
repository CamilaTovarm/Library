using BookHive.Models;
using Library.Models; // Ajusta el namespace según tu proyecto
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace FrontBerries.Controllers
{
    public class BooksController : Controller
    {
        private readonly Uri baseAddress = new Uri("https://bookhive-heaedbaqfgbacdhw.canadacentral-01.azurewebsites.net/api");
        private readonly HttpClient _client;

        public BooksController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Books()
        {
            List<BookViewModel> booksList = new List<BookViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Book").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                booksList = JsonConvert.DeserializeObject<List<BookViewModel>>(data);

                // Obtener autores directamente
                List<AuthorViewModel> authors = GetAuthors();

                // Obtener editoriales
                List<EditorialViewModel> editorials = GetEditorials();

                // Obtener países
                List<CountryViewModel> countries = GetCountries();

                // Mapear autor, editorial y país a cada libro
                foreach (var book in booksList)
                {
                    // Mapear autor directamente (asumiendo BookViewModel tiene AuthorId)
                    var author = authors.FirstOrDefault(a => a.AuthorId == book.AuthorId);
                    book.AuthorNames = author != null ? new List<string> { author.AuthorName } : new List<string> { "Autor desconocido" };

                    // Mapear editorial
                    var editorial = editorials.FirstOrDefault(e => e.EditorialId == book.EditorialId);
                    book.EditorialName = editorial != null ? editorial.EditorialName: "Editorial desconocida";

                    // Mapear país
                    var country = countries.FirstOrDefault(c => c.CountryId == book.CountryId);
                    book.CountryName = country != null ? country.CountryName : "País desconocido";
                }
            }

            // Filtrar solo libros activos (Estado == 0)
            var activeBooks = booksList.Where(b => b != null && b.State == false).ToList();
            return View(activeBooks);
        }




        [HttpGet]
        public IActionResult Delete()
        {
            // Cargar lista de libros para mostrar
            var books = Books(); // Implementa este método para obtener libros
            return View(books);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Lógica para eliminar libro vía API
            var response = _client.DeleteAsync($"/Book/Delete/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Libro eliminado con éxito.";
            }
            else
            {
                TempData["errorMessage"] = $"Error al eliminar libro: {response.ReasonPhrase}";
            }
            return RedirectToAction("Delete");
        }





        ///////////////////////////////////////
        /// 
        // GET: Mostrar formulario para editar libro
        [HttpGet]
        public IActionResult Edit(int id)
        {
            BookViewModel book = null;

            var response = _client.GetAsync($"/Book/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<BookViewModel>(data);
            }
            else
            {
                TempData["errorMessage"] = "Error al obtener el libro.";
                return RedirectToAction("Books");
            }

            // Cargar listas para dropdowns
            ViewBag.Authors = GetAuthors();
            ViewBag.Editorials = GetEditorials();
            ViewBag.Countries = GetCountries();

            return View(book);
        }

        [HttpGet]
        public IActionResult Update()
        {
            return View();
        }

        // Acción para actualizar libro vía PUT con parámetros en query string
        [HttpPut("/Book/Update/{id}")]
        public IActionResult UpdateBook(int id,
            [FromQuery] string title,
            [FromQuery] string isbn,
            [FromQuery] DateTime publicationDate,
            [FromQuery] int pageCount,
            [FromQuery] int editorialId,
            [FromQuery] int countryId,
            [FromQuery] string imgUrl,
            [FromQuery] int authorId,
            [FromQuery] bool loanState)
        {
            try
            {
                var responseGet = _client.GetAsync($"/Book/{id}").Result;
                if (!responseGet.IsSuccessStatusCode)
                    return NotFound("Libro no encontrado");

                var data = responseGet.Content.ReadAsStringAsync().Result;
                var book = JsonConvert.DeserializeObject<BookViewModel>(data);

                book.BookTitle = title;
                book.ISBN = isbn;
                book.PublicationDate = publicationDate;
                book.PageCount = pageCount;
                book.EditorialId = editorialId;
                book.CountryId = countryId;
                book.ImgUrl = imgUrl;
                book.AuthorId = authorId;
                book.LoanState = loanState;

                var jsonContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
                var responsePut = _client.PutAsync($"/Book/Update/{id}", jsonContent).Result;

                if (responsePut.IsSuccessStatusCode)
                    return Ok("Libro actualizado correctamente");
                else
                    return StatusCode((int)responsePut.StatusCode, "Error al actualizar libro");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }




        private List<EditorialViewModel> GetEditorials()
        {
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Editorial").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<EditorialViewModel>>(data);
            }
            return new List<EditorialViewModel>();
        }

        private List<AuthorViewModel> GetAuthors()
        {
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Author").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<AuthorViewModel>>(data);
            }
            return new List<AuthorViewModel>();
        }

        private List<CountryViewModel> GetCountries()
        {
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Country").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<CountryViewModel>>(data);
            }
            return new List<CountryViewModel>();
        }
    }
}
