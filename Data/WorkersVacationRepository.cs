using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VacationSystem.Models;

namespace VacationSystem.Data
{
    public class WorkersVacationRepository : IWorkersVacationRepository
    {
        private readonly DataContext _context;

        public WorkersVacationRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public async Task<Worker> GetWorker(int id)
        {
            var worker = await _context.Workers.FirstOrDefaultAsync(x => x.Id == id);
            return worker;
        }
        
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<VacationRequest> GetRequest(int id)
        {
            var request = await _context.VacationRequests.FirstOrDefaultAsync(x => x.WorkerId == id);
            return request;
        }
    }
}