using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Loans
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool State { get; set; }

    }
}
