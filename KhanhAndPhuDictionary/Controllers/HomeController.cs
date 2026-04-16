using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using KhanhAndPhuDictionary.Models;
using KhanhAndPhuDictionary.Data; 
using System.Linq;

namespace KhanhAndPhuDictionary.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var userIdString = HttpContext.Session.GetString("UserId");
        List<string> recentWords = new List<string>();

        if (!string.IsNullOrEmpty(userIdString))
        {
            int userId = int.Parse(userIdString);
            recentWords = _context.Favorites
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.Id) 
                .Select(f => f.Word)
                .Take(3)
                .ToList();
        }
        ViewBag.RecentSearches = recentWords;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}