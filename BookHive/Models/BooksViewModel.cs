using BookHive.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookHive.Models
{
    public class BookViewModel
    {
        public int IdBook { get; set; }  // Para edición, opcional

        [Required(ErrorMessage = "El título es obligatorio")]
        [Display(Name = "Título")]
        public string BookTitle { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio")]
        [Display(Name = "Autor")]
        public int AuthorId { get; set; }

        // Para mostrar nombre(s) de autor(es) en listas o detalles
        public List<string> AuthorNames { get; set; } = new List<string>();

        [Required(ErrorMessage = "La editorial es obligatoria")]
        [Display(Name = "Editorial")]
        public int EditorialId { get; set; }

        public string EditorialName { get; set; }

        [Required(ErrorMessage = "El país es obligatorio")]
        [Display(Name = "País")]
        public int CountryId { get; set; }

        public string CountryName { get; set; }

        [Required(ErrorMessage = "La fecha de publicación es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de publicación")]
        public DateTime PublicationDate { get; set; }

        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El número de páginas debe ser mayor que cero")]
        [Display(Name = "Número de páginas")]
        public int PageCount { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "URL de la portada")]
        public string ImgUrl { get; set; }

        [Display(Name = "Estado del préstamo")]
        public bool LoanState { get; set; } = false;

        // Listas para selects en la vista (no se envían a la API)
        public List<AuthorViewModel> Authors { get; set; } = new List<AuthorViewModel>();
        public List<EditorialViewModel> Editorials { get; set; } = new List<EditorialViewModel>();
        public List<CountryViewModel> Countries { get; set; } = new List<CountryViewModel>();
    }
}
