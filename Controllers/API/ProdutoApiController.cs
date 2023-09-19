﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UniApi.Context;
using UniApi.Exceptions;
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
                    Categoria = i.Categorias?.CategoriaNome ?? "Não tem categoria"
                };
                lista.Add(info);
            }
            return new JsonResult(lista);
        }

        [HttpGet("/{id:int}")]

        public async Task<Produto> GetId(int id)
        {
            try
            {
                var model = await _context.Produtos.FirstOrDefaultAsync(x => x.ProdutoId == id);
                if (model == null)
                {
                    throw new Exception($"O Id de numero {id}, não encontrado!!");
                }
                return model;
            }
            catch (ApplicationException ex)
            {
                throw new(ex.Message);
            }
        }

        [HttpPost("/")]
        public async Task<IActionResult> Post(Produto prod)
        {
            try
            {
                _context.Produtos.Add(prod);
                await _context.SaveChangesAsync();
                return Ok(prod);
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
                return Ok(model);
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
