using Microsoft.VisualStudio.TestTools.UnitTesting;
using CRUD_application_2.Controllers;
using CRUD_application_2.Models;
using System.Web.Mvc;
using System.Linq;

namespace CRUD_application_2.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        private UserController controller;

        [TestInitialize]
        public void Setup()
        {
            controller = new UserController();
        }

        [TestMethod]
        public void Index_ReturnsViewWithUsers()
        {
            // Arrange

            // Act
            var result = controller.Index(null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as System.Collections.Generic.List<User>;
            Assert.IsNotNull(model);
            CollectionAssert.AreEqual(UserController.userlist, model);
        }

        [TestMethod]
        public void Details_UserExists_ReturnsViewWithUser()
        {
            // Arrange
            var testUser = new User { Id = 1, Name = "Test User", Email = "test@example.com" };
            UserController.userlist.Add(testUser);

            // Act
            var result = controller.Details(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as User;
            Assert.IsNotNull(model);
            Assert.AreEqual(testUser, model);
        }

        [TestMethod]
        public void Create_Post_ValidUser_RedirectsToIndex()
        {
            // Arrange
            var newUser = new User { Id = 2, Name = "New User", Email = "new@example.com" };

            // Act
            var result = controller.Create(newUser) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.IsTrue(UserController.userlist.Contains(newUser));
        }

        [TestMethod]
        public void Edit_Post_UserExists_UpdatesUser()
        {
            // Arrange
            var existingUser = new User { Id = 3, Name = "Existing User", Email = "existing@example.com" };
            UserController.userlist.Add(existingUser);
            var updatedUser = new User { Id = 3, Name = "Updated User", Email = "updated@example.com" };

            // Act
            var result = controller.Edit(3, updatedUser) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            var userInList = UserController.userlist.FirstOrDefault(u => u.Id == 3);
            Assert.IsNotNull(userInList);
            Assert.AreEqual(updatedUser.Name, userInList.Name);
            Assert.AreEqual(updatedUser.Email, userInList.Email);
        }

        [TestMethod]
        public void Delete_Post_UserExists_RemovesUser()
        {
            // Arrange
            var userToDelete = new User { Id = 4, Name = "Delete User", Email = "delete@example.com" };
            UserController.userlist.Add(userToDelete);

            // Act
            var result = controller.Delete(4, null) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(UserController.userlist.Contains(userToDelete));
        }

        [TestMethod]
        public void Search_ReturnsViewWithMatchingUsers()
        {
            // Arrange
            var user1 = new User { Id = 5, Name = "John Doe", Email = "john@example.com" };
            var user2 = new User { Id = 6, Name = "Jane Smith", Email = "jane@example.com" };
            var user3 = new User { Id = 7, Name = "John Smith", Email = "john@example.com" };
            UserController.userlist.Add(user1);
            UserController.userlist.Add(user2);
            UserController.userlist.Add(user3);

            // Act
            var result = controller.Index("John") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as System.Collections.Generic.List<User>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
            Assert.IsTrue(model.Any(u => u.Name.Contains("John")));
        }
    }
}
