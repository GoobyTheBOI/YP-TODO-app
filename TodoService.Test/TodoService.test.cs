namespace TodoService.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Create_WithValidTitle_ReturnsConfirmationMessage()
        {
            // Arrange
            TodoService service = new();

            // Act
            var result = service.Create("Buy milk");

            // Assert
            Assert.Equal("Todo created: Buy milk", result);
        }

    }
}
