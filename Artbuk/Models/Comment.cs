﻿namespace Artbuk.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public Post? Post { get; set; }
        public Guid? PostId { get; set; }
        public User? User { get; set; }
        public Guid? UserId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
