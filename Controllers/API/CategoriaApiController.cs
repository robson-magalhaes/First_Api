using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniApi.Context;
using UniApi.Models;

namespace UniApi.Controllers.API
{
    [ApiController]
    public class CategoriaApiController : Controller
    {
        readonly AppDbContext _context;
        public Categoria cate = new Categoria();

        public CategoriaApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("/categoria")]
        public Categoria PostCategoria(Categoria prod)
        {
            _context.Categorias.Add(prod);
            _context.SaveChanges();
            return prod;
        }

    }
}
