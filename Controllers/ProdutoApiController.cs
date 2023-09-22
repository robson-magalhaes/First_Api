using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UniApi.Context;
using UniApi.Exceptions;
using UniApi.Models;
using UniApi.Services.Interfaces;

namespace UniApi.Controllers
{
    [ApiController]
    public class ProdutoApiController : Controller
    {
        readonly AppDbContext _context;
        public IConversaoJsonServices _json { get; set; }

        public ProdutoApiController(AppDbContext context, IConversaoJsonServices json)
        {
            _context = context;
            _json = json;
        }

        [HttpGet("/")]
        public async Task<IActionResult> Get()
        {
            var model = _context.Produtos.Include(x => x.Categorias).ToList();
            List<object> lista = new List<object>();
            foreach (var i in model)
            {
                var info = new
                {
                    i.ProdutoId,
                    i.ProdutoNome,
                    i.Descricao,
                    i.CategoriaId,
                    Categoria = i.Categorias?.CategoriaNome ?? "Não tem categoria"
                };
                lista.Add(info);
            }
            return new JsonResult(lista);
        }

        [HttpGet("/{id:int}")]
        public async Task<IActionResult> GetId(int id)
        {
            try
            {
                var model = await _context.Produtos.Include(x => x.Categorias).FirstOrDefaultAsync(x => x.ProdutoId == id);
                return new JsonResult(_json.ConversaoJson(model));
            }
            catch (ExceptionPersonal ex)
            {
                throw new($"Id de numero {id} nao foi encontrado!!");
            }
        }

        [HttpPost("/")]
        public async Task<IActionResult> Post(Produto prod)
        {
            try
            {
                _context.Produtos.Add(prod);
                await _context.SaveChangesAsync();
                return new JsonResult(_json.ConversaoJson(prod));
            }
            catch (SqliteException ex)
            {
                throw new SqliteException(ex.Message, ex.ErrorCode);
            }
            catch (Exception e)
            {
                throw new("Error: " + e.Message);
            }
        }

        [HttpPut("/{id:int}")]
        public async Task<IActionResult> Put(Produto prod, int id)
        {
            var model = _context.Produtos.FirstOrDefault(x => x.ProdutoId == id);
            if (!ModelState.IsValid)
            {
                throw new Exception("As informações não foram passadas corretamente");
            }
            try
            {
                model.ProdutoNome = prod.ProdutoNome;
                model.Descricao = prod.Descricao;
                model.CategoriaId = prod.CategoriaId;

                _context.Update(model);
                await _context.SaveChangesAsync();

                return new JsonResult(_json.ConversaoJson(model));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(ex.Message);
            }
            catch (ExceptionPersonal e)
            {
                throw new ExceptionPersonal("As informações não foram passadas corretamente - " + e.Message);
            }
        }

        [HttpDelete("/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = _context.Produtos.FirstOrDefault(x => x.ProdutoId == id);
            if (!ModelState.IsValid)
            {
                throw new Exception($"Id {id} nao existe!");
            }
            try
            {
                _context.Remove(model);
                await _context.SaveChangesAsync();
                return new JsonResult(_json.ConversaoJson(model));
            }
            catch
            {
                throw new Exception($"Produto com a Id '{id}' não existe.");
            }
        }
    }
}
