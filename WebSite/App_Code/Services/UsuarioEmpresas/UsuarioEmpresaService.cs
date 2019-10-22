using WebSite.App_Code.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebSites.App_Code.Services.UsuarioEmpresas;

namespace WebSite.App_Code.Services.UsuarioEmpresas
{
    public class UsuarioEmpresaService : IUsuarioEmpresaService
    {
        private readonly IRequest _request;
        private const string ApiUrlBase = "http://universesoftware2019.somee.com/api/CadUsuarios";

        public UsuarioEmpresaService()
        {
            _request = new Request();
        }

        public async Task<UsuarioEmpresa> DeleteUsuarioEmpresaAsync(int usuarioId)
        {
            string urlComplementar = string.Format("/{0}", usuarioId);
            await _request.DeleteAsync(ApiUrlBase + urlComplementar);
            return new UsuarioEmpresa() { IdUsuario = usuarioId };
        }

        public async Task<ObservableCollection<UsuarioEmpresa>> GetUsuarioEmpresaAsync()
        {
            ObservableCollection<UsuarioEmpresa> usuarioEmpresa = await
                _request.GetAsync<ObservableCollection<UsuarioEmpresa>>(ApiUrlBase);

            return usuarioEmpresa;
        }

        public async Task<UsuarioEmpresa> PostUsuarioEmpresaAsync(UsuarioEmpresa c)
        {
            if (c.IdUsuario == 0)
            {
                //Errado
                //string urlComplementar = string.Format("/I/{0}", c.TipoUsuario);
                //return await _request.PostAsync(ApiUrlBase, c);

                string urlComplementar = string.Format("/I/{0}", c.TipoUsuario);
                return await _request.PostAsync(ApiUrlBase + urlComplementar, c);

            }
            else
                return await _request.PutAsync(ApiUrlBase, c);
        }

        public async Task<UsuarioEmpresa> PutUsuarioEmpresaAsync(UsuarioEmpresa c)
        {
            string urlComplementar = string.Format("/U/{0}", c.IdUsuario);
            var result = await _request.PutAsync(ApiUrlBase + urlComplementar, c);
            return result;
        }
    }
}