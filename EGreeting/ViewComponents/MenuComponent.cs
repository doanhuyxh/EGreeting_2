using EGreeting.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tommava.ViewComponents
{
    [ViewComponent(Name = "HeaderMain")]
    public class MenuComponent : ViewComponent
    {
        private ICommon _icommom;

        public MenuComponent(ICommon icommom)
        {
            _icommom = icommom;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menu = await _icommom.MenuNav();
            return View(menu);
        }

    }
}
