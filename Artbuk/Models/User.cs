namespace Artbuk.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public Guid RoleId { get; set; }
        public string? ImagePath { get; set; }

        public User() : this(string.Empty, string.Empty, string.Empty) { }

        public User(string email, string name, string password)
        {
            Name = name;
            Password = password;
            Email = email;
        }
    }
}
