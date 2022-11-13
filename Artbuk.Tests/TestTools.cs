using Artbuk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artbuk.Tests
{
    internal class TestTools
    {
        public static readonly Guid Guid1 = new Guid("11111111-1111-1111-1111-111111111111");
        public static readonly Guid Guid2 = new Guid("22222222-2222-2222-2222-222222222222");
        public static readonly Guid Guid3 = new Guid("33333333-3333-3333-3333-333333333333");

        public static List<Post> GetTestPosts()
        {
            var posts = new List<Post>()
            {
                new Post("post1"),
                new Post("post2"),
                new Post("post3"),
            };
            posts[0].Id = Guid1;
            posts[1].Id = Guid2;
            posts[2].Id = Guid3;

            return posts;
        }

        public static List<Genre> GetTestGenres()
        {
            var genres = new List<Genre>()
            {
                new Genre() {Id = Guid1, Name = "genre1"},
                new Genre() {Id = Guid2, Name = "genre2"},
                new Genre() {Id = Guid3, Name = "genre3"}
            };

            return genres;
        }

        public static List<Software> GetTestSoftwares()
        {
            var softwares = new List<Software>()
            {
                new Software() {Id = Guid1, Name = "software1"},
                new Software() {Id = Guid2, Name = "software2"},
                new Software() {Id = Guid3, Name = "software3"}
            };

            return softwares;
        }

        public static PostInGenre GetTestPostInGenre()
        {
            var postInGenre = new PostInGenre() { Id = Guid1, GenreId = Guid1, PostId = Guid1};
            return postInGenre;
        }

        public static PostInSoftware GetTestPostInSoftware()
        {
            var postInSoftware = new PostInSoftware() { Id = Guid1, SoftwareId = Guid1, PostId = Guid1 };
            return postInSoftware;
        }

        public static Post GetTestPost()
        {
            var post = new Post() { Body = "post1", CreatedDate = DateTime.Now, Id = Guid1};
            return post;
        }

        public static Genre GetTestGenre()
        {
            var genre = new Genre() { Id = Guid1, Name = "genre1"};
            return genre;
        }

        public static Software GetTestSoftware()
        {
            var software = new Software() { Id = Guid1, Name = "software1" };
            return software;
        }
    }
}
