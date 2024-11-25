using OOP_ecommerce.Services;
using OOP_ecommerce.Models;
using OOP_ecommerce.Models.Products;
using OOP_ecommerce.Models.Users;
using System;
using System.Collections.Generic;
using Xunit;
using OOP_ecommerce.BaseModels;

namespace OOP_ecommerce.Tests
{
    public class OrderManagmentTests
    {
        private readonly OrderManagment _orderManagment;
        private AdminUser _adminUser;
        private readonly Desktop _mockProduct;

        public OrderManagmentTests()
        {
            _orderManagment = new OrderManagment();

            // Instanciamos un AdminUser real para la prueba
            _adminUser = new AdminUser("John", "Doe", "john.doe@example.com", "password", "johndoe", "bio", true, new List<string>(), new List<int>())
            {
                UserId = 1 // Asignamos explícitamente un UserId para asegurar que el usuario tenga un identificador único
            };

            // Creamos un producto de tipo Desktop real
            _mockProduct = new Desktop("Desktop Model 1", "Desktop 1", "A powerful desktop", 1200.00, true, DateTime.Now, DateTime.Now.AddYears(1), 10, false, 0, 0, 1, true, "Full Tower");
        }

        [Fact]
        public void CreateOrder_ShouldCreateNewOrder_WhenValidUserAndProducts()
        {
            // Arrange
            var userId = _adminUser.UserId; // Usamos el UserId del adminUser
            var products = new List<Product> { _mockProduct };

            // Agregar el usuario al sistema
            _orderManagment.AgregarUsuario(_adminUser);

            // Act
            var order = _orderManagment.CreateOrder(userId, products);

            // Assert
            Assert.NotNull(order);
            Assert.Equal("Nueva", order.Status);
            Assert.Equal(userId, order.UserId);
            Assert.Contains(_mockProduct, order.Products);
            Assert.Contains(order, _orderManagment.Orders);
        }

        [Fact]
        public void AutorizarOrden_ShouldReturnFalse_WhenOrderNotFound()
        {
            // Arrange
            var orderId = 999;
            var tokenSesion = "valid_token";

            // Act
            var result = _orderManagment.AutorizarOrden(orderId, tokenSesion);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AutorizarOrden_ShouldReturnFalse_WhenUserNotAuthenticated()
        {
            // Arrange
            var userId = _adminUser.UserId; // Usamos el UserId del adminUser
            var products = new List<Product> { _mockProduct };
            var order = _orderManagment.CreateOrder(userId, products); // Create order
            _orderManagment.AgregarUsuario(_adminUser); // Add user to system

            // Simular un usuario no autenticado
            _adminUser = new AdminUser("John", "Doe", "john.doe@example.com", "password", "johndoe", "bio", false, new List<string>(), new List<int>())
            {
                UserId = userId
            };

            // Act
            var result = _orderManagment.AutorizarOrden(order.OrderId, "invalid_token");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AutorizarOrden_ShouldReturnTrue_WhenOrderAndUserAreValid()
        {
            // Arrange
            var userId = _adminUser.UserId; // Usamos el UserId del adminUser
            var products = new List<Product> { _mockProduct };
            var order = _orderManagment.CreateOrder(userId, products); // Create order
            // Simular un usuario autenticado con el token correcto
            _adminUser = new AdminUser("John", "Doe", "john.doe@example.com", "password", "johndoe", "bio", true, new List<string>(), new List<int>())
            {
                UserId = userId,
            };
            _adminUser.setTokenSession("valid_token");
            _orderManagment.AgregarUsuario(_adminUser); // Add user to system


            // Act
            var result = _orderManagment.AutorizarOrden(order.OrderId, "valid_token");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AgregarUsuario_ShouldAddUserToManagement()
        {
            // Arrange
            var user = new AdminUser("Jane", "Smith", "jane.smith@example.com", "password", "janesmith", "bio", true, new List<string>(), new List<int>())
            {
                UserId = 2 // Asegúrate de asignar un UserId único
            };

            // Act
            _orderManagment.AgregarUsuario(user);

            // Assert
            Assert.Contains(user.UserId, _orderManagment._users.Keys);
        }
    }
}
