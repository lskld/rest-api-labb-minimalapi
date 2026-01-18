namespace rest_api_labb_minimalapi.Models.Entities
{
    public class Link
    {
        public int LinkId { get; set; }
        public string LinkUrl { get; set; } = string.Empty;

        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;

        public int InterestId { get; set; }
        public Interest Interest { get; set; } = null!;
    }
}