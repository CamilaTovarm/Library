using Newtonsoft.Json;
using System.ComponentModel;

namespace BookHive.Models
{
    public class AuthorViewModel
    {
        [JsonProperty("authorId")]
        public int AuthorId { get; set; }

        [JsonProperty("authorName")]
        public string AuthorName { get; set; }
    }
}
