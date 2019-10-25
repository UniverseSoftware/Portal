namespace WebSite.Entities.Models
{
    public class UsuarioEmpresa
    {
        public int IdUsuario { get; set; }
        public string UserUsuario { get; set; }
        public string PassUsuario { get; set; }
        public int StatusUsuario { get; set; }
        public int TipoUsuario { get; set; }
        public int IdEP { get; set; }
        public string NomeEP { get; set; }
        public string SnomeEP { get; set; }
        public string CGCEP { get; set; }
        public string TelEP { get; set; }
        public string EmailEP { get; set; }
        public string EndEP { get; set; }

    }
}