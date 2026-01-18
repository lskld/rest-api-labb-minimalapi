using System.ComponentModel.DataAnnotations;

namespace rest_api_labb_minimalapi.Models.Entities
{
    public class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<Interest> Interests { get; set; } = new List<Interest>(); //N:M relationsship with Interest
        public ICollection<Link> Links { get; set; } = new List<Link>();
    }
}