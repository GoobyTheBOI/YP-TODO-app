using System;
using System.Collections.Generic;
using System.Text;

namespace TodoRepositories.Entity
{
    public class TodoItemEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Boolean isCompleted { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
