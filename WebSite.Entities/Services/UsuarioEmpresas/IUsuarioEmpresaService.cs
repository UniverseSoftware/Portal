using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WebSite.Entities.Models;

namespace WebSite.Entities.Services.UsuarioEmpresas
{
    public interface IUsuarioEmpresaService
    {
        Task<ObservableCollection<UsuarioEmpresa>> GetUsuarioEmpresaAsync();

        Task<UsuarioEmpresa> PostUsuarioEmpresaAsync(UsuarioEmpresa e);

        Task<UsuarioEmpresa> PutUsuarioEmpresaAsync(UsuarioEmpresa e);

        Task<UsuarioEmpresa> DeleteUsuarioEmpresaAsync(int empresaId);
    }
}
