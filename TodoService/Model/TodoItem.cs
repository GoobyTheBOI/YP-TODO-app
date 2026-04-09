namespace TodoServices.Dto
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Boolean isCompleted { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
