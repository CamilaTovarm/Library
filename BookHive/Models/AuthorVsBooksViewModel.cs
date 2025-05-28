using Newtonsoft.Json;

namespace BookHive.Models
{
    public class AuthorVsBooksViewModel
    {
        [JsonProperty("authorId")]
        public int AuthorId { get; set; }

        [JsonProperty("bookId")]
        public int BookId { get; set; }
    }
}
