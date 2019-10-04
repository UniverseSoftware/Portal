using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class painel_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnAutenticar_Click(object sender, EventArgs e)
    {
        string Login = txtUsuario.Text;
        string Senha = txtSenha.Text;

        Senha = FormsAuthentication.HashPasswordForStoringInConfigFile(Senha, "MD5");

        WebSite.Business.Usuarios usuariosBO = new WebSite.Business.Usuarios();
        WebSite.Entities.Usuarios usuario = usuariosBO.AutenticaUsuario(Login, Senha);

        if (!usuariosBO.Erro && usuario.IdUsuario > 0)
        {
            Session.Add("PainelAutenticado", true);
            Session.Add("Usuario", usuario);
            if (usuario.TipoUsuario == 0)
            {
                Response.Redirect("default.aspx");//Quanto for Admin mandar para o gerenciamento da empresa.
            }
            else
            {
                Response.Redirect("default.apsx");//Quando for Empresa mandar para gestão da empresa.
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "msgbox", "alert('" + usuariosBO.MensagemErro + "')", true);
        }
    }
}