using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace BookHive.Models
{
    public class ReturnsViewModel
    {
        public int ReturnId { get; set; }
        public DateTime ReturnDate { get; set; }
        public int LoanId { get; set; }
        public float FineImposed { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool State { get; set; }

        // Para mostrar datos relacionados
        public string UserName { get; set; }
        public string BookTitle { get; set; }

        // Para dropdown en Create
        public List<SelectListItem> Loans { get; set; } = new List<SelectListItem>();
    }
}


