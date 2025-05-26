using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Country
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool State { get; set; }
    }
}
