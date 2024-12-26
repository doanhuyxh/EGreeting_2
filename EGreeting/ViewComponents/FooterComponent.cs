using Microsoft.AspNetCore.Mvc;

namespace EGreeting.ViewComponents
{
    [ViewComponent(Name = "FooterMain")]
    public class FooterComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
