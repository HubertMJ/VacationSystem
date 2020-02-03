using System.Threading.Tasks;
using VacationSystem.Models;

namespace VacationSystem.Data
{
    public interface IWorkersVacationRepository
    {
        void Add<T>(T entity) where T : class;
        Task<Worker> GetWorker(int id);
        Task<bool> SaveAll();
        Task<VacationRequest> GetRequest(int id);

    }
}