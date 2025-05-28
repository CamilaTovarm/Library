using BookHive.Models;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace BookHive.Controllers
{
    public class ReturnsController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri baseAddress = new Uri("https://bookhive-heaedbaqfgbacdhw.canadacentral-01.azurewebsites.net");

        public ReturnsController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        // GET: Listar devoluciones
        [HttpGet]
        public IActionResult ReturnGet()
        {
            List<ReturnsViewModel> returnList = new List<ReturnsViewModel>();

            var response = _client.GetAsync(_client.BaseAddress + "/api/Returns").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                returnList = JsonConvert.DeserializeObject<List<ReturnsViewModel>>(data);

                var loans = GetLoans();
                var users = GetUsers();
                var books = GetBooks();

                foreach (var ret in returnList)
                {
                    var loan = loans.FirstOrDefault(l => l.LoanId == ret.LoanId);
                    if (loan != null)
                    {
                        ret.UserName = users.FirstOrDefault(u => u.UserId == loan.UserId)?.Name ?? "Usuario desconocido";
                        ret.BookTitle = books.FirstOrDefault(b => b.BookId == loan.BookId)?.BookTitle ?? "Libro desconocido";
                    }
                }
            }
            else
            {
                TempData["errorMessage"] = "Error al obtener las devoluciones desde la API.";
            }

            return View(returnList);
        }

        // GET: Mostrar formulario para crear devolución
        [HttpGet]
        public IActionResult Create()
        {
            var model = new ReturnsViewModel
            {
                Loans = GetLoansSelectList(),
                ReturnDate = DateTime.Now
            };
            return View(model);
        }

        // POST: Crear devolución
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReturnsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Loans = GetLoansSelectList();
                return View(model);
            }

            // Construir URL para POST con query params según API
            string urlCreateReturn = $"/api/Returns?returnDate={model.ReturnDate:yyyy-MM-dd}&loanId={model.LoanId}&fineImposed={model.FineImposed}";

            var response = _client.PostAsync(_client.BaseAddress + urlCreateReturn, null).Result;

            if (!response.IsSuccessStatusCode)
            {
                TempData["errorMessage"] = "Error al crear la devolución.";
                model.Loans = GetLoansSelectList();
                return View(model);
            }

            TempData["successMessage"] = "Devolución creada con éxito.";
            return RedirectToAction("ReturnGet");
        }

        // Métodos auxiliares para obtener datos relacionados

        private List<LoansViewModel> GetLoans()
        {
            var response = _client.GetAsync(_client.BaseAddress + "/api/Loans").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var loans = JsonConvert.DeserializeObject<List<LoansViewModel>>(data);

                var users = GetUsers();
                var books = GetBooks();

                foreach (var loan in loans)
                {
                    loan.Name = users.FirstOrDefault(u => u.UserId == loan.UserId)?.Name ?? "Usuario desconocido";
                    loan.BookTitle = books.FirstOrDefault(b => b.BookId == loan.BookId)?.BookTitle ?? "Libro desconocido";
                }

                return loans;
            }
            return new List<LoansViewModel>();
        }

        private List<SelectListItem> GetLoansSelectList()
        {
            var loans = GetLoans();
            return loans.Select(l => new SelectListItem
            {
                Value = l.LoanId.ToString(),
                Text = $"ID: {l.LoanId} - Usuario: {l.Name} - Libro: {l.BookTitle}"
            }).ToList();
        }

        private List<User> GetUsers()
        {
            var response = _client.GetAsync(_client.BaseAddress + "/api/User").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<User>>(data);
            }
            return new List<User>();
        }

        private List<Book> GetBooks()
        {
            var response = _client.GetAsync(_client.BaseAddress + "/api/Book").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Book>>(data);
            }
            return new List<Book>();
        }
    }
}
