using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookHive.Models
{
    public class LoansViewModel
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; }
        public bool State { get; set; }

        // Para los select
        
        public List<SelectListItem> User { get; set; }
        public List<SelectListItem> Book { get; set; }
        public string BookTitle { get; set; }

        public string Name { get; set; }
    }
}