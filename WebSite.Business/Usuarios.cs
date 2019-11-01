using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Linq;

namespace WebSite.Business
{
    public class Usuarios
    {
        private string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        public bool Erro { get; set; }
        public string MensagemErro { get; set; }

        public Usuarios()
        {
            this.Erro = false;
            this.MensagemErro = string.Empty;
        }

        public Entities.Models.UsuarioEmpresa AutenticaUsuario(string Login, string Senha)
        {
            Entities.Models.UsuarioEmpresa[] usuarios = ListaUsuarios(new Entities.Models.UsuarioEmpresa(Login, Senha));
            Entities.Models.UsuarioEmpresa usuario = usuarios.FirstOrDefault();

            if (usuario == null)
            {
                this.Erro = true;
                this.MensagemErro = "Usuário ou senha inválido";
            }

            return usuario;
        }

        public bool LoginCadastrado(string Login)
        {
            Entities.Models.UsuarioEmpresa[] usuarios = ListaUsuarios(new Entities.Models.UsuarioEmpresa(Login));
            Entities.Models.UsuarioEmpresa usuario = usuarios.FirstOrDefault();

            bool existe = usuario != null && usuario.IdUsuario > 0;

            return existe;
        }

        public Entities.Models.UsuarioEmpresa[] ListaUsuarios()
        {
            return ListaUsuarios(null);
        }

        public Entities.Models.UsuarioEmpresa[] ListaUsuarios(Entities.Models.UsuarioEmpresa usuario)
        {
            List<Entities.Models.UsuarioEmpresa> lstUsuarios = new List<Entities.Models.UsuarioEmpresa>();

            Data.Connection connection = new Data.Connection(this.ConnectionString);
            connection.AbrirConexao();

            StringBuilder sqlString = new StringBuilder();

            sqlString.AppendLine(" SELECT USUARIO.IDUSUARIO AS ID,USUARIO.USERUSUARIO AS USR,CASE WHEN USUARIO.TIPOUSUARIO = 2 THEN PESSOA.NOMEPESSOA WHEN TIPOUSUARIO = 1 THEN EMPRESA.NFANTASIAEMPRESA ELSE 'administrador' END AS NOME,CASE WHEN USUARIO.TIPOUSUARIO = 2 THEN PESSOA.SOBRENOMEPESSOA WHEN TIPOUSUARIO = 1 THEN EMPRESA.RAZAOEMPRESA ELSE 'platpet' END AS SNOME,USUARIO.PASSUSUARIO AS PASS,USUARIO.TIPOUSUARIO AS TIPO,CASE WHEN USUARIO.TIPOUSUARIO = 1 THEN EMPRESA.EMAILEMPRESA WHEN USUARIO.TIPOUSUARIO = 2 THEN PESSOA.EMAILPESSOA ELSE 'UNIVERSE.SOFTWARE.2019@GMAIL.COM'	END EMAIL, STATUSUSUARIO AS STATUS,CASE WHEN USUARIO.TIPOUSUARIO = 2 THEN PESSOA.IDPESSOA WHEN TIPOUSUARIO = 1 THEN EMPRESA.IDEMPRESA ELSE 0 END AS IDEP,CASE WHEN USUARIO.TIPOUSUARIO = 2 THEN PESSOA.CPFPESSOA WHEN TIPOUSUARIO = 1 THEN EMPRESA.CNPJEMPRESA ELSE '' END AS CGC,CASE WHEN USUARIO.TIPOUSUARIO = 2 THEN PESSOA.TELPESSOA WHEN TIPOUSUARIO = 1 THEN EMPRESA.TELEMPRESA ELSE '' END AS TEL,CASE WHEN USUARIO.TIPOUSUARIO = 2 THEN PESSOA.ENDPESSOA WHEN TIPOUSUARIO = 1 THEN EMPRESA.ENDEMPRESA ELSE '' END AS ENDE, TIPOUSUARIO FROM USUARIO LEFT JOIN PESSOA  ON USUARIO.IDUSUARIO = PESSOA.IDUSUARIO LEFT JOIN EMPRESA ON USUARIO.IDUSUARIO = EMPRESA.IDUSUARIO ");

            if (usuario != null)
            {
                sqlString.AppendLine("WHERE 1 = 1");

                if (usuario.IdUsuario > 0)
                {
                    sqlString.AppendLine("AND  USUARIO.IDUSUARIO = " + usuario.IdUsuario + "");
                }

                if (!string.IsNullOrEmpty(usuario.UserUsuario) && usuario.UserUsuario.Length > 0)
                {
                    sqlString.AppendLine("AND USUARIO.USERUSUARIO LIKE '" + usuario.UserUsuario.Replace("'", "''") + "'");
                }

                if (!string.IsNullOrEmpty(usuario.PassUsuario) && usuario.PassUsuario.Length > 0)
                {
                    sqlString.AppendLine("AND USUARIO.PASSUSUARIO = '" + usuario.PassUsuario + "'");
                }
            }

            IDataReader reader = connection.RetornaDados(sqlString.ToString());

            int idxId = reader.GetOrdinal("ID");
            int idxLogin = reader.GetOrdinal("USR");
            int idxSenha = reader.GetOrdinal("PASS");
            int idxNome = reader.GetOrdinal("NOME");
            int idxSnome = reader.GetOrdinal("SNOME");
            int idxCGC = reader.GetOrdinal("CGC");
            int idxEmail = reader.GetOrdinal("EMAIL");
            int idxTel = reader.GetOrdinal("TEL");
            int idxEnde = reader.GetOrdinal("ENDE");
            int idxAtivo = reader.GetOrdinal("STATUS");
            int idxTipo =   reader.GetOrdinal("TIPOUSUARIO");

            while (reader.Read())
            {
                Entities.Models.UsuarioEmpresa _Usuario = new Entities.Models.UsuarioEmpresa();
                _Usuario.IdUsuario = reader.GetInt32(idxId);
                _Usuario.UserUsuario = reader.GetString(idxLogin);
                _Usuario.PassUsuario = reader.GetString(idxSenha);
                _Usuario.NomeEP = reader.GetString(idxNome);
                _Usuario.SnomeEP = reader.GetString(idxSnome);
                _Usuario.CGCEP = reader.GetString(idxCGC);
                _Usuario.EmailEP = reader.GetString(idxEmail);
                _Usuario.TelEP = reader.GetString(idxTel);
                _Usuario.EndEP = reader.GetString(idxEnde);
                _Usuario.StatusUsuario = reader.GetInt32(idxAtivo);
                _Usuario.TipoUsuario = reader.GetInt32(idxTipo);

                lstUsuarios.Add(_Usuario);
            }

            connection.FechaConexao();

            return lstUsuarios.ToArray();
        }

        public bool SalvaUsuario(Entities.Models.UsuarioEmpresa usuario)
        {
            bool salvou = false;

            if (usuario != null)
            {
                Data.Connection connection = new Data.Connection(this.ConnectionString);
                connection.AbrirConexao();

                StringBuilder sqlString = new StringBuilder();

                if (usuario.IdUsuario > 0)
                {
                    sqlString.AppendLine("update usuarios set");
                    sqlString.AppendLine("nome_usuario = '" + usuario.NomeEP.Replace("'", "''") + "',");
                    sqlString.AppendLine("email_usuario = '" + usuario.EmailEP.Replace("'", "''") + "',");
                    sqlString.AppendLine("login_usuario = '" + usuario.UserUsuario.Replace("'", "''") + "',");
                    sqlString.AppendLine("ativo_usuario = " + (usuario.StatusUsuario) + " ");
                    sqlString.AppendLine("where id_usuario = " + usuario.IdUsuario + "");
                }
                else
                {
                    if (!LoginCadastrado(usuario.UserUsuario))
                    {
                        sqlString.AppendLine("insert into usuarios(nome_usuario, email_usuario, login_usuario, senha_usuario, ativo_usuario)");
                        sqlString.AppendLine("values('" + usuario.NomeEP.Replace("'", "''") + "', '" + usuario.EmailEP.Replace("'", "''") + "', '" + usuario.UserUsuario.Replace("'", "''") + "', '" + usuario.PassUsuario.Replace("'", "''") + "', " + (usuario.StatusUsuario) + ")");
                    }
                    else
                    {
                        this.Erro = true;
                        this.MensagemErro = "Login já está sendo utilizado.";
                    }
                }

                int i = connection.ExecutaComando(sqlString.ToString());
                salvou = i > 0;

                connection.FechaConexao();
            }

            return salvou;
        }

        public bool SalvaUsuario(int IdUsuario, string Nome, string Email, string Login, string Senha)
        {
            return SalvaUsuario(new Entities.Models.UsuarioEmpresa(IdUsuario, Nome, Email, Login, Senha));
        }

        public bool ExcluiUsuario(Entities.Models.UsuarioEmpresa usuario)
        {
            bool salvou = false;

            if (usuario != null && usuario.IdUsuario > 0)
            {
                Data.Connection connection = new Data.Connection(this.ConnectionString);
                connection.AbrirConexao();

                StringBuilder sqlString = new StringBuilder();
                sqlString.AppendLine("DELETE FROM USUARIO");
                sqlString.AppendLine("WHERE IDUSUARIO = " + usuario.IdUsuario + "");

                int i = connection.ExecutaComando(sqlString.ToString());

                connection.FechaConexao();
            }

            return salvou;
        }

        public bool ExcluiUsuario(int IdUsuario)
        {
            return ExcluiUsuario(new Entities.Models.UsuarioEmpresa(IdUsuario));
        }
    }
}