using Moq;
using TodoServices.Service;
using TodoServices.Exceptions;
using TodoServices.Dto;
using TodoRepositories.IRepository;
using TodoRepositories.Entity;

namespace TodoService.Test
{
    public class TodoServiceTests
    {
        private readonly Mock<ITodoRepository> _mockRepository;
        private readonly TodoServices.Service.TodoService _todoService;

        public TodoServiceTests()
        {
            _mockRepository = new Mock<ITodoRepository>();
            _todoService = new TodoServices.Service.TodoService(_mockRepository.Object);
        }

        #region GetAllTodosAsync Tests

        [Fact]
        public async Task GetAllTodosAsync_WithMultipleTodos_ReturnsAllTodos()
        {
            // Arrange
            var todosEntities = new List<TodoItemEntity>
            {
                new TodoItemEntity { Id = Guid.NewGuid(), Title = "Task 1", isCompleted = false, CreatedDate = DateTime.Now },
                new TodoItemEntity { Id = Guid.NewGuid(), Title = "Task 2", isCompleted = true, CreatedDate = DateTime.Now },
                new TodoItemEntity { Id = Guid.NewGuid(), Title = "Task 3", isCompleted = false, CreatedDate = DateTime.Now }
            };

            _mockRepository.Setup(r => r.GetAllTodos()).ReturnsAsync(todosEntities);

            // Act
            var result = await _todoService.GetAllTodosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, item => Assert.NotNull(item));
            _mockRepository.Verify(r => r.GetAllTodos(), Times.Once);
        }

        [Fact]
        public async Task GetAllTodosAsync_WithEmptyList_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllTodos()).ReturnsAsync(new List<TodoItemEntity>());

            // Act
            var result = await _todoService.GetAllTodosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllTodosAsync_MapsPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var createdDate = new DateTime(2026, 4, 11, 10, 30, 0);
            var todosEntities = new List<TodoItemEntity>
            {
                new TodoItemEntity { Id = id, Title = "Test Task", isCompleted = true, CreatedDate = createdDate }
            };

            _mockRepository.Setup(r => r.GetAllTodos()).ReturnsAsync(todosEntities);

            // Act
            var result = await _todoService.GetAllTodosAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(id, result[0].Id);
            Assert.Equal("Test Task", result[0].Title);
            Assert.True(result[0].isCompleted);
            Assert.Equal(createdDate, result[0].CreatedDate);
        }

        #endregion

        #region GetTodoByIdAsync Tests

        [Fact]
        public async Task GetTodoByIdAsync_WithValidId_ReturnsTodo()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todoEntity = new TodoItemEntity { Id = id, Title = "Test Todo", isCompleted = false, CreatedDate = DateTime.Now };

            _mockRepository.Setup(r => r.GetTodoById(id)).ReturnsAsync(todoEntity);

            // Act
            var result = await _todoService.GetTodoByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Test Todo", result.Title);
            _mockRepository.Verify(r => r.GetTodoById(id), Times.Once);
        }

        [Fact]
        public async Task GetTodoByIdAsync_WithNonExistentId_ThrowsNotFoundException()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetTodoById(id)).ReturnsAsync((TodoItemEntity?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _todoService.GetTodoByIdAsync(id)
            );
            Assert.Contains("not found", exception.Message);
        }

        [Fact]
        public async Task GetTodoByIdAsync_WithEmptyGuid_ThrowsBadRequestException()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => _todoService.GetTodoByIdAsync(Guid.Empty)
            );
            Assert.Contains("cannot be empty", exception.Message);
        }

        [Fact]
        public async Task GetTodoByIdAsync_MapsPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var createdDate = new DateTime(2026, 4, 11, 10, 30, 0);
            var todoEntity = new TodoItemEntity { Id = id, Title = "Mapped Todo", isCompleted = true, CreatedDate = createdDate };

            _mockRepository.Setup(r => r.GetTodoById(id)).ReturnsAsync(todoEntity);

            // Act
            var result = await _todoService.GetTodoByIdAsync(id);

            // Assert
            Assert.Equal(id, result.Id);
            Assert.Equal("Mapped Todo", result.Title);
            Assert.True(result.isCompleted);
            Assert.Equal(createdDate, result.CreatedDate);
        }

        #endregion

        #region CreateTodoItemAsync Tests

        [Fact]
        public async Task CreateTodoItemAsync_WithValidTodo_CreatesSuccessfully()
        {
            // Arrange
            var todoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Title = "New Task",
                isCompleted = false,
                CreatedDate = DateTime.Now
            };

            _mockRepository.Setup(r => r.CreateTodoItemAsync(It.IsAny<TodoItemEntity>())).Returns(Task.CompletedTask);

            // Act
            await _todoService.CreateTodoItemAsync(todoItem);

            // Assert
            _mockRepository.Verify(r => r.CreateTodoItemAsync(It.Is<TodoItemEntity>(
                e => e.Id == todoItem.Id && e.Title == todoItem.Title && e.isCompleted == todoItem.isCompleted
            )), Times.Once);
        }

        [Fact]
        public async Task CreateTodoItemAsync_WithNullTodo_ThrowsBadRequestException()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => _todoService.CreateTodoItemAsync(null)
            );
            Assert.Contains("cannot be null", exception.Message);
        }

        [Fact]
        public async Task CreateTodoItemAsync_WithEmptyTitle_ThrowsBadRequestException()
        {
            // Arrange
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Title = "", isCompleted = false };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => _todoService.CreateTodoItemAsync(todoItem)
            );
            Assert.Contains("required", exception.Message);
        }

        [Fact]
        public async Task CreateTodoItemAsync_WithNullTitle_ThrowsBadRequestException()
        {
            // Arrange
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Title = null, isCompleted = false };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => _todoService.CreateTodoItemAsync(todoItem)
            );
            Assert.Contains("required", exception.Message);
        }

        [Fact]
        public async Task CreateTodoItemAsync_WithWhitespaceTitle_ThrowsBadRequestException()
        {
            // Arrange
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Title = "   ", isCompleted = false };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => _todoService.CreateTodoItemAsync(todoItem)
            );
            Assert.Contains("required", exception.Message);
        }

        [Fact]
        public async Task CreateTodoItemAsync_WithTitleExceeding200Chars_ThrowsBadRequestException()
        {
            // Arrange
            var longTitle = new string('a', 201);
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Title = longTitle, isCompleted = false };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => _todoService.CreateTodoItemAsync(todoItem)
            );
            Assert.Contains("exceed", exception.Message);
        }

        [Fact]
        public async Task CreateTodoItemAsync_WithMaxLengthTitle_CreatesSuccessfully()
        {
            // Arrange
            var maxLengthTitle = new string('a', 200);
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Title = maxLengthTitle, isCompleted = false };

            _mockRepository.Setup(r => r.CreateTodoItemAsync(It.IsAny<TodoItemEntity>())).Returns(Task.CompletedTask);

            // Act
            await _todoService.CreateTodoItemAsync(todoItem);

            // Assert
            _mockRepository.Verify(r => r.CreateTodoItemAsync(It.IsAny<TodoItemEntity>()), Times.Once);
        }

        [Fact]
        public async Task CreateTodoItemAsync_TrimsTitle()
        {
            // Arrange
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Title = "  Task  ", isCompleted = false };

            _mockRepository.Setup(r => r.CreateTodoItemAsync(It.IsAny<TodoItemEntity>())).Returns(Task.CompletedTask);

            // Act
            await _todoService.CreateTodoItemAsync(todoItem);

            // Assert
            _mockRepository.Verify(r => r.CreateTodoItemAsync(It.Is<TodoItemEntity>(
                e => e.Title == "Task"
            )), Times.Once);
        }

        #endregion

        #region UpdateTodoItemAsync Tests

        [Fact]
        public async Task UpdateTodoItemAsync_WithValidData_UpdatesSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todoItem = new TodoItem
            {
                Id = id,
                Title = "Updated Task",
                isCompleted = true,
                CreatedDate = DateTime.Now
            };

            _mockRepository.Setup(r => r.UpdateTodoItemAsync(It.IsAny<TodoItemEntity>())).Returns(Task.CompletedTask);

            // Act
            await _todoService.UpdateTodoItemAsync(id, todoItem);

            // Assert
            _mockRepository.Verify(r => r.UpdateTodoItemAsync(It.Is<TodoItemEntity>(
                e => e.Id == id && e.Title == todoItem.Title && e.isCompleted == todoItem.isCompleted
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateTodoItemAsync_WithNullTodo_ThrowsBadRequestException()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => _todoService.UpdateTodoItemAsync(id, null)
            );
            Assert.Contains("required", exception.Message);
        }

        [Fact]
        public async Task UpdateTodoItemAsync_WithEmptyGuid_ThrowsBadRequestException()
        {
            // Arrange
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Title = "Task", isCompleted = false };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => _todoService.UpdateTodoItemAsync(Guid.Empty, todoItem)
            );
            Assert.Contains("cannot be empty", exception.Message);
        }

        [Fact]
        public async Task UpdateTodoItemAsync_WithInvalidTitle_ThrowsBadRequestException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todoItem = new TodoItem { Id = id, Title = "", isCompleted = false };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => _todoService.UpdateTodoItemAsync(id, todoItem)
            );
            Assert.Contains("required", exception.Message);
        }

        [Fact]
        public async Task UpdateTodoItemAsync_WithTitleExceeding200Chars_ThrowsBadRequestException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var longTitle = new string('a', 201);
            var todoItem = new TodoItem { Id = id, Title = longTitle, isCompleted = false };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => _todoService.UpdateTodoItemAsync(id, todoItem)
            );
            Assert.Contains("exceed", exception.Message);
        }

        #endregion

        #region DeleteTodoItemAsync Tests

        [Fact]
        public async Task DeleteTodoItemAsync_WithValidId_DeletesSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todoEntity = new TodoItemEntity { Id = id, Title = "Task to Delete", isCompleted = false };

            _mockRepository.Setup(r => r.GetTodoById(id)).ReturnsAsync(todoEntity);
            _mockRepository.Setup(r => r.DeleteTodoItemAsync(id)).Returns(Task.CompletedTask);

            // Act
            await _todoService.DeleteTodoItemAsync(id);

            // Assert
            _mockRepository.Verify(r => r.GetTodoById(id), Times.Once);
            _mockRepository.Verify(r => r.DeleteTodoItemAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteTodoItemAsync_WithNonExistentId_ThrowsNotFoundException()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetTodoById(id)).ReturnsAsync((TodoItemEntity?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _todoService.DeleteTodoItemAsync(id)
            );
            Assert.Contains("not found", exception.Message);
            _mockRepository.Verify(r => r.DeleteTodoItemAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTodoItemAsync_WithEmptyGuid_ThrowsBadRequestException()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(
                () => _todoService.DeleteTodoItemAsync(Guid.Empty)
            );
            Assert.Contains("cannot be empty", exception.Message);
        }

        #endregion
    }
}
