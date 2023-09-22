using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UniApi.Context;
using UniApi.Exceptions;
using UniApi.Models;
using UniApi.Services.Interfaces;

namespace UniApi.Services
{
    public class ConversaoJsonServices : IConversaoJsonServices
    {
        readonly AppDbContext _context;

        public ConversaoJsonServices(AppDbContext context)
        {
            _context = context;
        }

        public Dictionary<object, object> ConversaoJson(Produto model)
        {
            try
            {
                Dictionary<object, object> _json = new Dictionary<object, object>();
                _json.Add("ProdutoId", model.ProdutoId);
                _json.Add("ProdutoNome", model.ProdutoNome);
                _json.Add("Descricao", model.Descricao);
                _json.Add("CategoriaId", model.CategoriaId);
                _json.Add("Categorias", _context.Categorias.Find(model.CategoriaId).CategoriaNome);

                return _json;
            }
            catch
            {
                throw new ExceptionPersonal("Id informado nao foi encontrado!");
            }
        }
    }
}