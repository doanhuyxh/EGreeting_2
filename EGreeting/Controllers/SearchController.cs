using EGreeting.Data;
using EGreeting.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EGreeting.Controllers
{
    public class SearchController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public SearchController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;

        }

        [HttpGet("search")]
        public async Task<IActionResult> Index(string search)
        {
            ViewBag.Search = search;
            if (!string.IsNullOrEmpty(search))
            {
                var cards = await _context.Cards
                    .Where(c => c.NameCard.Contains(search))
                    .ToListAsync();

                return View(cards);
            }

            var allCards = await _context.Cards.ToListAsync();
            return View(allCards);
        }

    }
}
