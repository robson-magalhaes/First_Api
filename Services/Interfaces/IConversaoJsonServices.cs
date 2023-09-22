using UniApi.Models;

namespace UniApi.Services.Interfaces
{
    public interface IConversaoJsonServices
    {
        Dictionary<object, object> ConversaoJson(Produto model);
    }
}
