using System.ComponentModel.DataAnnotations;

namespace SocialNinja.Contracts.Data.Entities
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string OIdProvider { get; set; }
        public string OId { get; set; }
    }
}