using System.Threading.Tasks;
using VacationSystem.Models;

namespace VacationSystem.Data
{
    public interface IAuthRepository
    {
        Task<Worker> Register(Worker Username, string Password);
        Task<Worker> Login(string Username, string Password);
        Task<bool> WorkerExists(string Username);
    }
}