using System;
using VacationSystem.Models;

namespace VacationSystem.Dtos
{
    public class VacationRequestDto
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public int BossId { get; set; }
        public DateTime VacationStart { get; set; }
        public DateTime VacationEnd { get; set; }
    }
}