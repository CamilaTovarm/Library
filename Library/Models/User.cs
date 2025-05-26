using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; }
        public int UserTypeId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool State { get; set; }
    }
}
