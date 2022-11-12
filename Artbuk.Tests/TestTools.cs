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
    }
}
