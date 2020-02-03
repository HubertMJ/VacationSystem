using System;
using System.ComponentModel.DataAnnotations;

namespace VacationSystem.Models
{
    public class VacationRequest
    {
        public int Id { get; set; }
        [Required]
        public int WorkerId { get; set; }
        [Required]
        public int BossId { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime VacationStart { get; set; }
        public DateTime VacationEnd { get; set; }
    }
}