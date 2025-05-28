using Library.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

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

        [Display(Name = "Estado del préstamo")]
        public bool LoanState { get; set; } = false;

        // Listas para selects en la vista (no se envían a la API)
        public List<AuthorViewModel> Authors { get; set; } = new List<AuthorViewModel>();
        public List<EditorialViewModel> Editorials { get; set; } = new List<EditorialViewModel>();
        public List<CountryViewModel> Countries { get; set; } = new List<CountryViewModel>();
    }
}
