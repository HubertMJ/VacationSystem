using Microsoft.EntityFrameworkCore;
using VacationSystem.Models;

namespace VacationSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}
        
        public DbSet<Worker> Workers { get; set; }
        public DbSet<VacationRequest> VacationRequests { get; set; }

    }
}