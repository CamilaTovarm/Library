using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using BookHive.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BookHive.Controllers
{
    public class LoansController : Controller
    {
        private readonly ILogger<LoansController> _logger;
        private readonly HttpClient _client;
        private readonly Uri baseAddress = new Uri("https://bookhive-heaedbaqfgbacdhw.canadacentral-01.azurewebsites.net");

        public LoansController(ILogger<LoansController> logger)
        {
            _logger = logger;
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        [HttpGet]
        public IActionResult LoanGet()
        {
            List<LoansViewModel> loanList = new List<LoansViewModel>();

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/api/Loans").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                loanList = JsonConvert.DeserializeObject<List<LoansViewModel>>(data);

                var users = GetUsers();
                var books = GetBooks();

                foreach (var loan in loanList)
                {
                    loan.Name = users.FirstOrDefault(u => u.UserId == loan.UserId)?.Name ?? "Usuario desconocido";
                    loan.BookTitle = books.FirstOrDefault(b => b.BookId == loan.BookId)?.BookTitle ?? "Libro desconocido";
                }
            }
            else
            {
                TempData["errorMessage"] = "Error al obtener las reservas desde la API.";
            }

            return View(loanList);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var model = new LoansViewModel
            {
                User = GetUsersSelectList(),
                Book = GetBooksSelectList(),
                LoanDate = DateTime.Now
            };
            return View(model);
        }

        // Ya no necesitas POST porque el envío se hará vía JS directamente a la API
        // Pero puedes dejarlo para fallback o validación adicional si quieres

        // Métodos auxiliares para llenar selects
        private List<User> GetUsers()
        {
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/api/User").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<User>>(data);
            }
            return new List<User>();
        }

        private List<Book> GetBooks()
        {
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/api/Book").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Book>>(data);
            }
            return new List<Book>();
        }

        private List<SelectListItem> GetUsersSelectList()
        {
            var users = GetUsers();
            return users.Select(u => new SelectListItem
            {
                Value = u.UserId.ToString(),
                Text = u.Name
            }).ToList();
        }

        private List<SelectListItem> GetBooksSelectList()
        {
            var books = GetBooks();
            return books.Select(b => new SelectListItem
            {
                Value = b.BookId.ToString(),
                Text = b.BookTitle
            }).ToList();
        }
    }
}
