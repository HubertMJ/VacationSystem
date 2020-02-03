using System.ComponentModel.DataAnnotations;

namespace VacationSystem.Dtos
{
    public class WorkerForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 8 characters")]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        public int VacationDays { get; set; }
        public int PermissionLevel { get; set; }
    }
}