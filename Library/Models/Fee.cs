using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Fee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeeId { get; set; }
        public int DaysMin { get; set; }
        public int DaysMax { get; set; }
        public float FeeValue { get; set; }
        public string FeeDescription { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool State { get; set; }
    }
}
