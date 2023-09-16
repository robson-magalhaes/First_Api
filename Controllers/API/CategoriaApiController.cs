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
        public Categoria Post(Categoria prod)
        {
            _context.Categorias.Add(prod);
            _context.SaveChanges();
            return prod;
        }

        [HttpPut("/categoria/{id:int}")]
        public Categoria Update(Categoria categoria, int id)
        {
            var model = _context.Categorias.Find(id);
            model.CategoriaNome = categoria.CategoriaNome;
            _context.Categorias.Update(model);
            _context.SaveChanges();
            return model;
        }

        [HttpDelete("/categoria/{id:int}")]
        public Categoria Delete(int id)
        {
            var model = _context.Categorias.Find(id);
            if(model != null)
            {
                _context.Categorias.Remove(model);
                _context.SaveChanges();
            }
            return model;
        }
    }
}
