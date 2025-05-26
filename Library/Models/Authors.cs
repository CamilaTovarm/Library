using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Authors
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool State { get; set; }
    }
}
