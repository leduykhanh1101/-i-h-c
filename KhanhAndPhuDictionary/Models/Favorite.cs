using System.ComponentModel.DataAnnotations;

namespace KhanhAndPhuDictionary.Models
{
public class Favorite {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Word { get; set; } = string.Empty;
}
}