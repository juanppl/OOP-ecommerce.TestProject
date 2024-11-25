using Moq;
using OOP_ecommerce.Models.Products;
using OOP_ecommerce.Services;
using OOP_ecommerce.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;
using OOP_ecommerce.Utils;

namespace OOP_ecommerce.Tests
{
    public class ProductsManagmentProcessorTests
    {
        private Mock<IProductFactory> _mockProductFactory;
        private Mock<LogManager> _mockLogger;
        private ProductsManagmentProcessor _productProcessor;

        public ProductsManagmentProcessorTests()
        {
            // Mock del IProductFactory
            _mockProductFactory = new Mock<IProductFactory>();

            // Mock del logger
            _mockLogger = new Mock<LogManager>();

            // Creamos el objeto de ProductsManagmentProcessor
            _productProcessor = new ProductsManagmentProcessor(_mockProductFactory.Object);
        }

        [Fact]
        public void CreateProduct_ShouldReturnDesktop_WhenProductTypeIsDesktop()
        {
            // Arrange
            var productType = ProductType.Desktop;
            var fullName = "Desktop";
            var displayName = "Desktop 24";
            var description = "High performance desktop.";
            var price = 1200.00;
            var isActive = true;
            var creationDate = DateTime.Now;
            var expireDate = DateTime.Now.AddYears(1);
            var availableQty = 50;
            var isDeleted = false;
            var timesViewed = 100;
            var timesBuyed = 20;
            var categoryId = 1;
            bool extraInfo1 = false;
            string extraInfo2 = "";

            var desktopProduct = new Desktop(fullName, displayName, description, price, isActive, creationDate, expireDate, availableQty, isDeleted, timesViewed, timesBuyed, categoryId, extraInfo1, extraInfo2);

            // Setup para mockear la creación del producto Desktop
            _mockProductFactory.Setup(f => f.CreateDesktop(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<bool>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(desktopProduct);

            // Act
            var result = _productProcessor.CreateProduct(productType, fullName, displayName, description, price, isActive, creationDate, expireDate, availableQty, isDeleted, timesViewed, timesBuyed, categoryId, extraInfo1, extraInfo2);

            // Assert
            Assert.IsType<Desktop>(result);
            Assert.Equal(fullName, result.FullName);
        }

        [Fact]
        public void AddProduct_ShouldAddProduct_WhenProductDoesNotExistInInventory()
        {
            // Arrange
            var product = new Desktop("Desktop", "Desktop 24", "High performance desktop.", 1200.00, true, DateTime.Now, DateTime.Now.AddYears(1), 50, false, 100, 20, 1, false, "");

            // Act
            _productProcessor.AddProduct(product);

            // Assert
            Assert.Contains(product, _productProcessor.Inventory);
        }

        [Fact]
        public void DeleteProduct_ShouldRemoveProduct_WhenProductExistsInInventory()
        {
            // Arrange
            var product = new Desktop("Desktop", "Desktop 24", "High performance desktop.", 1200.00, true, DateTime.Now, DateTime.Now.AddYears(1), 50, false, 100, 20, 1, false, "");
            _productProcessor.AddProduct(product); // Aseguramos que el producto esté en el inventario

            // Act
            _productProcessor.DeleteProduct(product.ProductId);

            // Assert
            Assert.DoesNotContain(product, _productProcessor.Inventory);
        }

        [Fact]
        public void UpdateStock_ShouldUpdateStock_WhenProductExistsInInventory()
        {
            // Arrange
            var product = new Desktop("Desktop", "Desktop 24", "High performance desktop.", 1200.00, true, DateTime.Now, DateTime.Now.AddYears(1), 50, false, 100, 20, 1, false, "");
            _productProcessor.AddProduct(product); // Añadimos el producto al inventario

            var stockToAdd = 20;

            // Act
            _productProcessor.UpdateStock(product.ProductId, stockToAdd);

            // Assert
            Assert.Equal(70, product.AvailableQty);
        }
    }
}
