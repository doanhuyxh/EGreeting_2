using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EGreeting.Models
{
    public class EmailOrders
    {
        [Key]
        public int IDEmailOrder { get; set; }
        public string UserID { get; set; }
        public int CardID { get; set; }
        public string EmailSubject { get; set; }
        public DateTime CreateDate { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser? User { get; set; }

        [ForeignKey("CardID")]
        public virtual Cards? Card { get; set; }

        public virtual ICollection<EmailList> EmailLists { get; set; } = new List<EmailList>();
    }
}
