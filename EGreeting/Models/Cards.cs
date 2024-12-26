using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EGreeting.Models
{
    public class Cards
    {
        [Key]
        public int IDCard { get; set; }
        public string NameCard { get; set; }
        public string Slug { get; set; }
        public string? ImageCardPreview { get; set; }
        public string? ImageCardMain { get; set; }

        public int CategoryID { get; set; }
        public string? ShortDecription { get; set; }
        public string? TypeJsonCard { get; set; }
        public DateTime CreateDate { get; set; }

        [ForeignKey("CategoryID")]
        public virtual CategoriesCard? Categories { get; set; }
    }
}
