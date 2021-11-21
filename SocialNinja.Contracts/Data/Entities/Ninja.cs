using System.ComponentModel.DataAnnotations;

namespace SocialNinja.Contracts.Data.Entities
{
    public class Ninja
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Moniker { get; set; }
        public DateTime AddedOn { get; set; }
    }
}