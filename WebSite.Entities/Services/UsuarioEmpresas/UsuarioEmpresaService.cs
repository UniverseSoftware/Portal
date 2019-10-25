using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebSite.Entities.Models;

namespace WebSite.Entities.Services.UsuarioEmpresas
{
    public class UsuarioEmpresaService
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

        public async Task<UsuarioEmpresa> PostUsuarioEmpresaAsync(UsuarioEmpresa e)
        {
            if (e.IdUsuario == 0)
            {
                //Errado
                //string urlComplementar = string.Format("/I/{0}", c.TipoUsuario);
                //return await _request.PostAsync(ApiUrlBase, c);

                string urlComplementar = string.Format("/I/{0}", e.TipoUsuario);
                return await _request.PostAsync(ApiUrlBase + urlComplementar, e);

            }
            else
                return await _request.PutAsync(ApiUrlBase, e);
        }

        public async Task<UsuarioEmpresa> PutUsuarioPessoaAsync(UsuarioEmpresa e)
        {
            string urlComplementar = string.Format("/U/{0}", e.IdUsuario);
            var result = await _request.PutAsync(ApiUrlBase + urlComplementar, e);
            return result;
        }
    }
}
