using System;
using Xunit;

namespace Skunked.UnitTest.Exceptions
{
    public class InvalidCribbageOperationExceptionTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(42)]
        public void Constructor_SetsOperationProperty(int rawValue)
        {
            // Arrange
            var op = (InvalidCribbageOperation)rawValue;

            // Act
            var ex = new InvalidCribbageOperationException(op);

            // Assert
            Assert.Equal(op, ex.Operation);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(123)]
        public void Constructor_SetsMessage_FromEnumToString(int rawValue)
        {
            // Arrange
            var op = (InvalidCribbageOperation)rawValue;

            // Act
            var ex = new InvalidCribbageOperationException(op);

            // Assert
            // Message should be operation.ToString() as passed to the base constructor.
            Assert.Equal(op.ToString(), ex.Message);
        }
    }
}
