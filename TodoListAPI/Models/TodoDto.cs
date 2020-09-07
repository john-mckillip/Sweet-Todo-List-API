using System;
using TodoListAPI.Models.Abstract;

namespace TodoListAPI.Models
{
    public class TodoDto : TodoForManipulationDto
    {
        /// <summary>
        /// The Id of the todo
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The date the todo was last modified
        /// </summary>
        public DateTime LastModified { get; set; }
    }
}
