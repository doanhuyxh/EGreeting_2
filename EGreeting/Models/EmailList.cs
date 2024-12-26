using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EGreeting.Models
{
    public class EmailList
    {
        [Key]
        public int IDEmailList { get; set; }
        public int EmailOrderID { get; set; }
        public string EmailSender { get; set; }
        public bool Status { get; set; }


        [ForeignKey("EmailOrderID")]
        [JsonIgnore]
        public virtual EmailOrders EmailOrder { get; set; }
    }
}
