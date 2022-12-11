using Artbuk.Controllers;
using Artbuk.Core.Interfaces;
using Artbuk.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace Artbuk.Tests
{
    public class ProfileControllerTests
    {
        [Fact]
        public void DeletePost_NullPostId()
        {
            // Arrange
            var postMock = new Mock<IPostRepository>();
            Post post = null;
            var controller = new ProfileController(postMock.Object, null, null);

            // Act
            var result = controller.DeletePost(null);

            // Assert
            postMock.Verify(r => r.Delete(post), Times.Never);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeletePost_NotExistsPostId()
        {
            // Arrange
            var postMock = new Mock<IPostRepository>();
            Post post = null;
            Guid postId = TestTools.Guid1;
            postMock.Setup(r => r.GetById(postId))
                .Returns(post);
            var controller = new ProfileController(postMock.Object, null, null);

            // Act
            var result = controller.DeletePost(postId);

            // Assert
            postMock.Verify(r => r.GetById(postId));
            postMock.Verify(r => r.Delete(post), Times.Never);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeletePost_CorrectPostId()
        {
            // Arrange
            var postMock = new Mock<IPostRepository>();
            Post post = TestTools.GetTestPosts()[0];
            Guid postId = TestTools.Guid1;
            postMock.Setup(r => r.GetById(postId))
                .Returns(post);
            var controller = new ProfileController(postMock.Object, null, null);

            // Act
            var result = controller.DeletePost(postId);

            // Assert
            postMock.Verify(r => r.GetById(postId));
            postMock.Verify(r => r.Delete(post));
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Profile", redirectToActionResult.ActionName);
            Assert.Null(redirectToActionResult.ControllerName);
        }

        [Fact]
        public void Registration_HttpGet()
        {
            // Arrange
            var controller = new ProfileController(null, null, null);

            // Act
            var result = controller.Registration();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async Task Registration_UserIsNullAsync()
        {
            // Arrange
            var userMock = new Mock<IUserRepository>();
            User user = null;
            var controller = new ProfileController(null, userMock.Object, null);

            // Act
            var result = await controller.RegistrationAsync(user);

            // Assert
            userMock.Verify(r => r.Add(user), Times.Never);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Feed", redirectToActionResult.ActionName);
            Assert.Equal("Feed", redirectToActionResult.ControllerName);
        }

        [Fact]
        public async Task Registration_UserLoginAlreadyExistsAsync()
        {
            // Arrange
            var userMock = new Mock<IUserRepository>();
            var user = new User { Login = "login", Password = "password" };
            userMock.Setup(r => r.CheckUserExistsWithLogin(user.Login))
                .Returns(true);

            var controller = new ProfileController(null, userMock.Object, null);

            // Act
            var result = await controller.RegistrationAsync(user);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Пользователь с таким логином уже существует.", controller.ViewBag.Message);
            userMock.Verify(r => r.CheckUserExistsWithLogin(user.Login));
            userMock.Verify(r => r.Add(user), Times.Never);
        }

        [Fact]
        public async Task Registration_UserEmailAlreadyExistsAsync()
        {
            // Arrange
            var userMock = new Mock<IUserRepository>();
            var user = new User { Email = "email", Login = "login", Password = "password" };
            userMock.Setup(r => r.CheckUserExistsWithEmail(user.Email))
                .Returns(true);

            var controller = new ProfileController(null, userMock.Object, null);

            // Act
            var result = await controller.RegistrationAsync(user);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Пользователь с такой почтой уже существует.", controller.ViewBag.Message);
            userMock.Verify(r => r.CheckUserExistsWithEmail(user.Email));
            userMock.Verify(r => r.Add(user), Times.Never);
        }

        [Fact]
        public async Task Registration_UserWithEmptyLoginAndPassword()
        {
            // Arrange
            var user = new User { Login = "", Password = "" };
            var controller = new ProfileController(null, null, null);

            // Act
            var result = await controller.RegistrationAsync(user);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Заполните поля!", controller.ViewBag.Message);
        }

        [Fact]
        public async Task Registration_UserWithEmptyLogin()
        {
            // Arrange
            var user = new User { Login = "", Password = "password" };
            var controller = new ProfileController(null, null, null);

            // Act
            var result = await controller.RegistrationAsync(user);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Введите логин!", controller.ViewBag.Message);
        }

        [Fact]
        public async Task Registration_UserWithEmptyPassword()
        {
            // Arrange
            var user = new User { Login = "login", Password = "" };
            var controller = new ProfileController(null, null, null);

            // Act
            var result = await controller.RegistrationAsync(user);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Введите пароль!", controller.ViewBag.Message);
        }

        [Fact]
        public void Login_HttpGet()
        {
            // Arrange
            var controller = new ProfileController(null, null, null);

            // Act
            var result = controller.Login();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async Task Login_FormWithEmptyLoginAndPassword()
        {
            // Arrange
            var controller = new ProfileController(null, null, null);
            controller.TestForm = ConstructLoginForm("", "");

            // Act
            var result = await controller.LoginAsync(null);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Заполните поля!", controller.ViewBag.Message);
        }

        [Fact]
        public async Task Login_FormWithEmptyLogin()
        {
            // Arrange
            var controller = new ProfileController(null, null, null);
            controller.TestForm = ConstructLoginForm("", "something");

            // Act
            var result = await controller.LoginAsync(null);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Введите логин!", controller.ViewBag.Message);
        }

        [Fact]
        public async Task Login_FormWithEmptyPassword()
        {
            // Arrange
            var controller = new ProfileController(null, null, null);
            controller.TestForm = ConstructLoginForm("something", "");

            // Act
            var result = await controller.LoginAsync(null);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Введите пароль!", controller.ViewBag.Message);
        }

        [Fact]
        public async Task Login_UserNotExists()
        {
            // Arrange
            var userMock = new Mock<IUserRepository>();
            User user = null;
            var login = "login";
            var password = "password";
            userMock.Setup(r => r.GetByCredentials(login, password))
                .Returns(user);

            var controller = new ProfileController(null, userMock.Object, null);
            controller.TestForm = ConstructLoginForm(login, password);

            // Act
            var result = await controller.LoginAsync(null);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Такого пользователя не существует.", controller.ViewBag.Message);
            userMock.Verify(r => r.GetByCredentials(login, password));
        }

        [Fact]
        public async Task Login_CorrectCredentials()
        {
            // Arrange
            var userMock = new Mock<IUserRepository>();
            var login = "login";
            var password = "password";
            User user = new User { Login = login, Password = password, RoleId = TestTools.Guid1 };
            userMock.Setup(r => r.GetByCredentials(login, password))
                .Returns(user);

            var roleMock = new Mock<IRoleRepository>();
            var roleName = "roleName";
            roleMock.Setup(r => r.GetRoleNameById(user.RoleId))
                .Returns(roleName);

            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var services = new ServiceCollection();
            services.AddSingleton<IAuthenticationService>(authServiceMock.Object);

            var controller = new ProfileController(null, userMock.Object, roleMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        RequestServices = services.BuildServiceProvider()
                    }
                }
            };

            controller.Request.Form = ConstructLoginForm(login, password);

            var returnUrl = "returnUrl";

            // Act
            var result = await controller.LoginAsync(returnUrl);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(returnUrl, redirectResult.Url);
            userMock.Verify(r => r.GetByCredentials(login, password));
            roleMock.Verify(r => r.GetRoleNameById(user.RoleId));
            authServiceMock.Verify(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()));
        }
        
        [Fact]
        public async Task Logout_Success()
        {
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var services = new ServiceCollection();
            services.AddSingleton<IAuthenticationService>(authServiceMock.Object);

            var controller = new ProfileController(null, null, null)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        RequestServices = services.BuildServiceProvider()
                    }
                }
            };

            // Act
            await controller.LogoutAsync();

            // Assert
            authServiceMock.Verify(_ => _.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()));
        }

        [Fact]
        public void Profile_ReturnsViewResultWithCorrectModel()
        {
            var userMock = new Mock<IUserRepository>();
            var login = "login";
            User user = new User { Login = login, Id = TestTools.Guid1 };
            userMock.Setup(r => r.GetByLogin(login))
                .Returns(user);

            var postMock = new Mock<IPostRepository>();
            var posts = TestTools.GetTestPosts();
            postMock.Setup(r => r.ListByUserId(user.Id))
                .Returns(posts);

            var controller = new ProfileController(postMock.Object, userMock.Object, null);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new MyIdentity(login)))
                }
            };

            // Act
            var result = controller.Profile();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Post>>(viewResult.Model);
            Assert.Equal(posts, model);

            userMock.Verify(r => r.GetByLogin(login));
            postMock.Verify(r => r.ListByUserId(user.Id));
        }

        public FormCollection ConstructLoginForm(string login, string password)
        {
            return new FormCollection
            (
                new Dictionary<string, StringValues>
                {
                    { "Login", new StringValues(login) },
                    { "Password", new StringValues(password) }
                }
            );
        }
    }
}
