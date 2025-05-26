using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Editorial
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EditorialId { get; set; }
        public string EditorialName { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool State { get; set; }
    }
}
