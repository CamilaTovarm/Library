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
    public class LoansController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri baseAddress = new Uri("https://bookhive-heaedbaqfgbacdhw.canadacentral-01.azurewebsites.net");

        public LoansController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult LoanGet()
        {
            var loans = GetLoans();
            return View(loans);
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

        // Métodos auxiliares
        private List<LoansViewModel> GetLoans()
        {
            var response = _client.GetAsync("/api/Loans").Result;
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

        private List<SelectListItem> GetUsersSelectList()
        {
            var users = GetUsers();
            return users.Select(u => new SelectListItem { Value = u.UserId.ToString(), Text = u.Name }).ToList();
        }

        private List<SelectListItem> GetBooksSelectList()
        {
            var books = GetBooks().Where(b => b.LoanState == false).ToList(); // Solo libros disponibles
            return books.Select(b => new SelectListItem { Value = b.BookId.ToString(), Text = b.BookTitle }).ToList();
        }

        private List<User> GetUsers()
        {
            var response = _client.GetAsync("/api/User").Result;
            if (!response.IsSuccessStatusCode) return new List<User>();

            var data = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<User>>(data);
        }

        private List<Book> GetBooks()
        {
            var response = _client.GetAsync("/api/Book").Result;
            if (!response.IsSuccessStatusCode) return new List<Book>();

            var data = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Book>>(data);
        }
    }
}

