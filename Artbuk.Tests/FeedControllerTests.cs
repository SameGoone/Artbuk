using Artbuk.Controllers;
using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Artbuk.Tests
{
    public class FeedControllerTests
    {
        [Fact]
        public void Feed_ReturnsAViewResult()
        {
            // Arrange
            var postMock = new Mock<PostRepository>();
            var posts = TestTools.GetTestPosts();
            postMock.Setup(repo => repo.ListAll()).Returns(posts);

            var genreMock = new Mock<GenreRepository>();
            var sofrwareMock = new Mock<SoftwareRepository>();
            var likeMock = new Mock<LikeRepository>();
            var controller = new FeedController(postMock.Object, genreMock.Object, null, sofrwareMock.Object, null, null, likeMock.Object);

            // Act
            var result = controller.Feed();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Feed_ReturnsResultWithCorrectModel()
        {
            // Arrange
            var postMock = new Mock<PostRepository>();
            var posts = TestTools.GetTestPosts();
            postMock.Setup(repo => repo.ListAll()).Returns(posts);

            var postId0 = TestTools.Guid1;
            var postId1 = TestTools.Guid2;
            var postId2 = TestTools.Guid3;

            var likeMock = new Mock<LikeRepository>();

            var likesCount0 = 1;
            var likesCount1 = 4;
            var likesCount2 = 5;
            likeMock.Setup(repo => repo.GetPostLikesCount(postId0)).Returns(likesCount0);
            likeMock.Setup(repo => repo.GetPostLikesCount(postId1)).Returns(likesCount1);
            likeMock.Setup(repo => repo.GetPostLikesCount(postId2)).Returns(likesCount2);

            var genreMock = new Mock<GenreRepository>();
            var genres = TestTools.GetTestGenres();
            genreMock.Setup(repo => repo.List()).Returns(genres);

            var sofrwareMock = new Mock<SoftwareRepository>();
            var softwares = TestTools.GetTestSoftwares();
            sofrwareMock.Setup(repo => repo.List()).Returns(softwares);

            var controller = new FeedController
            (
                postMock.Object,
                genreMock.Object,
                null,
                sofrwareMock.Object,
                null,
                null,
                likeMock.Object
            );

            // Act
            var result = controller.Feed() as ViewResult;

            // Assert
            postMock.Verify(r => r.ListAll());
            likeMock.Verify(r => r.GetPostLikesCount(postId0));
            likeMock.Verify(r => r.GetPostLikesCount(postId1));
            likeMock.Verify(r => r.GetPostLikesCount(postId2));

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

        [Fact]
        public void Feed_ReturnsACorrectFeedDatas()
        {
            // Arrange
            var ids = TestTools.GetTestIds();

            var postMock = new Mock<PostRepository>();
            var posts = TestTools.GetTestPosts();
            postMock.Setup(repo => repo.GetByIds(ids)).Returns(posts);

            var genreMock = new Mock<GenreRepository>();
            var genres = TestTools.GetTestGenres();
            genreMock.Setup(repo => repo.List()).Returns(genres);

            var postInGenreMock = new Mock<PostInGenreRepository>();
            var postInGenre = TestTools.GetTestPostInGenre();
            postInGenreMock.Setup(repo => repo.GetPostIdsByGenreId(genres[0].Id)).Returns(ids);

            var postInSoftwareMock = new Mock<PostInSoftwareRepository>();
            var postInSoftware = TestTools.GetTestPostInSoftware();

            var softwareMock = new Mock<SoftwareRepository>();
            var softwares = TestTools.GetTestSoftwares();
            softwareMock.Setup(repo => repo.List()).Returns(softwares);

            var likeMock = new Mock<LikeRepository>();
            likeMock.Setup(repo => repo.GetPostLikesCount(posts[0].Id)).Returns(0);
            likeMock.Setup(repo => repo.GetPostLikesCount(posts[1].Id)).Returns(0);
            likeMock.Setup(repo => repo.GetPostLikesCount(posts[2].Id)).Returns(0);

            var controller = new FeedController
            (
                postMock.Object,
                genreMock.Object,
                postInGenreMock.Object,
                softwareMock.Object,
                postInSoftwareMock.Object,
                null,
                likeMock.Object
            );

            // Act
            var result = controller.Feed(genres[0].Id);

            // Assert
            postMock.Verify(r => r.GetByIds(ids));
            likeMock.Verify(r => r.GetPostLikesCount(posts[0].Id));
            likeMock.Verify(r => r.GetPostLikesCount(posts[1].Id));
            likeMock.Verify(r => r.GetPostLikesCount(posts[2].Id));

            postInGenreMock.Verify(r => r.GetPostIdsByGenreId(genres[0].Id));

            genreMock.Verify(r => r.List());

            softwareMock.Verify(r => r.List());

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<FeedData>(viewResult.Model);

            Assert.Equal(genres, model.Genres);
            Assert.Equal(softwares, model.Software);

            var postDatas = model.PostDatas;
            Assert.Equal(postDatas[0].Post, posts[0]);
            Assert.Equal(postDatas[1].Post, posts[1]);
            Assert.Equal(postDatas[2].Post, posts[2]);

            Assert.NotNull(result);
        }

        [Fact]
        public void CreatePost_ReturnsAViewResultWithCorrectModel()
        {
            // Arrange
            var genreMock = new Mock<GenreRepository>();
            var genres = TestTools.GetTestGenres();
            genreMock.Setup(repo => repo.List()).Returns(genres);

            var softwareMock = new Mock<SoftwareRepository>();
            var softwares = TestTools.GetTestSoftwares();
            softwareMock.Setup(repo => repo.List()).Returns(softwares);

            var controller = new FeedController(null, genreMock.Object, null, softwareMock.Object, null, null, null);

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
            var postMock = new Mock<PostRepository>();
            var postInGenreMock = new Mock<PostInGenreRepository>();
            var postInSoftwareMock = new Mock<PostInSoftwareRepository>();

            Post post = null;
            PostInGenre postInGenre = TestTools.GetTestPostInGenre();
            PostInSoftware postInSoftware = TestTools.GetTestPostInSoftware();

            var controller = new FeedController(postMock.Object, null, postInGenreMock.Object, null, postInSoftwareMock.Object, null, null);

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
            var postMock = new Mock<PostRepository>();
            var postInGenreMock = new Mock<PostInGenreRepository>();
            var postInSoftwareMock = new Mock<PostInSoftwareRepository>();

            Post post = TestTools.GetTestPost();
            PostInGenre postInGenre = null;
            PostInSoftware postInSoftware = TestTools.GetTestPostInSoftware();

            var controller = new FeedController(postMock.Object, null, postInGenreMock.Object, null, postInSoftwareMock.Object, null, null);

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
            var postMock = new Mock<PostRepository>();
            var postInGenreMock = new Mock<PostInGenreRepository>();
            var postInSoftwareMock = new Mock<PostInSoftwareRepository>();

            Post post = TestTools.GetTestPost();
            PostInGenre postInGenre = TestTools.GetTestPostInGenre();
            PostInSoftware postInSoftware = null;

            var controller = new FeedController(postMock.Object, null, postInGenreMock.Object, null, postInSoftwareMock.Object, null, null);

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
        public void CreatePost_AllIsNull()
        {
            // Arrange
            var postMock = new Mock<PostRepository>();
            var postInGenreMock = new Mock<PostInGenreRepository>();
            var postInSoftwareMock = new Mock<PostInSoftwareRepository>();

            Post post = null;
            PostInGenre postInGenre = null;
            PostInSoftware postInSoftware = null;

            var controller = new FeedController(postMock.Object, null, postInGenreMock.Object, null, postInSoftwareMock.Object, null, null);

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
        public void CreatePost_CorrectData()
        {
            // Arrange
            var postMock = new Mock<PostRepository>();
            var postInGenreMock = new Mock<PostInGenreRepository>();
            var postInSoftwareMock = new Mock<PostInSoftwareRepository>();

            var userMock = new Mock<UserRepository>();
            var login = "login";
            User user = new User { Login = login, Id = TestTools.Guid1 };
            userMock.Setup(r => r.GetByLogin(login))
                .Returns(user);

            Post post = TestTools.GetTestPost();
            PostInGenre postInGenre = TestTools.GetTestPostInGenre();
            PostInSoftware postInSoftware = TestTools.GetTestPostInSoftware();

            var controller = new FeedController(postMock.Object, null, postInGenreMock.Object, null, postInSoftwareMock.Object, userMock.Object, null);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new MyIdentity(login)))
                }
            };

            // Act
            var result = controller.CreatePost(post, postInGenre, postInSoftware);

            // Assert
            postMock.Verify(r => r.Add(post));
            postInGenreMock.Verify(r => r.Add(postInGenre));
            postInSoftwareMock.Verify(r => r.Add(postInSoftware));

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Feed", redirectToActionResult.ActionName);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(user.Id, post.UserId);
        }
    }
}
