using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UniApi.Context;
using UniApi.Exceptions;
using UniApi.Models;
using UniApi.Services;

namespace UniApi.Controllers.API
{
    [ApiController]
    public class ProdutoApiController : Controller
    {
        readonly AppDbContext _context;
        public ConversaoJsonServices _json;

        public ProdutoApiController(AppDbContext context, ConversaoJsonServices json)
        {
            _context = context;
            _json = json;
        }

        [HttpGet("/")]
        public async Task<IActionResult> Get()
        {
            var model = _context.Produtos.Include(x=>x.Categorias).ToList();
            List<object> lista = new List<object>();
            foreach (var i in model)
            {
                var info = new
                {
                    ProdutoId = i.ProdutoId,
                    ProdutoNome = i.ProdutoNome,
                    Descricao = i.Descricao,
                    CategoriaId = i.CategoriaId,
                    Categoria = i.Categorias.CategoriaNome ?? "Não tem categoria"
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
                var model = await _context.Produtos.Include(x=>x.Categorias).FirstOrDefaultAsync(x => x.ProdutoId == id);
                //if (model == null)
                //{
                //    throw new Exception($"Id de numero {id}, não encontrado!!");
                //}

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
                var json = new
                {
                    ProdutoId = prod.ProdutoId,
                    ProdutoNome = prod.ProdutoNome,
                    Descricao = prod.Descricao,
                    Categoria = _context.Categorias.Find(prod.CategoriaId).CategoriaNome ?? "Categoria não informada"
                };
                return new JsonResult(json);
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
                throw new ExceptionPersonal("As informações não foram passadas corretamente");
            }
        }

        [HttpDelete("/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = _context.Produtos.FirstOrDefault(x => x.ProdutoId == id);
            if (!ModelState.IsValid)
            {
                throw new Exception("Dados foram passados errado");
            }
            try
            {
                _context.Remove(model);
                await _context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                throw new Exception($"Produto com a Id '{id}' não existe.");
            }
        }
    }
}
