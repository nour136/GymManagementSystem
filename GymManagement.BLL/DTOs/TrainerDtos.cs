using System.ComponentModel.DataAnnotations;

namespace GymManagement.BLL.DTOs
{
    public class TrainerDto
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Specialization { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateTrainerDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? Specialization { get; set; }
    }

    public class UpdateTrainerDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? Specialization { get; set; }
    }
}
