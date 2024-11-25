using Moq;
using OOP_ecommerce.Models.Orders;
using OOP_ecommerce.Services;
using OOP_ecommerce.Interfaces;
using OOP_ecommerce.Models;
using OOP_ecommerce.Utils;
using System;
using System.Collections.Generic;
using Xunit;
using OOP_ecommerce.BaseModels;

namespace OOP_ecommerce.Tests
{
    public class PaymentProcessorTests
    {
        private readonly Mock<OrderManagment> _mockOrderManagment;
        private readonly Mock<IProcessPayment> _mockPaymentProcessor;
        private PaymentProcessor _paymentProcessor;
        private readonly Order _mockOrder;

        public PaymentProcessorTests()
        {
            // Mock de OrderManagment
            _mockOrderManagment = new Mock<OrderManagment>();

            // Mock de IProcessPayment
            _mockPaymentProcessor = new Mock<IProcessPayment>();

            // Creación de una orden simulada
            _mockOrder = new Order("Nueva", 1, new List<Product>())
            {
                OrderId = 1,
                IsAuthorized = true,
                TotalAmount = 100.0
            };

            // Configuración del mock para OrderManagment
            _mockOrderManagment.Setup(m => m.GetOrders()).Returns(new List<Order> { _mockOrder });

            // Creación de PaymentProcessor con el mock
            _paymentProcessor = new PaymentProcessor(_mockOrderManagment.Object);
        }

        [Fact]
        public void CreatePayment_ShouldCreatePayment_WhenOrderIsAuthorized()
        {
            // Act
            var payment = _paymentProcessor.CreatePayment(_mockOrder.OrderId, _mockPaymentProcessor.Object);

            // Assert
            Assert.NotNull(payment);
            Assert.Equal(1, payment.PaymentId); // El primer pago tiene ID 1 (basado en el contador de _payments)
            Assert.Equal(_mockOrder.OrderId, payment.OrderId);
            Assert.Equal(_mockOrder.TotalAmount, payment.Amount);
        }

        [Fact]
        public void CreatePayment_ShouldReturnNull_WhenOrderIsNotAuthorized()
        {
            // Arrange: Hacer que la orden no esté autorizada
            _mockOrder.IsAuthorized = false;

            // Act
            var payment = _paymentProcessor.CreatePayment(_mockOrder.OrderId, _mockPaymentProcessor.Object);

            // Assert
            Assert.Null(payment); // No se debe crear el pago si la orden no está autorizada
        }

        [Fact]
        public void ConfirmPayment_ShouldConfirmPayment_WhenPaymentIsSuccessful()
        {
            // Arrange
            var payment = new Payment(1, _mockOrder.OrderId, _mockOrder.TotalAmount, "CreditCard");

            // Agregar el pago a la lista de pagos de PaymentProcessor
            payment = _paymentProcessor.CreatePayment(_mockOrder.OrderId, _mockPaymentProcessor.Object);  // Esto agrega el pago al procesador

            // Simulamos la verificación del pago exitoso
            _mockPaymentProcessor.Setup(p => p.VerifyPayment(payment.PaymentId, payment.Amount, "tokenSesion")).Returns(true);

            // Act
            _paymentProcessor.ConfirmPayment(payment.PaymentId, _mockPaymentProcessor.Object);

            // Assert
            Assert.True(payment.IsSuccessfullPayment); // El pago debe ser exitoso
            Assert.True(_mockOrder.IsPaid);// La orden debe marcarse como pagada
        }

        [Fact]
        public void ConfirmPayment_ShouldNotConfirmPayment_WhenPaymentFails()
        {
            // Arrange
            var payment = new Payment(1, _mockOrder.OrderId, _mockOrder.TotalAmount, "CreditCard");

            // Simulamos que la verificación del pago falle
            _mockPaymentProcessor.Setup(p => p.VerifyPayment(payment.PaymentId, payment.Amount, "tokenSesion")).Returns(false);

            // Act
            _paymentProcessor.ConfirmPayment(payment.PaymentId, _mockPaymentProcessor.Object);

            // Assert
            Assert.False(payment.IsSuccessfullPayment); // El pago no debe ser exitoso
            Assert.False(_mockOrder.IsPaid); // La orden no debe marcarse como pagada
        }
    }
}
