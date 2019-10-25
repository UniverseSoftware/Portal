using System.Threading.Tasks;

namespace WebSite.Entities.Services
{
    public interface IRequest
    {
        Task<TResult> GetAsync<TResult>(string uri);

        Task<TResult> PostAsync<TResult>(string uri, TResult data);

        Task<TResult> PutAsync<TResult>(string uri, TResult data);

        Task DeleteAsync(string uri);
    }
}
