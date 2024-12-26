using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EGreeting.Models
{
    public class Recharge
    {
        [Key]
        public int IDRecharge { get; set; }
        public string UserID { get; set; }
        public int PackageID { get; set; }
        public string CodeRecharge { get; set; }
        public decimal PriceRecharge { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser? UserData { get; set; }

        [ForeignKey("PackageID")]
        public virtual Packages? Package { get; set; }
    }
}
