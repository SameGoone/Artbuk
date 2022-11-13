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
            var genres = TestTools.GetTestGenres();
            genreMock.Setup(repo => repo.List()).Returns(genres);

            var sofrwareMock = new Mock<ISoftwareRepository>();
            var softwares = TestTools.GetTestSoftwares();
            sofrwareMock.Setup(repo => repo.List()).Returns(softwares);

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
            Assert.Equal(softwares, model.Software);

            var postDatas = model.PostDatas;
            Assert.Equal(postDatas[0].Post, posts[0]);
            Assert.Equal(postDatas[1].Post, posts[1]);
            Assert.Equal(postDatas[2].Post, posts[2]);

            Assert.Equal(postDatas[0].LikesCount, likesCount0);
            Assert.Equal(postDatas[1].LikesCount, likesCount1);
            Assert.Equal(postDatas[2].LikesCount, likesCount2);
        }

        //[Fact]
        //public void Feed_ReturnsACorrectFeedDatas()
        //{
        //    var postMock = new Mock<IPostRepository>();
        //    var posts = TestTools.GetTestPost();

        //    var genreMock = new Mock<IGenreRepository>();
        //    var genres = TestTools.GetTestGenre();

        //    var postInGenreMock = new Mock<IPostInGenreRepository>();
        //    var postInGenre = TestTools.GetTestPostInGenre();

        //    var postInSoftwareMock = new Mock<IPostInSoftwareRepository>();
        //    var postInSoftware = TestTools.GetTestPostInSoftware();

        //    var sofrwareMock = new Mock<ISoftwareRepository>();
        //    var softwares = TestTools.GetTestSoftware();

        //    var controller = new FeedController
        //    (
        //        postMock.Object,
        //        genreMock.Object,
        //        postInGenreMock.Object,
        //        sofrwareMock.Object,
        //        postInSoftwareMock.Object,
        //        null
        //    );

        //    var result = controller.Feed(genres.Id);

        //    Assert.NotNull(result);
        //}

        [Fact]
        public void CreatePost_ReturnsAViewResultWithCorrectModel()
        {
            var genreMock = new Mock<IGenreRepository>();
            var genres = TestTools.GetTestGenres();
            genreMock.Setup(repo => repo.List()).Returns(genres);

            var softwareMock = new Mock<ISoftwareRepository>();
            var softwares = TestTools.GetTestSoftwares();
            softwareMock.Setup(repo => repo.List()).Returns(softwares);

            var controller = new FeedController(null, genreMock.Object, null, softwareMock.Object, null, null);

            // Act
            var result = controller.CreatePost();

            // Assert
            genreMock.Verify(r => r.List());
            softwareMock.Verify(r => r.List());

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CreatePostData>(viewResult.Model);

            Assert.Equal(genres, model.Genres);
            Assert.Equal(softwares, model.Software);
        }

        [Fact]
        public void CreatePost_PostIsNull()
        {
            // Arrange
            var postMock = new Mock<IPostRepository>();
            var postInGenreMock = new Mock<IPostInGenreRepository>();
            var postInSoftwareMock = new Mock<IPostInSoftwareRepository>();

            Post post = null;
            PostInGenre postInGenre = TestTools.GetTestPostInGenre();
            PostInSoftware postInSoftware = TestTools.GetTestPostInSoftware();

            var controller = new FeedController(postMock.Object, null, postInGenreMock.Object, null, postInSoftwareMock.Object, null);

            // Act
            var result = controller.CreatePost(post, postInGenre, postInSoftware);

            // Assert
            postMock.Verify(r => r.Add(post), Times.Never);
            postInGenreMock.Verify(r => r.Add(postInGenre), Times.Never);
            postInSoftwareMock.Verify(r => r.Add(postInSoftware), Times.Never);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Feed", redirectToActionResult.ActionName);
            Assert.Null(redirectToActionResult.ControllerName);
        }

        [Fact]
        public void CreatePost_GenreIsNull()
        {
            // Arrange
            var postMock = new Mock<IPostRepository>();
            var postInGenreMock = new Mock<IPostInGenreRepository>();
            var postInSoftwareMock = new Mock<IPostInSoftwareRepository>();

            Post post = TestTools.GetTestPost();
            PostInGenre postInGenre = null;
            PostInSoftware postInSoftware = TestTools.GetTestPostInSoftware();

            var controller = new FeedController(postMock.Object, null, postInGenreMock.Object, null, postInSoftwareMock.Object, null);

            // Act
            var result = controller.CreatePost(post, postInGenre, postInSoftware);

            // Assert
            postMock.Verify(r => r.Add(post), Times.Never);
            postInGenreMock.Verify(r => r.Add(postInGenre), Times.Never);
            postInSoftwareMock.Verify(r => r.Add(postInSoftware), Times.Never);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Feed", redirectToActionResult.ActionName);
            Assert.Null(redirectToActionResult.ControllerName);
        }

        [Fact]
        public void CreatePost_SoftwareIsNull()
        {
            // Arrange
            var postMock = new Mock<IPostRepository>();
            var postInGenreMock = new Mock<IPostInGenreRepository>();
            var postInSoftwareMock = new Mock<IPostInSoftwareRepository>();

            Post post = TestTools.GetTestPost();
            PostInGenre postInGenre = TestTools.GetTestPostInGenre();
            PostInSoftware postInSoftware = null;

            var controller = new FeedController(postMock.Object, null, postInGenreMock.Object, null, postInSoftwareMock.Object, null);

            // Act
            var result = controller.CreatePost(post, postInGenre, postInSoftware);

            // Assert
            postMock.Verify(r => r.Add(post), Times.Never);
            postInGenreMock.Verify(r => r.Add(postInGenre), Times.Never);
            postInSoftwareMock.Verify(r => r.Add(postInSoftware), Times.Never);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Feed", redirectToActionResult.ActionName);
            Assert.Null(redirectToActionResult.ControllerName);
        }

        //[Fact]
        //public void CreatePost_CorrectData()
        //{
        //    // Arrange
        //    var postMock = new Mock<IPostRepository>();
        //    var postInGenreMock = new Mock<IPostInGenreRepository>();
        //    var postInSoftwareMock = new Mock<IPostInSoftwareRepository>();

        //    Post post = TestTools.GetTestPost();
        //    PostInGenre postInGenre = TestTools.GetTestPostInGenre();
        //    PostInSoftware postInSoftware = TestTools.GetTestPostInSoftware();

        //    var controller = new FeedController(postMock.Object, null, postInGenreMock.Object, null, postInSoftwareMock.Object, null);

        //    // Act
        //    var result = controller.CreatePost(post, postInGenre, postInSoftware);

        //    // Assert
        //    postMock.Verify(r => r.Add(post));
        //    postInGenreMock.Verify(r => r.Add(postInGenre));
        //    postInSoftwareMock.Verify(r => r.Add(postInSoftware));

        //    var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        //    Assert.Equal("Feed", redirectToActionResult.ActionName);
        //    Assert.Null(redirectToActionResult.ControllerName);
        //}
    }
}
