namespace TodoApi.DTO
{
    public class TodoItemResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Boolean isCompleted { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
