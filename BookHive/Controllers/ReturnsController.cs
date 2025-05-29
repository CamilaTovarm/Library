using BookHive.Models;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace BookHive.Controllers
{
    public class ReturnsController : Controller
    {
        private readonly HttpClient _client;
        private readonly string baseApiUrl = "https://bookhive-heaedbaqfgbacdhw.canadacentral-01.azurewebsites.net/api";

        public ReturnsController()
        {
            _client = new HttpClient();
        }

        // GET: Listar devoluciones
        [HttpGet]
        public IActionResult ReturnGet()
        {
            var returnList = new List<ReturnsViewModel>();

            var response = _client.GetAsync($"{baseApiUrl}/Returns").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
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
                Loans = GetActiveLoansSelectList(),
                ReturnDate = System.DateTime.Now
            };
            return View(model);
        }

        // No hay POST porque se maneja con JS en la vista

        // Métodos auxiliares

        private List<LoansViewModel> GetLoans()
        {
            var response = _client.GetAsync($"{baseApiUrl}/Loans").Result;
            if (!response.IsSuccessStatusCode) return new List<LoansViewModel>();

            var data = response.Content.ReadAsStringAsync().Result;
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

        // Solo reservas activas (state == true)
        private List<LoansViewModel> GetActiveLoans()
        {
            return GetLoans().Where(l => l.State).ToList();
        }

        private List<SelectListItem> GetActiveLoansSelectList()
        {
            var activeLoans = GetActiveLoans();
            return activeLoans.Select(l => new SelectListItem
            {
                Value = l.LoanId.ToString(),
                Text = $"ID: {l.LoanId} - Usuario: {l.Name} - Libro: {l.BookTitle}"
            }).ToList();
        }

        private List<User> GetUsers()
        {
            var response = _client.GetAsync($"{baseApiUrl}/User").Result;
            if (!response.IsSuccessStatusCode) return new List<User>();

            var data = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<User>>(data);
        }

        private List<Book> GetBooks()
        {
            var response = _client.GetAsync($"{baseApiUrl}/Book").Result;
            if (!response.IsSuccessStatusCode) return new List<Book>();

            var data = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Book>>(data);
        }
    }
}

