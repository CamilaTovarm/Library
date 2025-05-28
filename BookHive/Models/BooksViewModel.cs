using Library.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace BookHive.Models
{
    public class BookViewModel
    {
        public int IdBook { get; set; }
        public string BookTitle { get; set; }
        public int EditorialId { get; set; }
        public string EditorialName { get; set; }  // Para mostrar en la vista
        public DateTime PublicationDate { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string ISBN { get; set; }
        public int PageCount { get; set; }
        public string ImgUrl { get; set; }
        public bool State { get; set; }

        // Para mostrar los autores en la vista
        public List<string> AuthorNames { get; set; } = new List<string>();
    }

}
