﻿using System.Threading.Tasks;
using Estudos.EFCore.RepositoryUoW.Data;
using Estudos.EFCore.RepositoryUoW.Data.Repositories.UsesGeneric;
using Estudos.EFCore.RepositoryUoW.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.RepositoryUoW.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DepartamentosGenericController : ControllerBase
    {
        private readonly IDepartamentoGenericRepository _departamentoGenericRepository;
        private readonly IUnitOfWork _uow;


        public DepartamentosGenericController(IDepartamentoGenericRepository departamentoGenericRepository, IUnitOfWork uow)
        {
            _departamentoGenericRepository = departamentoGenericRepository;
            _uow = uow;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var departamento = await _departamentoGenericRepository.GetByIdAsync(id);

            return Ok(departamento);
        }

        //departamento/?descricao=teste
        [HttpGet]
        public async Task<IActionResult> ConsultarDepartamentoAsync([FromQuery] string descricao)
        {
            var departamentos = await _departamentoGenericRepository
                .GetDataAsync(
                    p => p.Descricao.Contains(descricao),
                    p => p.Include(c => c.Colaboradores),
                    take: 2
                );

            return Ok(departamentos);
        }


        [HttpPost]
        public async Task<IActionResult> CreateDepartamento(Departamento departamento)
        {
            _departamentoGenericRepository.Add(departamento);
            var saved = await _uow.CommitAsync();

            return CreatedAtAction(nameof(Get), new { id = departamento.Id }, departamento);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemoveDepartamento(int id)
        {
            var departamento = await _departamentoGenericRepository.GetByIdAsync(id);
            _departamentoGenericRepository.Remove(departamento);
            var saved = await _uow.CommitAsync();

            return Ok(departamento);
        }


    }
}