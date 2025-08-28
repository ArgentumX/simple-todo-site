using System.ComponentModel.DataAnnotations;

namespace TODO.Models.Task.view
{
    public class EditTaskViewModel
    {
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name field cannot exceed 100 characters.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
