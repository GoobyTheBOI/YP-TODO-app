using TodoServices.Exceptions;

namespace TodoServices.ValueObjects
{
    /// <summary>
    /// Value Object for Todo Item ID
    /// Encapsulates validation logic for todo identifiers
    /// </summary>
    public class TodoId
    {
        public Guid Value { get; }

        private TodoId(Guid value)
        {
            Value = value;
        }

        public static TodoId Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadRequestException("Todo item ID cannot be empty.");
            }

            return new TodoId(id);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not TodoId other)
                return false;

            return Value == other.Value;
        }
    }
}
