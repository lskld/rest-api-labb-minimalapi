namespace rest_api_labb_minimalapi.Models.Entities
{
    public class Interest
    {
        public int InterestId { get; set; }
        public string InterestName { get; set; } = string.Empty;
        public string InterestDescription { get; set; } = string.Empty;

        public ICollection<Person> People { get; set; } = new List<Person>(); //N:M relationsship with Person
        public ICollection<Link> Links { get; set; } = new List<Link>();
    }
}