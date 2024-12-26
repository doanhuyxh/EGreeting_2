using System.ComponentModel.DataAnnotations;

namespace EGreeting.Models
{
    public class Feedbacks
    {
        [Key]
        public int IDFeedback { get; set; }

        [Required]
        public string NameUser { get; set; }
        [Required]
        public string EmailUser { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string ContentFeedback { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
