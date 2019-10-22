using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebSite.App_Code.Models;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace WebSites.App_Code.Services.UsuarioEmpresas
{
    public interface IUsuarioEmpresaService
{
    Task<ObservableCollection<UsuarioEmpresa>> GetUsuarioEmpresaAsync();

    Task<UsuarioEmpresa> PostUsuarioEmpresaAsync(UsuarioEmpresa c);

    Task<UsuarioEmpresa> PutUsuarioEmpresaAsync(UsuarioEmpresa c);

    Task<UsuarioEmpresa> DeleteUsuarioEmpresaAsync(int pessoaId);
}
}

}