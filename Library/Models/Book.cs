using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string ISBN { get; set; }
        public DateOnly PublicationDate { get; set; }
        public int PageCount { get; set; }
        public Editorial Editorial { get; set; }
        public int EditorialId { get; set; }
        public Country Country { get; set; }
        public int CountryId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool State { get; set; }
    }
}
