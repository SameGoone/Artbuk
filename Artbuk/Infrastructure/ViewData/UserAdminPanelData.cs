using Artbuk.Models;

namespace Artbuk.Infrastructure.ViewData
{
    public class UserAdminPanelData
    {
        public User User { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsMe { get; set; }
    }
}
