using Artbuk.Controllers;
using Artbuk.Core.Interfaces;
using Artbuk.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artbuk.Tests
{
    public class FeedControllerTests
    {
        private readonly Guid _guid1 = new Guid("11111111-1111-1111-1111-111111111111");
        private readonly Guid _guid2 = new Guid("22222222-2222-2222-2222-222222222222");
        private readonly Guid _guid3 = new Guid("33333333-3333-3333-3333-333333333333");

        [Fact]
        public void Feed_ReturnsAViewResult()
        {
            // Arrange
            var postMock = new Mock<IPostRepository>();
            var posts = GetTestPosts();
            postMock.Setup(repo => repo.ListAll()).Returns(posts);

            var genreMock = new Mock<IGenreRepository>();
            var sofrwareMock = new Mock<ISoftwareRepository>();
            var controller = new FeedController(postMock.Object, genreMock.Object, null, sofrwareMock.Object, null, null);

            // Act
            var result = controller.Feed();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Feed_ReturnsResultWithCorrectModel()
        {
            // Arrange
            var postMock = new Mock<IPostRepository>();
            var posts = GetTestPosts();
            postMock.Setup(repo => repo.ListAll()).Returns(posts);

            var likesCount0 = 1;
            var likesCount1 = 4;
            var likesCount2 = 5;
            postMock.Setup(repo => repo.GetLikesCount(_guid1)).Returns(likesCount0);
            postMock.Setup(repo => repo.GetLikesCount(_guid2)).Returns(likesCount1);
            postMock.Setup(repo => repo.GetLikesCount(_guid3)).Returns(likesCount2);

            var genreMock = new Mock<IGenreRepository>();
            var genres = GetTestGenres();
            genreMock.Setup(repo => repo.List()).Returns(genres);

            var sofrwareMock = new Mock<ISoftwareRepository>();
            var sofrwares = GetTestSoftwares();
            sofrwareMock.Setup(repo => repo.List()).Returns(sofrwares);

            var controller = new FeedController
            (
                postMock.Object,
                genreMock.Object,
                null,
                sofrwareMock.Object,
                null,
                null
            );

            // Act
            var result = controller.Feed() as ViewResult;

            // Assert
            postMock.Verify(r => r.ListAll());
            postMock.Verify(r => r.GetLikesCount(_guid1));
            postMock.Verify(r => r.GetLikesCount(_guid2));
            postMock.Verify(r => r.GetLikesCount(_guid3));

            genreMock.Verify(r => r.List());

            sofrwareMock.Verify(r => r.List());

            var model = Assert.IsAssignableFrom<FeedData>(result.Model);
            Assert.Equal(genres, model.Genres);
            Assert.Equal(sofrwares, model.Software);

            var postDatas = model.PostDatas;
            Assert.Equal(postDatas[0].Post, posts[0]);
            Assert.Equal(postDatas[1].Post, posts[1]);
            Assert.Equal(postDatas[2].Post, posts[2]);

            Assert.Equal(postDatas[0].LikesCount, likesCount0);
            Assert.Equal(postDatas[1].LikesCount, likesCount1);
            Assert.Equal(postDatas[2].LikesCount, likesCount2);
        }

        public List<Post> GetTestPosts()
        {
            var posts = new List<Post>()
            {
                new Post("post1"),
                new Post("post2"),
                new Post("post3"),
            };
            posts[0].Id = _guid1;
            posts[1].Id = _guid2;
            posts[2].Id = _guid3;

            return posts;
        }

        public List<Genre> GetTestGenres()
        {
            return new List<Genre>()
            {
                new Genre(),
                new Genre(),
                new Genre()
            };
        }

        public List<Software> GetTestSoftwares()
        {
            return new List<Software>()
            {
                new Software(),
                new Software(),
                new Software()
            };
        }
    }
}
