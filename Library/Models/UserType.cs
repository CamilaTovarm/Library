using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class UserType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool State { get; set; }
    }
}
