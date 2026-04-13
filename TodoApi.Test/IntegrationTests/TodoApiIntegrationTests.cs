using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoApi.DTO;
using Xunit;

namespace TodoApi.Test.IntegrationTests
{
    //public class TodoApiIntegrationTests : IAsyncLifetime
    //{
    //    private WebApplicationFactory<Program> _factory;
    //    private HttpClient _client;

    //    public async Task InitializeAsync()
    //    {
    //        _factory = new WebApplicationFactory<Program>();
    //        _client = _factory.CreateClient();
    //        await Task.CompletedTask;
    //    }

    //    public async Task DisposeAsync()
    //    {
    //        _client?.Dispose();
    //        _factory?.Dispose();
    //        await Task.CompletedTask;
    //    }

    //    // ====== GET ALL TODOS TESTS ======

    //    [Fact]
    //    public async Task GetAllTodos_ReturnsOkStatus()
    //    {
    //        // Act
    //        var response = await _client.GetAsync("/api/todos");

    //        // Assert
    //        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    //    }

    //    [Fact]
    //    public async Task GetAllTodos_ReturnsListOfTodos()
    //    {
    //        // Act
    //        var response = await _client.GetAsync("/api/todos");
    //        var content = await response.Content.ReadAsStringAsync();
    //        var todos = JsonSerializer.Deserialize<List<TodoItemResponse>>(content,
    //            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    //        // Assert
    //        Assert.NotNull(todos);
    //        Assert.IsType<List<TodoItemResponse>>(todos);
    //    }

    //    // ====== CREATE TODO TESTS ======

    //    [Fact]
    //    public async Task CreateTodo_WithValidData_ReturnsOkStatus()
    //    {
    //        // Arrange
    //        var createRequest = new CreateToDoItemRequestDto { Title = "Test Todo Item" };
    //        var json = JsonSerializer.Serialize(createRequest);
    //        var content = new StringContent(json, Encoding.UTF8, "application/json");

    //        // Act
    //        var response = await _client.PostAsync("/api/todos", content);

    //        // Assert
    //        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //    }

    //    [Fact]
    //    public async Task CreateTodo_WithEmptyTitle_ReturnsBadRequest()
    //    {
    //        // Arrange
    //        var createRequest = new CreateToDoItemRequestDto { Title = "" };
    //        var json = JsonSerializer.Serialize(createRequest);
    //        var content = new StringContent(json, Encoding.UTF8, "application/json");

    //        // Act
    //        var response = await _client.PostAsync("/api/todos", content);

    //        // Assert
    //        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    //    }

    //    [Fact]
    //    public async Task CreateTodo_WithNullTitle_ReturnsBadRequest()
    //    {
    //        // Arrange
    //        var createRequest = new { title = (string)null };
    //        var json = JsonSerializer.Serialize(createRequest);
    //        var content = new StringContent(json, Encoding.UTF8, "application/json");

    //        // Act
    //        var response = await _client.PostAsync("/api/todos", content);

    //        // Assert
    //        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    //    }

    //    // ====== GET TODO BY ID TESTS ======

    //    [Fact]
    //    public async Task GetTodoById_WithValidId_ReturnsOkStatus()
    //    {
    //        // Arrange - Create a todo first
    //        var createRequest = new CreateToDoItemRequestDto { Title = "Test Todo for Get" };
    //        var json = JsonSerializer.Serialize(createRequest);
    //        var createContent = new StringContent(json, Encoding.UTF8, "application/json");
    //        var createResponse = await _client.PostAsync("/api/todos", createContent);

    //        var allTodos = await _client.GetAsync("/api/todos");
    //        var todosContent = await allTodos.Content.ReadAsStringAsync();
    //        var todos = JsonSerializer.Deserialize<List<TodoItemResponse>>(todosContent,
    //            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    //        var todoId = todos?.FirstOrDefault()?.Id ?? Guid.Empty;

    //        // Act
    //        var response = await _client.GetAsync($"/api/todos/{todoId}");

    //        // Assert
    //        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //    }

    //    [Fact]
    //    public async Task GetTodoById_WithInvalidId_ReturnsBadRequest()
    //    {
    //        // Arrange
    //        var invalidId = Guid.Parse("00000000-0000-0000-0000-000000000000");

    //        // Act
    //        var response = await _client.GetAsync($"/api/todos/{invalidId}");

    //        // Assert - Empty GUID triggers BadRequestException (validation)
    //        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    //    }

    //    [Fact]
    //    public async Task GetTodoById_WithInvalidId_ReturnsErrorResponse()
    //    {
    //        // Arrange
    //        var invalidId = Guid.Parse("00000000-0000-0000-0000-000000000000");

    //        // Act
    //        var response = await _client.GetAsync($"/api/todos/{invalidId}");
    //        var content = await response.Content.ReadAsStringAsync();

    //        // Assert
    //        Assert.Contains("traceId", content);
    //        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    //    }

    //    // ====== UPDATE TODO TESTS ======

    //    [Fact]
    //    public async Task UpdateTodo_WithValidData_ReturnsNoContent()
    //    {
    //        // Arrange - Create a todo first
    //        var createRequest = new CreateToDoItemRequestDto { Title = "Todo to Update" };
    //        var json = JsonSerializer.Serialize(createRequest);
    //        var createContent = new StringContent(json, Encoding.UTF8, "application/json");
    //        await _client.PostAsync("/api/todos", createContent);

    //        var allTodos = await _client.GetAsync("/api/todos");
    //        var todosContent = await allTodos.Content.ReadAsStringAsync();
    //        var todos = JsonSerializer.Deserialize<List<TodoItemResponse>>(todosContent,
    //            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    //        var todoId = todos?.FirstOrDefault()?.Id ?? Guid.Empty;

    //        // Update the todo
    //        var updateRequest = new PatchToDoItemRequestDto
    //        {
    //            Title = "Updated Todo Title",
    //            isCompleted = true
    //        };
    //        var updateJson = JsonSerializer.Serialize(updateRequest);
    //        var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");

    //        // Act
    //        var response = await _client.PatchAsync($"/api/todos/{todoId}", updateContent);

    //        // Assert
    //        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    //    }

    //    [Fact]
    //    public async Task UpdateTodo_WithInvalidId_ReturnsBadRequest()
    //    {
    //        // Arrange
    //        var invalidId = Guid.Parse("00000000-0000-0000-0000-000000000000");
    //        var updateRequest = new PatchToDoItemRequestDto
    //        {
    //            Title = "Updated Title",
    //            isCompleted = true
    //        };
    //        var json = JsonSerializer.Serialize(updateRequest);
    //        var content = new StringContent(json, Encoding.UTF8, "application/json");

    //        // Act
    //        var response = await _client.PatchAsync($"/api/todos/{invalidId}", content);

    //        // Assert - Empty GUID triggers BadRequestException (validation)
    //        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    //    }

    //    // ====== DELETE TODO TESTS ======

    //    [Fact]
    //    public async Task DeleteTodo_WithValidId_ReturnsNoContent()
    //    {
    //        // Arrange - Create a todo first
    //        var createRequest = new CreateToDoItemRequestDto { Title = "Todo to Delete" };
    //        var json = JsonSerializer.Serialize(createRequest);
    //        var createContent = new StringContent(json, Encoding.UTF8, "application/json");
    //        await _client.PostAsync("/api/todos", createContent);

    //        var allTodos = await _client.GetAsync("/api/todos");
    //        var todosContent = await allTodos.Content.ReadAsStringAsync();
    //        var todos = JsonSerializer.Deserialize<List<TodoItemResponse>>(todosContent,
    //            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    //        var todoId = todos?.FirstOrDefault()?.Id ?? Guid.Empty;

    //        // Act
    //        var response = await _client.DeleteAsync($"/api/todos/{todoId}");

    //        // Assert
    //        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    //    }

    //    [Fact]
    //    public async Task DeleteTodo_WithInvalidId_ReturnsBadRequest()
    //    {
    //        // Arrange
    //        var invalidId = Guid.Parse("00000000-0000-0000-0000-000000000000");

    //        // Act
    //        var response = await _client.DeleteAsync($"/api/todos/{invalidId}");

    //        // Assert - Empty GUID triggers BadRequestException (validation)
    //        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    //    }

    //    // ====== ERROR HANDLING TESTS ======

    //    [Fact]
    //    public async Task ErrorResponse_ContainsTraceId()
    //    {
    //        // Arrange
    //        var invalidId = Guid.Parse("00000000-0000-0000-0000-000000000000");

    //        // Act
    //        var response = await _client.GetAsync($"/api/todos/{invalidId}");
    //        var content = await response.Content.ReadAsStringAsync();
    //        var errorResponse = JsonSerializer.Deserialize<JsonElement>(content,
    //            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    //        // Assert
    //        Assert.True(response.StatusCode == HttpStatusCode.BadRequest || 
    //                   response.StatusCode == HttpStatusCode.NotFound);

    //        // Check if response has extensions with traceId
    //        if (errorResponse.TryGetProperty("extensions", out var extensions))
    //        {
    //            Assert.True(extensions.TryGetProperty("traceId", out var traceId));
    //            Assert.NotEqual("", traceId.GetString());
    //        }
    //    }

    //    [Fact]
    //    public async Task ErrorResponse_ContainsTitle()
    //    {
    //        // Arrange
    //        var invalidId = Guid.Parse("00000000-0000-0000-0000-000000000000");

    //        // Act
    //        var response = await _client.GetAsync($"/api/todos/{invalidId}");
    //        var content = await response.Content.ReadAsStringAsync();
    //        var errorResponse = JsonSerializer.Deserialize<JsonElement>(content,
    //            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    //        // Assert
    //        Assert.True(errorResponse.TryGetProperty("title", out var title));
    //        Assert.NotEqual("", title.GetString());
    //    }

    //    // ====== HAPPY PATH INTEGRATION TEST ======

    //    [Fact]
    //    public async Task HappyPath_CreateReadUpdateDelete_Succeeds()
    //    {
    //        // Arrange
    //        var createRequest = new CreateToDoItemRequestDto { Title = "Happy Path Todo" };
    //        var createJson = JsonSerializer.Serialize(createRequest);
    //        var createContent = new StringContent(createJson, Encoding.UTF8, "application/json");

    //        // Act 1: Create
    //        var createResponse = await _client.PostAsync("/api/todos", createContent);
    //        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

    //        // Act 2: Get All and find the created todo
    //        var getAllResponse = await _client.GetAsync("/api/todos");
    //        var allTodosContent = await getAllResponse.Content.ReadAsStringAsync();
    //        var todos = JsonSerializer.Deserialize<List<TodoItemResponse>>(allTodosContent,
    //            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    //        var createdTodo = todos?.FirstOrDefault(t => t.Title == "Happy Path Todo");
    //        Assert.NotNull(createdTodo);

    //        // Act 3: Get Single Todo
    //        var getSingleResponse = await _client.GetAsync($"/api/todos/{createdTodo!.Id}");
    //        Assert.Equal(HttpStatusCode.OK, getSingleResponse.StatusCode);

    //        // Act 4: Update Todo
    //        var updateRequest = new PatchToDoItemRequestDto
    //        {
    //            Title = "Updated Happy Path Todo",
    //            isCompleted = true
    //        };
    //        var updateJson = JsonSerializer.Serialize(updateRequest);
    //        var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");
    //        var updateResponse = await _client.PatchAsync($"/api/todos/{createdTodo.Id}", updateContent);
    //        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

    //        // Act 5: Verify Update
    //        var verifyResponse = await _client.GetAsync($"/api/todos/{createdTodo.Id}");
    //        var verifyContent = await verifyResponse.Content.ReadAsStringAsync();
    //        var updatedTodo = JsonSerializer.Deserialize<TodoItemResponse>(verifyContent,
    //            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    //        Assert.True(updatedTodo?.isCompleted);

    //        // Act 6: Delete Todo
    //        var deleteResponse = await _client.DeleteAsync($"/api/todos/{createdTodo.Id}");
    //        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

    //        // Assert: Verify Delete
    //        var verifyDeleteResponse = await _client.GetAsync($"/api/todos/{createdTodo.Id}");
    //        Assert.Equal(HttpStatusCode.NotFound, verifyDeleteResponse.StatusCode);
    //    }
    //}
}
