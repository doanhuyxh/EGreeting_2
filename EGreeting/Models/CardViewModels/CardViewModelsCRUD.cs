namespace EGreeting.Models.CardViewModels
{
    public class CardViewModelsCRUD
    {
        public int IDCard { get; set; }
        public string NameCard { get; set; }
        public string Slug { get; set; }
        public string? ImageCardPreview { get; set; }
        public string? ImageCardMain { get; set; }

        public int CategoryID { get; set; }
        public string? ShortDecription { get; set; }
        public string? TypeJsonCard { get; set; }
        public DateTime CreateDate { get; set; }

        public IFormFile? ImageCardPreviewPath { get; set; }

        public static implicit operator CardViewModelsCRUD(Cards _card)
        {
            return new CardViewModelsCRUD
            {
                IDCard = _card.IDCard,
                NameCard = _card.NameCard,
                Slug = _card.Slug,
                ImageCardPreview = _card.ImageCardPreview,
                ImageCardMain = _card.ImageCardMain,
                CategoryID = _card.CategoryID,
                ShortDecription = _card.ShortDecription,
                TypeJsonCard = _card.TypeJsonCard,
                CreateDate = _card.CreateDate,


            };
        }

        public static implicit operator Cards(CardViewModelsCRUD vm)
        {
            return new Cards
            {

                IDCard = vm.IDCard,
                NameCard = vm.NameCard,
                Slug = vm.Slug,
                ImageCardPreview = vm.ImageCardPreview,
                ImageCardMain = vm.ImageCardMain,
                CategoryID = vm.CategoryID,
                ShortDecription = vm.ShortDecription,
                TypeJsonCard = vm.TypeJsonCard,
                CreateDate = vm.CreateDate,
            };
        }

    }
}
