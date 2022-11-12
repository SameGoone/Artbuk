using Artbuk.Controllers;
using Artbuk.Core.Interfaces;
using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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
            postMock.Setup(r => r.GetById(postId)).Returns(post);
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
            postMock.Setup(r => r.GetById(postId)).Returns(post);
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
            var user = new User { Login = "login" };
            userMock.Setup(r => r.CheckUserExistsWithLogin(user.Login)).Returns(true);

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
            var user = new User { Email = "email" };
            userMock.Setup(r => r.CheckUserExistsWithEmail(user.Email)).Returns(true);

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
            userMock.Setup(r => r.GetByCredentials(login, password)).Returns(user);

            var controller = new ProfileController(null, userMock.Object, null);
            controller.TestForm = ConstructLoginForm(login, password);

            // Act
            var result = await controller.LoginAsync(null);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Такого пользователя не существует.", controller.ViewBag.Message);
            userMock.Verify(r => r.GetByCredentials(login, password));
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
