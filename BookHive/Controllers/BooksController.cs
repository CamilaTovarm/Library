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

                // Obtener autores y relaciones (como ya tienes)
                List<AuthorViewModel> authors = GetAuthors();

                // Obtener editoriales
                List<EditorialViewModel> editorials = GetEditorials();

                // Mapear autores a libros
                foreach (var book in booksList)
                {
                    var authorIds = authorVsBooks.Where(avb => avb.BookId == book.IdBook)
                                                .Select(avb => avb.AuthorId)
                                                .ToList();

                    var authorNames = authors.Where(a => authorIds.Contains(a.AuthorId))
                                             .Select(a => a.AuthorName)
                                             .ToList();

                    book.AuthorNames = authorNames;

                    var editorial = editorials.FirstOrDefault(e => e.EditorialId == book.EditorialId);
                    book.EditorialName = editorial != null ? editorial.EditorialName : "Editorial desconocida";
                }
            }

            var activeBooks = booksList.Where(b => b != null).ToList();

            return View(activeBooks);
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

        }

