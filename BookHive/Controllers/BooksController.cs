using BookHive.Models;
using Library.Models; // Ajusta el namespace según tu proyecto
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

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

                // Obtener autores y relaciones
                List<AuthorViewModel> authors = GetAuthors();
                List<AuthorVsBooksViewModel> authorVsBooks = GetAuthorVsBooks();

                // Obtener editoriales
                List<EditorialViewModel> editorials = GetEditorials();

                // Obtener países
                List<CountryViewModel> countries = GetCountries();

                // Mapear autores, editoriales y países a libros
                foreach (var book in booksList)
                {
                    // Autores
                    var authorIds = authorVsBooks.Where(avb => avb.BookId == book.IdBook)
                                                .Select(avb => avb.AuthorId)
                                                .ToList();

                    var authorNames = authors.Where(a => authorIds.Contains(a.AuthorId))
                                             .Select(a => a.AuthorName)
                                             .ToList();

                    book.AuthorNames = authorNames;

                    // Editorial
                    var editorial = editorials.FirstOrDefault(e => e.EditorialId == book.EditorialId);
                    book.EditorialName = editorial != null ? editorial.EditorialName : "Editorial desconocida";

                    // País
                    var country = countries.FirstOrDefault(c => c.CountryId == book.CountryId);
                    book.CountryName = country != null ? country.CountryName : "País desconocido";
                }
            }

            var activeBooks = booksList.Where(b => b != null).ToList();

            return View(activeBooks);
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

        private List<AuthorVsBooksViewModel> GetAuthorVsBooks()
        {
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/AuthorVsBook").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<AuthorVsBooksViewModel>>(data);
            }
            return new List<AuthorVsBooksViewModel>();
        }

    }
}