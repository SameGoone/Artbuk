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
        [Fact]
        public void Feed_ReturnsAViewResult()
        {
            // Arrange
            var postMock = new Mock<IPostRepository>();
            var posts = TestTools.GetTestPosts();
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
            var posts = TestTools.GetTestPosts();
            postMock.Setup(repo => repo.ListAll()).Returns(posts);

            var postId0 = TestTools.Guid1;
            var postId1 = TestTools.Guid2;
            var postId2 = TestTools.Guid3;

            var likesCount0 = 1;
            var likesCount1 = 4;
            var likesCount2 = 5;
            postMock.Setup(repo => repo.GetLikesCount(postId0)).Returns(likesCount0);
            postMock.Setup(repo => repo.GetLikesCount(postId1)).Returns(likesCount1);
            postMock.Setup(repo => repo.GetLikesCount(postId2)).Returns(likesCount2);

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
            postMock.Verify(r => r.GetLikesCount(postId0));
            postMock.Verify(r => r.GetLikesCount(postId1));
            postMock.Verify(r => r.GetLikesCount(postId2));

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
