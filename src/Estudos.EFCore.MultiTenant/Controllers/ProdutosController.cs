using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.EFCore.MultiTenant.Data;
using Estudos.EFCore.MultiTenant.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.MultiTenant.Controllers
{
    [ApiController]
    [Route("api/v1/{tenant}/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Produto>> Get()
        {
            return await _context.Produtos.ToListAsync();
        }
    }
}