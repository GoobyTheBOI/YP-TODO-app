namespace TodoApi.DTO
{
    public class PatchToDoItemRequestDto
    {
        public string Title { get; set; }
        public bool isCompleted { get; set; }
    }
}
