using Newtonsoft.Json;

namespace BookHive.Models
{
    public class EditorialViewModel
    {
        [JsonProperty("idEditorial")]
        public int EditorialId { get; set; }
        [JsonProperty("nameEditorial")]
        public string EditorialName { get; set; }
        public bool State { get; set; }
    }

}
