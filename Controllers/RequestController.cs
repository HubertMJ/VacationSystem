using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacationSystem.Data;
using VacationSystem.Dtos;
using VacationSystem.Models;

namespace VacationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RequestController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWorkersVacationRepository _repository;
        public RequestController(DataContext context, IWorkersVacationRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        [HttpPost("NewRequest")]
        public async Task<IActionResult> NewRequest([FromBody]VacationRequestDto vacationRequestDto)
        {
            var message = "Failed at : ";
            var boss = await _repository.GetWorker(vacationRequestDto.BossId);
            var worker = await _repository.GetWorker(vacationRequestDto.WorkerId);
            var request = await _repository.GetRequest(vacationRequestDto.WorkerId); // Sprawdzenie czy pracownik posiada już jakieś zgłoszenie

            if (worker.PermissionLevel < boss.PermissionLevel && 
                request == null && vacationRequestDto.VacationEnd > vacationRequestDto.VacationStart)
            {
                var newVacationRequest = new VacationRequest()
                {
                    BossId = vacationRequestDto.BossId,
                    WorkerId = vacationRequestDto.WorkerId,
                    CreateDate = DateTime.Today,
                    VacationStart = vacationRequestDto.VacationStart,
                    VacationEnd = vacationRequestDto.VacationEnd
                };

                _repository.Add(newVacationRequest);
                await _repository.SaveAll();
                return Ok();
            }

            if (worker == null || boss == null)
            {
                message = message + "boss or worker Id";
            }
            
            if (request != null)
            {
                message = message + " worker already have one request ";
            }

            if (worker.PermissionLevel >= boss.PermissionLevel)
            {
               message = message + " Boss permission level must be higher then worker permission level ";
            }

            if (vacationRequestDto.VacationEnd < vacationRequestDto.VacationStart)
            {
                message = message + " Wrong Vacation Start/End dates";
            }
            return BadRequest(message);
        }
        
    }
}