using EGreeting.Models;

namespace EGreeting.DTO
{
    public class ViewHomeDTO
    {
        public List<CategoriesCard> SuKienSapDienRa { get; set; }
        public List<CategoriesCard> HangNgay { get; set; }
        public List<CategoriesCard> ShowMacDinh { get; set; }
    }
}
