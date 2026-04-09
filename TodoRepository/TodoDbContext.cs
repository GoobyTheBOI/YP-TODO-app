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
                new TodoItemEntity { Id = new Guid("20b3ff4d-ee8e-414d-823f-65ae3600e2af"), Title = "First dummy todo", isCompleted = false, CreatedDate = new DateTime(2026, 4, 9, 15, 25, 27, 39, DateTimeKind.Utc).AddTicks(737) },
                new TodoItemEntity { Id = new Guid("58e50c68-460e-40f2-a2df-792b1cf17d4c"), Title = "Second dummy todo", isCompleted = false, CreatedDate = new DateTime(2026, 4, 9, 15, 25, 27, 39, DateTimeKind.Utc).AddTicks(1122) },
                new TodoItemEntity { Id = new Guid("5d5794cc-8d91-4a32-bc8c-2f9b72b9faa3"), Title = "Third dummy todo", isCompleted = true, CreatedDate = new DateTime(2026, 4, 9, 15, 25, 27, 39, DateTimeKind.Utc).AddTicks(1130) },
                new TodoItemEntity { Id = new Guid("ada242e8-f933-421e-bcca-49fdcff41096"), Title = "Fourth dummy todo", isCompleted = false, CreatedDate = new DateTime(2026, 4, 9, 15, 25, 27, 39, DateTimeKind.Utc).AddTicks(1134) },
                new TodoItemEntity { Id = new Guid("ddb976e7-9344-4533-95de-ebcb71bbd969"), Title = "Fifth dummy todo", isCompleted = false, CreatedDate = new DateTime(2026, 4, 9, 15, 25, 27, 39, DateTimeKind.Utc).AddTicks(1138) }
            );
        }
    }
}
