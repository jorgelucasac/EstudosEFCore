using System.Collections.Generic;
using System.Threading.Tasks;
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

        public DepartamentosController(IDepartamentoRepository departamentoRepository)
        {
            _departamentoRepository = departamentoRepository;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var departamento = await _departamentoRepository.GetByIdAsync(id);

            return Ok(departamento);
        }
    }
}