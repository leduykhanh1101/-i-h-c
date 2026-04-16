using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using KhanhAndPhuDictionary.Models;
using KhanhAndPhuDictionary.Data;

namespace KhanhAndPhuDictionary.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApplicationDbContext _context;

        public DictionaryController(IHttpClientFactory clientFactory, ApplicationDbContext context)
        {
            _clientFactory = clientFactory;
            _context = context;
        }

        public async Task<IActionResult> Search(string word)
        {
            if (string.IsNullOrEmpty(word)) return RedirectToAction("Index", "Home");

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "DotNetApp");
            var response = await client.GetAsync($"https://api.dictionaryapi.dev/api/v2/entries/en/{word}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<DictionaryResponse>>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (data != null && data.Count > 0)
                {
                    var userIdString = HttpContext.Session.GetString("UserId");
                    bool isSaved = false;
                    if (!string.IsNullOrEmpty(userIdString))
                    {
                        int userId = int.Parse(userIdString);
                        isSaved = _context.Favorites.Any(f => f.UserId == userId && f.Word.ToLower() == word.ToLower());
                    }

                    ViewBag.IsSaved = isSaved;
                    return View(data[0]);
                }
            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(string word)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để lưu từ!" });
            }

            int userId = int.Parse(userIdString);
            var existing = _context.Favorites
                .FirstOrDefault(f => f.UserId == userId && f.Word.ToLower() == word.ToLower());

            if (existing != null)
            {
                _context.Favorites.Remove(existing);
                await _context.SaveChangesAsync();
                return Json(new { success = true, isFavorite = false, message = "Đã xóa khỏi danh sách yêu thích" });
            }
            else
            {
                _context.Favorites.Add(new Favorite { UserId = userId, Word = word.Trim() });
                await _context.SaveChangesAsync();
                return Json(new { success = true, isFavorite = true, message = "Đã lưu vào danh sách yêu thích" });
            }
        }
        public IActionResult MyFavorites()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdString);
            var list = _context.Favorites
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.Id)
                .ToList();

            return View(list);
        }
    }
}