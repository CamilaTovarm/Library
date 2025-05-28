namespace BookHive.Models
{
    public class AuthorsViewModel
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool State { get; set; }
    }
}
