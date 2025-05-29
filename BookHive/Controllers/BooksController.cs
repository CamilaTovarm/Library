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


        // GET: Books/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Authors = GetAuthors();
            ViewBag.Editorials = GetEditorials();
            ViewBag.Countries = GetCountries();

            var model = new BookViewModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Authors = GetAuthors();
                model.Editorials = GetEditorials();
                model.Countries = GetCountries();
                return View(model);
            }

            try
            {
                // Construir URL con parámetros codificados
                string url = $"/Book?" +
                             $"title={Uri.EscapeDataString(model.BookTitle)}" +
                             $"&isbn={Uri.EscapeDataString(model.ISBN ?? "")}" +
                             $"&publicationDate={model.PublicationDate:yyyy-MM-dd}" +
                             $"&pageCount={model.PageCount}" +
                             $"&editorialId={model.EditorialId}" +
                             $"&countryId={model.CountryId}" +
                             $"&imgUrl={Uri.EscapeDataString(model.ImgUrl ?? "")}" +
                             $"&authorId={model.AuthorId}" +
                             $"&loanState={model.LoanState.ToString().ToLower()}";


                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "¡Libro creado con éxito!";
                    return RedirectToAction("Books");
                }
                else
                {
                    string errorContent = response.Content.ReadAsStringAsync().Result;
                    TempData["errorMessage"] = $"Error de la API: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Error interno: {ex.Message}";
            }

            // Recargar listas si hay error
            model.Authors = GetAuthors();
            model.Editorials = GetEditorials();
            model.Countries = GetCountries();
            return View(model);
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
