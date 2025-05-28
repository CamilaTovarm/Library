using BookHive.Models;
using Library.Models; // Ajusta el namespace según tu proyecto
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

    public class BooksController : Controller
    {
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

                // Si necesitas mapear o procesar datos adicionales, hazlo aquí
                // Ejemplo: formatear fechas, filtrar, etc.
            }


            return View(activeBooks);
        }
    }
}
