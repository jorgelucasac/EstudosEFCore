using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.EFCore.MultiTenant.Data;
using Estudos.EFCore.MultiTenant.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.MultiTenant.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PessoasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Pessoa>> Index()
        {
            return await _context.Pessoas.ToListAsync();
        }
    }
}