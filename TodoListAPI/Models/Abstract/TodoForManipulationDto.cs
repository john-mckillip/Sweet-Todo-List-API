using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.Models.Abstract
{
    public abstract class TodoForManipulationDto
    {
        /// <summary>
        /// The name of the todo
        /// </summary>
        [Required(ErrorMessage = "Please provide a name value.")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// The optional description of the todo
        /// </summary>
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
