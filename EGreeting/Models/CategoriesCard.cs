using System.ComponentModel.DataAnnotations;

namespace EGreeting.Models
{
    public class CategoriesCard
    {
        [Key]
        public int IDCategoryCard { get; set; }
        public string NameCategory { get; set; }
        public string? ShortDecription { get; set; }
        public string Slug { get; set; }
        public bool IsComming { get; set; }
        public bool IsOutstanding { get; set; }
        public bool IsDefault { get; set; }

    }
}
