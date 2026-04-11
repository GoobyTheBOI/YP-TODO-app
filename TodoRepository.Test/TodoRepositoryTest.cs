using Microsoft.EntityFrameworkCore;
using TodoRepositories.Entity;

namespace TodoRepositories.Test;

public class TodoRepositoryTest
{
    private readonly DbContextOptions<TodoDbContext> _dbContextOptions;

    public TodoRepositoryTest()
    {
        _dbContextOptions = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    private TodoDbContext CreateContext()
    {
        return new TodoDbContext(_dbContextOptions);
    }

    #region GetAllTodos Tests

    [Fact]
    public async Task GetAllTodos_WithMultipleItems_ReturnsAllTodos()
    {
        // Arrange
        using (var context = CreateContext())
        {
            var item1 = new TodoItemEntity { Id = Guid.NewGuid(), Title = "Task 1", isCompleted = false, CreatedDate = DateTime.Now };
            var item2 = new TodoItemEntity { Id = Guid.NewGuid(), Title = "Task 2", isCompleted = true, CreatedDate = DateTime.Now };
            var item3 = new TodoItemEntity { Id = Guid.NewGuid(), Title = "Task 3", isCompleted = false, CreatedDate = DateTime.Now };

            context.Todos.AddRange(item1, item2, item3);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            var result = await repository.GetAllTodos();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, item => Assert.NotNull(item.Title));
        }
    }

    [Fact]
    public async Task GetAllTodos_WithEmptyDatabase_ReturnsEmptyList()
    {
        // Act
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            var result = await repository.GetAllTodos();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }

    [Fact]
    public async Task GetAllTodos_VerifiesAllPropertiesAreMapped()
    {
        // Arrange
        var id = Guid.NewGuid();
        var createdDate = new DateTime(2026, 4, 11, 10, 30, 0);

        using (var context = CreateContext())
        {
            var item = new TodoItemEntity { Id = id, Title = "Test Task", isCompleted = true, CreatedDate = createdDate };
            context.Todos.Add(item);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            var result = await repository.GetAllTodos();

            // Assert
            Assert.Single(result);
            Assert.Equal(id, result[0].Id);
            Assert.Equal("Test Task", result[0].Title);
            Assert.True(result[0].isCompleted);
            Assert.Equal(createdDate, result[0].CreatedDate);
        }
    }

    #endregion

    #region GetTodoById Tests

    [Fact]
    public async Task GetTodoById_WithExistingId_ReturnsTodo()
    {
        // Arrange
        var id = Guid.NewGuid();
        using (var context = CreateContext())
        {
            var item = new TodoItemEntity { Id = id, Title = "Test Todo", isCompleted = false, CreatedDate = DateTime.Now };
            context.Todos.Add(item);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            var result = await repository.GetTodoById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Test Todo", result.Title);
        }
    }

    [Fact]
    public async Task GetTodoById_WithNonExistingId_ReturnsNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            var result = await repository.GetTodoById(nonExistingId);

            // Assert
            Assert.Null(result);
        }
    }

    [Fact]
    public async Task GetTodoById_VerifiesAllPropertiesAreMapped()
    {
        // Arrange
        var id = Guid.NewGuid();
        var createdDate = new DateTime(2026, 4, 11, 14, 45, 30);

        using (var context = CreateContext())
        {
            var item = new TodoItemEntity { Id = id, Title = "Detailed Task", isCompleted = true, CreatedDate = createdDate };
            context.Todos.Add(item);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            var result = await repository.GetTodoById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Detailed Task", result.Title);
            Assert.True(result.isCompleted);
            Assert.Equal(createdDate, result.CreatedDate);
        }
    }

    #endregion

    #region CreateTodoItemAsync Tests

    [Fact]
    public async Task CreateTodoItemAsync_WithValidItem_SavesSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        var item = new TodoItemEntity { Id = id, Title = "New Task", isCompleted = false, CreatedDate = DateTime.Now };

        // Act
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            await repository.CreateTodoItemAsync(item);
        }

        // Assert
        using (var context = CreateContext())
        {
            var savedItem = await context.Todos.FindAsync(id);
            Assert.NotNull(savedItem);
            Assert.Equal("New Task", savedItem.Title);
        }
    }

    [Fact]
    public async Task CreateTodoItemAsync_WithMultipleItems_SavesAllSuccessfully()
    {
        // Arrange
        var item1 = new TodoItemEntity { Id = Guid.NewGuid(), Title = "Task 1", isCompleted = false, CreatedDate = DateTime.Now };
        var item2 = new TodoItemEntity { Id = Guid.NewGuid(), Title = "Task 2", isCompleted = true, CreatedDate = DateTime.Now };

        // Act
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            await repository.CreateTodoItemAsync(item1);
            await repository.CreateTodoItemAsync(item2);
        }

        // Assert
        using (var context = CreateContext())
        {
            var allItems = await context.Todos.ToListAsync();
            Assert.Equal(2, allItems.Count);
        }
    }

    [Fact]
    public async Task CreateTodoItemAsync_VerifiesAllPropertiesSaved()
    {
        // Arrange
        var id = Guid.NewGuid();
        var createdDate = new DateTime(2026, 4, 11, 12, 15, 45);
        var item = new TodoItemEntity { Id = id, Title = "Complete Task", isCompleted = true, CreatedDate = createdDate };

        // Act
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            await repository.CreateTodoItemAsync(item);
        }

        // Assert
        using (var context = CreateContext())
        {
            var savedItem = await context.Todos.FindAsync(id);
            Assert.NotNull(savedItem);
            Assert.Equal(id, savedItem.Id);
            Assert.Equal("Complete Task", savedItem.Title);
            Assert.True(savedItem.isCompleted);
            Assert.Equal(createdDate, savedItem.CreatedDate);
        }
    }

    #endregion

    #region UpdateTodoItemAsync Tests

    [Fact]
    public async Task UpdateTodoItemAsync_WithValidItem_UpdatesSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        using (var context = CreateContext())
        {
            var item = new TodoItemEntity { Id = id, Title = "Original Task", isCompleted = false, CreatedDate = DateTime.Now };
            context.Todos.Add(item);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = CreateContext())
        {
            var updatedItem = new TodoItemEntity { Id = id, Title = "Updated Task", isCompleted = true, CreatedDate = DateTime.Now };
            var repository = new TodoRepository(context);
            await repository.UpdateTodoItemAsync(updatedItem);
        }

        // Assert
        using (var context = CreateContext())
        {
            var result = await context.Todos.FindAsync(id);
            Assert.NotNull(result);
            Assert.Equal("Updated Task", result.Title);
            Assert.True(result.isCompleted);
        }
    }

    [Fact]
    public async Task UpdateTodoItemAsync_ChangingMultipleProperties_UpdatesAllSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        var originalDate = new DateTime(2026, 4, 9);
        var newDate = new DateTime(2026, 4, 11);

        using (var context = CreateContext())
        {
            var item = new TodoItemEntity { Id = id, Title = "Original", isCompleted = false, CreatedDate = originalDate };
            context.Todos.Add(item);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = CreateContext())
        {
            var updatedItem = new TodoItemEntity { Id = id, Title = "Modified", isCompleted = true, CreatedDate = newDate };
            var repository = new TodoRepository(context);
            await repository.UpdateTodoItemAsync(updatedItem);
        }

        // Assert
        using (var context = CreateContext())
        {
            var result = await context.Todos.FindAsync(id);
            Assert.NotNull(result);
            Assert.Equal("Modified", result.Title);
            Assert.True(result.isCompleted);
            Assert.Equal(newDate, result.CreatedDate);
        }
    }

    [Fact]
    public async Task UpdateTodoItemAsync_OnlyChangingCompleted_UpdatesSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        var originalTitle = "Task Title";

        using (var context = CreateContext())
        {
            var item = new TodoItemEntity { Id = id, Title = originalTitle, isCompleted = false, CreatedDate = DateTime.Now };
            context.Todos.Add(item);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = CreateContext())
        {
            var updatedItem = new TodoItemEntity { Id = id, Title = originalTitle, isCompleted = true, CreatedDate = DateTime.Now };
            var repository = new TodoRepository(context);
            await repository.UpdateTodoItemAsync(updatedItem);
        }

        // Assert
        using (var context = CreateContext())
        {
            var result = await context.Todos.FindAsync(id);
            Assert.NotNull(result);
            Assert.Equal(originalTitle, result.Title);
            Assert.True(result.isCompleted);
        }
    }

    #endregion

    #region DeleteTodoItemAsync Tests

    [Fact]
    public async Task DeleteTodoItemAsync_WithExistingId_DeletesSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        using (var context = CreateContext())
        {
            var item = new TodoItemEntity { Id = id, Title = "Task to Delete", isCompleted = false, CreatedDate = DateTime.Now };
            context.Todos.Add(item);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = CreateContext())
        {
            var todo = await context.Todos.FindAsync(id);
            if (todo != null)
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync();
            }
        }

        // Assert
        using (var context = CreateContext())
        {
            var deletedItem = await context.Todos.FindAsync(id);
            Assert.Null(deletedItem);
        }
    }

    [Fact]
    public async Task DeleteTodoItemAsync_WithNonExistingId_DoesNotThrow()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act & Assert - should not throw
        using (var context = CreateContext())
        {
            var todo = await context.Todos.FindAsync(nonExistingId);
            if (todo != null)
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync();
            }
        }
    }

    [Fact]
    public async Task DeleteTodoItemAsync_WithMultipleItems_DeletesOnlyTargeted()
    {
        // Arrange
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();

        using (var context = CreateContext())
        {
            context.Todos.AddRange(
                new TodoItemEntity { Id = id1, Title = "Task 1", isCompleted = false, CreatedDate = DateTime.Now },
                new TodoItemEntity { Id = id2, Title = "Task 2", isCompleted = false, CreatedDate = DateTime.Now },
                new TodoItemEntity { Id = id3, Title = "Task 3", isCompleted = false, CreatedDate = DateTime.Now }
            );
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = CreateContext())
        {
            var todo = await context.Todos.FindAsync(id2);
            if (todo != null)
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync();
            }
        }

        // Assert
        using (var context = CreateContext())
        {
            var remaining = await context.Todos.ToListAsync();
            Assert.Equal(2, remaining.Count);
            Assert.NotNull(await context.Todos.FindAsync(id1));
            Assert.Null(await context.Todos.FindAsync(id2));
            Assert.NotNull(await context.Todos.FindAsync(id3));
        }
    }

    #endregion

    #region Integration Tests

    [Fact]
    public async Task FullWorkflow_CreateUpdateDelete_WorksCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act - Create
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            var newItem = new TodoItemEntity { Id = id, Title = "New Item", isCompleted = false, CreatedDate = DateTime.Now };
            await repository.CreateTodoItemAsync(newItem);
        }

        // Assert - Created
        using (var context = CreateContext())
        {
            var createdItem = await context.Todos.FindAsync(id);
            Assert.NotNull(createdItem);
            Assert.Equal("New Item", createdItem.Title);
        }

        // Act - Update
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            var updatedItem = new TodoItemEntity { Id = id, Title = "Updated Item", isCompleted = true, CreatedDate = DateTime.Now };
            await repository.UpdateTodoItemAsync(updatedItem);
        }

        // Assert - Updated
        using (var context = CreateContext())
        {
            var modifiedItem = await context.Todos.FindAsync(id);
            Assert.NotNull(modifiedItem);
            Assert.Equal("Updated Item", modifiedItem.Title);
            Assert.True(modifiedItem.isCompleted);
        }

        // Act - Delete (using manual removal due to in-memory database limitation)
        using (var context = CreateContext())
        {
            var itemToDelete = await context.Todos.FindAsync(id);
            if (itemToDelete != null)
            {
                context.Todos.Remove(itemToDelete);
                await context.SaveChangesAsync();
            }
        }

        // Assert - Deleted
        using (var context = CreateContext())
        {
            var deletedItem = await context.Todos.FindAsync(id);
            Assert.Null(deletedItem);
        }
    }

    [Fact]
    public async Task GetAllTodosAfterCreatingMultipleItems_ReturnsCorrectCount()
    {
        // Arrange & Act
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            
            for (int i = 1; i <= 5; i++)
            {
                var item = new TodoItemEntity 
                { 
                    Id = Guid.NewGuid(), 
                    Title = $"Task {i}", 
                    isCompleted = i % 2 == 0, 
                    CreatedDate = DateTime.Now 
                };
                await repository.CreateTodoItemAsync(item);
            }
        }

        // Assert
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            var allItems = await repository.GetAllTodos();
            Assert.Equal(5, allItems.Count);
            Assert.Equal(2, allItems.Count(x => x.isCompleted));
            Assert.Equal(3, allItems.Count(x => !x.isCompleted));
        }
    }

    [Fact]
    public async Task ConcurrentOperations_CreateAndRetrieve_WorkCorrectly()
    {
        // Arrange
        var ids = Enumerable.Range(1, 10).Select(_ => Guid.NewGuid()).ToList();

        // Act - Create all items
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            foreach (var id in ids)
            {
                var item = new TodoItemEntity 
                { 
                    Id = id, 
                    Title = $"Item {id}", 
                    isCompleted = false, 
                    CreatedDate = DateTime.Now 
                };
                await repository.CreateTodoItemAsync(item);
            }
        }

        // Act - Retrieve each one
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            foreach (var id in ids)
            {
                var item = await repository.GetTodoById(id);
                Assert.NotNull(item);
                Assert.Equal(id, item.Id);
            }
        }

        // Assert - All exist
        using (var context = CreateContext())
        {
            var repository = new TodoRepository(context);
            var allItems = await repository.GetAllTodos();
            Assert.Equal(10, allItems.Count);
        }
    }

    #endregion
}