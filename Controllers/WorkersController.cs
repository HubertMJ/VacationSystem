using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacationSystem.Data;

namespace VacationSystem.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly DataContext _context;
        public WorkersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> getWorkers()
        {
            var workers = await _context.Workers.ToListAsync();
            return Ok(workers);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> getWorker(int id) {
            var worker = await _context.Workers.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(worker);
        }

        
    }
}