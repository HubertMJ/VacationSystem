using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VacationSystem.Models;

namespace VacationSystem.Data
{
    public class AuthRepository: IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Worker> Login(string username, string password)
        {
            var worker = await _context.Workers.FirstOrDefaultAsync(x => x.Username == username);

            if(worker == null)
                return null;

            if(!VerifyPasswordHash(password, worker.PasswordHash, worker.PasswordSalt))
                return null;
                
            return worker;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] paswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(paswordSalt)){
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < computedHash.Length; i++){
                    if (computedHash[i] != passwordHash[i]) 
                        return false;
                }
            }
            return true;
        }

        public async Task<Worker> Register(Worker worker, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            worker.PasswordHash = passwordHash;
            worker.PasswordSalt = passwordSalt;

            await _context.Workers.AddAsync(worker);
            await _context.SaveChangesAsync();

            return worker;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> WorkerExists(string username)
        {
            if(await _context.Workers.AnyAsync(x => x.Username == username))
                return true;

            return false;

        }
        
    }
}