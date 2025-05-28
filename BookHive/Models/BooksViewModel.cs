using Library.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookHive.Models
{
    public class BookViewModel
    {
        public int IdBook { get; set; }
        public string BookTitle { get; set; }
        public string ISBN { get; set; }
        public DateTime PublicationDate { get; set; }
        public int PageCount { get; set; }
        public string Editorial { get; set; }
        public string Country { get; set; }
        public string ImgUrl { get; set; }

        // Nuevo campo para el nombre del autor
        public string AuthorName { get; set; }
    }

}
