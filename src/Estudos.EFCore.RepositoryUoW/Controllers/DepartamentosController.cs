using System.Threading.Tasks;
using Estudos.EFCore.RepositoryUoW.Data;
using Estudos.EFCore.RepositoryUoW.Data.Repositories;
using Estudos.EFCore.RepositoryUoW.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.EFCore.RepositoryUoW.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DepartamentosController : ControllerBase
    {
        private readonly IDepartamentoRepository _departamentoRepository;
        private readonly IUnitOfWork _uow;

        public DepartamentosController(IDepartamentoRepository departamentoRepository, IUnitOfWork uow)
        {
            _departamentoRepository = departamentoRepository;
            _uow = uow;
        }

        [HttpGet("repository/{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var departamento = await _departamentoRepository.GetByIdAsync(id);

            return Ok(departamento);
        }

        [HttpGet("unit-of-work/{id:int}")]
        public async Task<IActionResult> GetUoW(int id)
        {
            var departamento = await _uow.DepartamentoRepository.GetByIdAsync(id);
            return Ok(departamento);
        }

        [HttpPost("repository")]
        public async Task<IActionResult> CreateDepartamento(Departamento departamento)
        {
            await _departamentoRepository.AddAsync(departamento);
            var saved = await _departamentoRepository.SaveAsync();

            return CreatedAtAction(nameof(Get), new { id = departamento.Id }, departamento);
        }


        [HttpPost("unit-of-work")]
        public async Task<IActionResult> CreateDepartamentoUoW(Departamento departamento)
        {
            await _departamentoRepository.AddAsync(departamento);
            var saved = await _uow.CommitAsync();

            return CreatedAtAction(nameof(Get), new { id = departamento.Id }, departamento);
        }

        [HttpPost("unit-of-work-2")]
        public async Task<IActionResult> CreateDepartamentoUoW2(Departamento departamento)
        {
            await _uow.DepartamentoRepository.AddAsync(departamento);
            var saved = await _uow.CommitAsync();

            return CreatedAtAction(nameof(Get), new { id = departamento.Id }, departamento);
        }
    }
}