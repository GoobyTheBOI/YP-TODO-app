namespace TodoService
{
    public class TodoService
    {
        public string Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required");

            return $"Todo created: {title}";
        }
    }
}
