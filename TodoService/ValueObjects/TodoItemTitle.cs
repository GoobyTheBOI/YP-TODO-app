using TodoServices.Exceptions;

namespace TodoServices.ValueObjects
{
    /// <summary>
    /// Value Object for Todo Item Title
    /// Encapsulates validation logic for todo titles
    /// </summary>
    public class TodoItemTitle
    {
        private const int MaxLength = 200;
        private const int MinLength = 1;

        public string Value { get; }

        private TodoItemTitle(string value)
        {
            Value = value;
        }

        public static TodoItemTitle Create(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new BadRequestException("Todo item title is required and cannot be empty.");
            }

            if (title.Length < MinLength)
            {
                throw new BadRequestException($"Todo item title must be at least {MinLength} character.");
            }

            if (title.Length > MaxLength)
            {
                throw new BadRequestException($"Todo item title cannot exceed {MaxLength} characters.");
            }

            return new TodoItemTitle(title.Trim());
        }

        public static implicit operator string(TodoItemTitle title) => title.Value;

        public override bool Equals(object? obj)
        {
            if (obj is not TodoItemTitle other)
                return false;

            return Value == other.Value;
        }
    }
}
