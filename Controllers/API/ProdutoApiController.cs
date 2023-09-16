using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniApi.Context;
using UniApi.Models;

namespace UniApi.Controllers.API
{
    [ApiController]
    public class ProdutoApiController : Controller
    {
        readonly AppDbContext _context;

        public ProdutoApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("/prod")]
        public List<Produto> Get()
        {
            var c = _context.Produtos;
            return c.ToList();
        }

        [HttpGet("/prod/{id:int}")]

        public async Task<Produto> GetId(int id)
        {
            //var ok = _context.Produtos.Any(x => x.CategoriaId == id);
            //if (!ok)
            //{
            //    throw new Exception("Id informada nao existe");
            //}
            try
            {
                var model = await _context.Produtos.FirstOrDefaultAsync(x => x.ProdutoId == id);
                return model;
            }
            catch (ApplicationException ex)
            {
                throw new (ex.Message);
            }
        }

        [HttpGet("/produto")]
        public List<string> GetAllProduto()
        {
            var result = _context.Produtos.Include(x => x.Categorias);
            List<string> lista = new List<string>();

            foreach (var i in result)
            {
                lista.Add("ID: "+i.ProdutoId);
                lista.Add("Nome: " + i.ProdutoNome);
                lista.Add("Descrição: " + i.Descricao);
                lista.Add("Categoria: " + i.Categorias?.CategoriaNome);
            }
            return lista;
        }

        [HttpGet("/produto/{id:int}")]
        public List<string> GetIdProduto(int id)
        {
                var result = _context.Produtos.Include(x => x.Categorias).FirstOrDefault(x => x.ProdutoId == id);
                Produto prods = new Produto();
                prods.ProdutoId = id;
                prods.ProdutoNome = result?.ProdutoNome;
                prods.Descricao = result?.Descricao;
                var rs = _context.Produtos.Include(x => x.Categorias).FirstOrDefault(x => x.Categorias.CategoriaId == id);
                if(rs == null)
                {
                    List<string> l = new List<string> {"Produto não foi encontrado!"};
                    return l;
                }
                string catNome = rs.Categorias.CategoriaNome;
                List<string> lista = new List<string>();
                lista.Add("ID: " + prods.ProdutoId);
                lista.Add("Nome Do Produto: " + prods.ProdutoNome);
                lista.Add("Descrição: " + prods.Descricao);
                lista.Add("Categoria: " + catNome);
                return lista;
        }

        [HttpPost("/prod")]
        public Produto Post(Produto prod)
        {
            _context.Produtos.Add(prod);
            _context.SaveChanges();
            return prod;
        }

        [HttpPut("/prod/{id:int}")]
        public async Task<IActionResult> Put(Produto prod, int id)
        {
            var model = _context.Produtos.FirstOrDefault(x => x.ProdutoId == id);
            if (!ModelState.IsValid)
            {
                throw new Exception("Dados foram passados errado");
            }
            model.ProdutoNome = prod.ProdutoNome;
            model.Descricao = prod.Descricao;
            model.CategoriaId = prod.CategoriaId;
            _context.Update(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        [HttpDelete("/prod/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = _context.Produtos.FirstOrDefault(x => x.ProdutoId == id);
            if (!ModelState.IsValid)
            {
                throw new Exception("Dados foram passados errado");
            }
            _context.Remove(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }


    }
}
