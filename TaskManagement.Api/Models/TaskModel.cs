using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Api.Models
{
    public class TaskModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; }

        public string Description { get; set; }

        public int AssignedToUserId { get; set; } // Foreign key

        public string Status { get; set; } = "Open"; // Default status
    }
}
