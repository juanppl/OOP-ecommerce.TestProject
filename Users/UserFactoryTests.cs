using OOP_ecommerce.Services.Users;
using OOP_ecommerce.Models.Users;
using System;
using System.Collections.Generic;
using Xunit;

namespace OOP_ecommerce.Tests
{
    public class UserFactoryTests
    {
        [Fact]
        public void CreateUser_ShouldReturnAdminUser_WhenRoleIsAdmin()
        {
            // Arrange
            var role = "admin";
            var firstName = "John";
            var lastName = "Doe";
            var email = "john.doe@example.com";
            var password = "password";
            var userName = "johndoe";
            var bio = "Bio of John";
            var isActive = true;
            var permissions = new List<string> { "Permission1", "Permission2" };
            var assignedDepartments = new List<int> { 1, 2 };

            // Act
            var user = UserFactory.CreateUser(role, firstName, lastName, email, password, userName, bio, isActive, permissions, assignedDepartments);

            // Assert
            Assert.IsType<AdminUser>(user);
            var adminUser = (AdminUser)user;
            Assert.Equal(firstName, adminUser.FirstName);
            Assert.Equal(lastName, adminUser.LastName);
            Assert.Equal(email, adminUser.Email);
            Assert.Equal(userName, adminUser.UserName);
            Assert.Equal(bio, adminUser.Bio);
            Assert.True(adminUser.IsActive);
            Assert.Equal(permissions, adminUser.Permissions);
            Assert.Equal(assignedDepartments, adminUser.AssignedDepartments);
        }

        [Fact]
        public void CreateUser_ShouldReturnSuperAdminUser_WhenRoleIsSuperAdmin()
        {
            // Arrange
            var role = "superadmin";
            var firstName = "Jane";
            var lastName = "Smith";
            var email = "jane.smith@example.com";
            var password = "password";
            var userName = "janesmith";
            var bio = "Bio of Jane";
            var isActive = true;
            var permissions = new List<string> { "Permission1", "Permission2" };

            // Act
            var user = UserFactory.CreateUser(role, firstName, lastName, email, password, userName, bio, isActive, permissions);

            // Assert
            Assert.IsType<SuperAdminUser>(user);
            var superAdminUser = (SuperAdminUser)user;
            Assert.Equal(firstName, superAdminUser.FirstName);
            Assert.Equal(lastName, superAdminUser.LastName);
            Assert.Equal(email, superAdminUser.Email);
            Assert.Equal(userName, superAdminUser.UserName);
            Assert.Equal(bio, superAdminUser.Bio);
            Assert.True(superAdminUser.IsActive);
        }

        [Fact]
        public void CreateUser_ShouldReturnRegularUser_WhenRoleIsRegular()
        {
            // Arrange
            var role = "regular";
            var firstName = "Bob";
            var lastName = "Johnson";
            var email = "bob.johnson@example.com";
            var password = "password";
            var userName = "bobjohnson";
            var bio = "Bio of Bob";
            var isActive = true;

            // Act
            var user = UserFactory.CreateUser(role, firstName, lastName, email, password, userName, bio, isActive);

            // Assert
            Assert.IsType<RegularUser>(user);
            var regularUser = (RegularUser)user;
            Assert.Equal(firstName, regularUser.FirstName);
            Assert.Equal(lastName, regularUser.LastName);
            Assert.Equal(email, regularUser.Email);
            Assert.Equal(userName, regularUser.UserName);
            Assert.Equal(bio, regularUser.Bio);
            Assert.True(regularUser.IsActive);
        }

        [Fact]
        public void CreateUser_ShouldReturnVipUser_WhenRoleIsVip()
        {
            // Arrange
            var role = "vip";
            var firstName = "Alice";
            var lastName = "Williams";
            var email = "alice.williams@example.com";
            var password = "password";
            var userName = "alicewilliams";
            var bio = "Bio of Alice";
            var isActive = true;

            // Act
            var user = UserFactory.CreateUser(role, firstName, lastName, email, password, userName, bio, isActive);

            // Assert
            Assert.IsType<VipUser>(user);
            var vipUser = (VipUser)user;
            Assert.Equal(firstName, vipUser.FirstName);
            Assert.Equal(lastName, vipUser.LastName);
            Assert.Equal(email, vipUser.Email);
            Assert.Equal(userName, vipUser.UserName);
            Assert.Equal(bio, vipUser.Bio);
            Assert.True(vipUser.IsActive);
        }
    }
}
