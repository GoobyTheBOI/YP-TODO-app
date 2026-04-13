using TodoApi.Middleware;
using TodoServices.Exceptions;
using Xunit;

namespace TodoApi.Test
{
    public class GlobalExceptionHandlerTests
    {
        [Fact]
        public void MapException_NotFoundException_Returns404()
        {
            // Arrange
            var exception = new NotFoundException("Item not found");

            // Act
            var (statusCode, title) = GlobalExceptionHandler.TestableMapException(exception);

            // Assert
            Assert.Equal(404, statusCode);
            Assert.Equal("Item not found", title);
        }

        [Fact]
        public void MapException_BadRequestException_Returns400()
        {
            // Arrange
            var exception = new BadRequestException("Invalid input");

            // Act
            var (statusCode, title) = GlobalExceptionHandler.TestableMapException(exception);

            // Assert
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public void MapException_UnknownException_Returns500()
        {
            // Arrange
            var exception = new InvalidOperationException("Something broke");

            // Act
            var (statusCode, title) = GlobalExceptionHandler.TestableMapException(exception);

            // Assert
            Assert.Equal(500, statusCode);
            Assert.Equal("An error occurred", title);
        }
    }
}
