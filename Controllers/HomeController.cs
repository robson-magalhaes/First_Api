using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using UniApi.Context;
using UniApi.Models;

namespace UniApi.Controllers
{
    public class HomeController : Controller
    {
        readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            var c = _context.Produtos.Include(x=>x.Categorias);
            return View(await c.ToListAsync());
        }

    }
}
