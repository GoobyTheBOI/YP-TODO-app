using Microsoft.EntityFrameworkCore;
using TodoRepositories.Entity;

namespace TodoRepositories
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }

        public DbSet<TodoItemEntity> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItemEntity>().HasData(
                new TodoItemEntity { Id = Guid.NewGuid(), Title = "First dummy todo", isCompleted = false, CreatedDate = DateTime.UtcNow },
                new TodoItemEntity { Id = Guid.NewGuid(), Title = "Second dummy todo", isCompleted = false, CreatedDate = DateTime.UtcNow },
                new TodoItemEntity { Id = Guid.NewGuid(), Title = "Third dummy todo", isCompleted = true, CreatedDate = DateTime.UtcNow },
                new TodoItemEntity { Id = Guid.NewGuid(), Title = "Fourth dummy todo", isCompleted = false, CreatedDate = DateTime.UtcNow },
                new TodoItemEntity { Id = Guid.NewGuid(), Title = "Fifth dummy todo", isCompleted = false, CreatedDate = DateTime.UtcNow }
            );
        }
    }
}
