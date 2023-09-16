using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniApi.Context;
using UniApi.Models;
using UniApi.Services;

namespace UniApi.Controllers
{
    public class RecordsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly Record _record;

        public RecordsController(AppDbContext context, Record record)
        {
            _context = context;
            _record = record;
        }

        public IActionResult Index()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome");
            var tela = _context.Produtos.Include(x=>x.Categorias).ToList();
            return View(tela);
        }

        public async Task<IActionResult> SimpleSearch(Produto prod)
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome");
            var model = _record.Search(prod);
            ViewData["util"] = model;
            return View(model);
        }
    }
}
