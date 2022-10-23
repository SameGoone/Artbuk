namespace Artbuk.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role? Role { get; set; }
        public Guid? RoleId { get; set; }
    }
}
