using TodoServices.ValueObjects;
using TodoServices.Exceptions;

namespace TodoService.Test
{
    public class TodoIdTests
    {
        #region TodoId.Create Tests

        [Fact]
        public void TodoId_Create_WithValidGuid_ReturnsTodoId()
        {
            // Arrange
            var validGuid = Guid.NewGuid();

            // Act
            var result = TodoId.Create(validGuid);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(validGuid, result.Value);
        }

        [Fact]
        public void TodoId_Create_WithEmptyGuid_ThrowsBadRequestException()
        {
            // Act & Assert
            var exception = Assert.Throws<BadRequestException>(
                () => TodoId.Create(Guid.Empty)
            );
            Assert.Contains("cannot be empty", exception.Message);
        }

        #endregion

        #region TodoId.Equals Tests

        [Fact]
        public void TodoId_Equals_WithSameId_ReturnsTrue()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var todoId1 = TodoId.Create(guid);
            var todoId2 = TodoId.Create(guid);

            // Act
            var result = todoId1.Equals(todoId2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TodoId_Equals_WithDifferentId_ReturnsFalse()
        {
            // Arrange
            var todoId1 = TodoId.Create(Guid.NewGuid());
            var todoId2 = TodoId.Create(Guid.NewGuid());

            // Act
            var result = todoId1.Equals(todoId2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TodoId_Equals_WithNonTodoIdObject_ReturnsFalse()
        {
            // Arrange
            var todoId = TodoId.Create(Guid.NewGuid());

            // Act
            var result = todoId.Equals("not a todo id");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TodoId_Equals_WithNull_ReturnsFalse()
        {
            // Arrange
            var todoId = TodoId.Create(Guid.NewGuid());

            // Act
            var result = todoId.Equals(null);

            // Assert
            Assert.False(result);
        }

        #endregion
    }

    public class TodoItemTitleTests
    {
        #region TodoItemTitle.Create Tests

        [Fact]
        public void TodoItemTitle_Create_WithValidTitle_ReturnsTodoItemTitle()
        {
            // Arrange
            var validTitle = "Valid Task Title";

            // Act
            var result = TodoItemTitle.Create(validTitle);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(validTitle, result.Value);
        }

        [Fact]
        public void TodoItemTitle_Create_WithEmptyString_ThrowsBadRequestException()
        {
            // Act & Assert
            var exception = Assert.Throws<BadRequestException>(
                () => TodoItemTitle.Create("")
            );
            Assert.Contains("required", exception.Message);
        }

        [Fact]
        public void TodoItemTitle_Create_WithNullTitle_ThrowsBadRequestException()
        {
            // Act & Assert
            var exception = Assert.Throws<BadRequestException>(
                () => TodoItemTitle.Create(null)
            );
            Assert.Contains("required", exception.Message);
        }

        [Fact]
        public void TodoItemTitle_Create_WithOnlyWhitespace_ThrowsBadRequestException()
        {
            // Act & Assert
            var exception = Assert.Throws<BadRequestException>(
                () => TodoItemTitle.Create("   \t\n   ")
            );
            Assert.Contains("required", exception.Message);
        }

        [Fact]
        public void TodoItemTitle_Create_WithSingleCharacter_ReturnsSuccess()
        {
            // Arrange
            var singleChar = "A";

            // Act
            var result = TodoItemTitle.Create(singleChar);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(singleChar, result.Value);
        }

        [Fact]
        public void TodoItemTitle_Create_With200Characters_ReturnsSuccess()
        {
            // Arrange
            var maxTitle = new string('a', 200);

            // Act
            var result = TodoItemTitle.Create(maxTitle);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Value.Length);
        }

        [Fact]
        public void TodoItemTitle_Create_With201Characters_ThrowsBadRequestException()
        {
            // Arrange
            var overMaxTitle = new string('a', 201);

            // Act & Assert
            var exception = Assert.Throws<BadRequestException>(
                () => TodoItemTitle.Create(overMaxTitle)
            );
            Assert.Contains("exceed", exception.Message);
            Assert.Contains("200", exception.Message);
        }

        [Fact]
        public void TodoItemTitle_Create_TrimsTitleCorrectly()
        {
            // Arrange
            var titleWithSpaces = "  Trimmed Title  ";

            // Act
            var result = TodoItemTitle.Create(titleWithSpaces);

            // Assert
            Assert.Equal("Trimmed Title", result.Value);
        }

        #endregion

        #region TodoItemTitle.Equals Tests

        [Fact]
        public void TodoItemTitle_Equals_WithSameValue_ReturnsTrue()
        {
            // Arrange
            var title1 = TodoItemTitle.Create("Same Title");
            var title2 = TodoItemTitle.Create("Same Title");

            // Act
            var result = title1.Equals(title2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TodoItemTitle_Equals_WithDifferentValue_ReturnsFalse()
        {
            // Arrange
            var title1 = TodoItemTitle.Create("Title 1");
            var title2 = TodoItemTitle.Create("Title 2");

            // Act
            var result = title1.Equals(title2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TodoItemTitle_Equals_WithNonTodoItemTitleObject_ReturnsFalse()
        {
            // Arrange
            var title = TodoItemTitle.Create("Test Title");

            // Act
            var result = title.Equals("Test Title");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TodoItemTitle_Equals_WithNull_ReturnsFalse()
        {
            // Arrange
            var title = TodoItemTitle.Create("Test Title");

            // Act
            var result = title.Equals(null);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region TodoItemTitle Implicit Conversion Tests

        [Fact]
        public void TodoItemTitle_ImplicitConversion_ConvertsTitleToString()
        {
            // Arrange
            var todoItemTitle = TodoItemTitle.Create("Convert Me");

            // Act
            string result = todoItemTitle;

            // Assert
            Assert.Equal("Convert Me", result);
            Assert.IsType<string>(result);
        }

        #endregion
    }

    public class ExceptionTests
    {
        #region BadRequestException Tests

        [Fact]
        public void BadRequestException_WithMessage_CanBeThrown()
        {
            // Arrange
            var message = "This is a bad request";

            // Act & Assert
            var exception = new BadRequestException(message);
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void BadRequestException_InheritsFromException()
        {
            // Arrange
            var message = "Test exception";
            var exception = new BadRequestException(message);

            // Act & Assert
            Assert.IsAssignableFrom<Exception>(exception);
        }

        #endregion

        #region NotFoundException Tests

        [Fact]
        public void NotFoundException_WithMessage_CanBeThrown()
        {
            // Arrange
            var message = "Item not found";

            // Act & Assert
            var exception = new NotFoundException(message);
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void NotFoundException_InheritsFromException()
        {
            // Arrange
            var message = "Test exception";
            var exception = new NotFoundException(message);

            // Act & Assert
            Assert.IsAssignableFrom<Exception>(exception);
        }

        #endregion
    }
}



