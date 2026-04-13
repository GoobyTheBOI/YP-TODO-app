using Microsoft.EntityFrameworkCore;
using TodoApi.Middleware;
using TodoRepositories;
using TodoRepositories.IRepository;
using TodoServices.IService;
using TodoServices.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Add root endpoint
app.MapGet("/", () => Results.Ok(new { message = "TODO API is running", version = "1.0", docs = "/swagger" }))
    .WithName("Root")
    .WithOpenApi()
    .Produces(StatusCodes.Status200OK)
    .WithSummary("Root endpoint");

app.Run();
