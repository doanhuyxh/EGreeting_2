using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EGreeting.Models
{
    public class OrdersPackage
    {
        [Key]
        public int IDOrderPackage { get; set; }
        public string UserID { get; set; }
        public int PackageID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser? User { get; set; }

        [ForeignKey("PackageID")]
        public virtual Packages? Package { get; set; }


    }
}
