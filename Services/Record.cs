using Microsoft.EntityFrameworkCore;
using UniApi.Context;
using UniApi.Models;

namespace UniApi.Services
{
    public class Record
    {
        private readonly AppDbContext _context;

        public Record(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Produto> Search(Produto prod)
        {
            var modelo = _context.Produtos.Include(x => x.Categorias).Where(x => x.CategoriaId == prod.CategoriaId);
            return modelo.ToList();
        }
    }
}
