using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class AuthorVsBook
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorVsBookId { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
        public Authors Authors { get; set; }
        public int AuthorId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool State { get; set; }
    }
}
