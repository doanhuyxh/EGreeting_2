using System.ComponentModel.DataAnnotations;

namespace EGreeting.Models
{
    public class Packages
    {
        [Key]
        public int IDPackage { get; set; }
        public decimal Price { get; set; }
        public string Decription { get; set; }
        public string Duration { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
