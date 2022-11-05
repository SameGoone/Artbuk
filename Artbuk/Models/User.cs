namespace Artbuk.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public Guid RoleId { get; set; }

        public User() : this(string.Empty, string.Empty, string.Empty) { }

        public User(string email, string login, string password)
        {
            Login = login;
            Password = password;
            Email = email;
        }
    }
}
