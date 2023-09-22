using UniApi.Context;
using UniApi.Exceptions;

namespace UniApi.Models.ConversorJson
{
    public class ProdutoConverterJson
    {
        readonly AppDbContext _context;

        public ProdutoConverterJson(AppDbContext context)
        {
            _context = context;
        }

        public Dictionary<object, object> ConversaoJson(Produto model)
        {
            try
            {
                Dictionary<object, object> json = new Dictionary<object, object>();
                json.Add("ProdutoId", model.ProdutoId);
                json.Add("ProdutoNome", model.ProdutoNome);
                json.Add("Descricao", model.Descricao);
                json.Add("CategoriaId", model.CategoriaId);
                json.Add("Categorias", _context.Categorias.Find(model.CategoriaId).CategoriaNome);

                return json;
            }
            catch
            {
                throw new ExceptionPersonal("Id informado nao foi encontrado!");
            }
        }
    }

}
