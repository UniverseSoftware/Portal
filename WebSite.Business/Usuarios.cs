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

        public Entities.Usuarios AutenticaUsuario(string Login, string Senha)
        {
            Entities.Usuarios[] usuarios = ListaUsuarios(new Entities.Usuarios(Login, Senha));
            Entities.Usuarios usuario = usuarios.FirstOrDefault();

            if (usuario == null)
            {
                this.Erro = true;
                this.MensagemErro = "Usuário ou senha inválido";
            }

            return usuario;
        }

        public bool LoginCadastrado(string Login)
        {
            Entities.Usuarios[] usuarios = ListaUsuarios(new Entities.Usuarios(Login));
            Entities.Usuarios usuario = usuarios.FirstOrDefault();

            bool existe = usuario != null && usuario.IdUsuario > 0;

            return existe;
        }

        public Entities.Usuarios[] ListaUsuarios()
        {
            return ListaUsuarios(null);
        }

        public Entities.Usuarios[] ListaUsuarios(Entities.Usuarios usuario)
        {
            List<Entities.Usuarios> lstUsuarios = new List<Entities.Usuarios>();

            Data.Connection connection = new Data.Connection(this.ConnectionString);
            connection.AbrirConexao();

            StringBuilder sqlString = new StringBuilder();

            sqlString.AppendLine(" SELECT USUARIO.IDUSUARIO AS ID,USUARIO.USERUSUARIO AS USR,CASE WHEN USUARIO.TIPOUSUARIO = 2 THEN PESSOA.NOMEPESSOA WHEN TIPOUSUARIO = 1 THEN EMPRESA.NFANTASIAEMPRESA ELSE 'administrador' END AS NOME,CASE WHEN USUARIO.TIPOUSUARIO = 2 THEN PESSOA.SOBRENOMEPESSOA WHEN TIPOUSUARIO = 1 THEN EMPRESA.RAZAOEMPRESA ELSE 'platpet' END AS SNOME,USUARIO.PASSUSUARIO AS PASS,USUARIO.TIPOUSUARIO AS TIPO,CASE WHEN USUARIO.TIPOUSUARIO = 1 THEN EMPRESA.EMAILEMPRESA WHEN USUARIO.TIPOUSUARIO = 2 THEN PESSOA.EMAILPESSOA ELSE 'UNIVERSE.SOFTWARE.2019@GMAIL.COM'	END EMAIL, STATUSUSUARIO AS STATUS,CASE WHEN USUARIO.TIPOUSUARIO = 2 THEN PESSOA.IDPESSOA WHEN TIPOUSUARIO = 1 THEN EMPRESA.IDEMPRESA ELSE 0 END AS IDEP,CASE WHEN USUARIO.TIPOUSUARIO = 2 THEN PESSOA.CPFPESSOA WHEN TIPOUSUARIO = 1 THEN EMPRESA.CNPJEMPRESA ELSE '' END AS CGC,CASE WHEN USUARIO.TIPOUSUARIO = 2 THEN PESSOA.TELPESSOA WHEN TIPOUSUARIO = 1 THEN EMPRESA.TELEMPRESA ELSE '' END AS TEL,CASE WHEN USUARIO.TIPOUSUARIO = 2 THEN PESSOA.ENDPESSOA WHEN TIPOUSUARIO = 1 THEN EMPRESA.ENDEMPRESA ELSE '' END AS ENDE, TIPOUSUARIO FROM USUARIO LEFT JOIN PESSOA  ON USUARIO.IDUSUARIO = PESSOA.IDUSUARIO LEFT JOIN EMPRESA ON USUARIO.IDUSUARIO = EMPRESA.IDUSUARIO ");

            if (usuario != null)
            {
                sqlString.AppendLine("WHERE 1 = 1");

                //if (usuario.IdUsuario > 0)
                //{
                //    sqlString.AppendLine("AND  USUARIO.IDUSUARIO = " + usuario.IdUsuario + "");
                //}

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
            int idxNome = reader.GetOrdinal("USR");
            int idxEmail = reader.GetOrdinal("EMAIL");
            int idxLogin = reader.GetOrdinal("USR");
            int idxSenha = reader.GetOrdinal("PASS");
            int idxAtivo = reader.GetOrdinal("STATUS");
            int idxTipo =   reader.GetOrdinal("TIPOUSUARIO");

            while (reader.Read())
            {
                Entities.Usuarios _Usuario = new Entities.Usuarios();
                _Usuario.IdUsuario = reader.GetInt32(idxId);
                _Usuario.NomeEP = reader.GetString(idxNome);
                _Usuario.EmailEP = reader.GetString(idxEmail);
                _Usuario.UserUsuario = reader.GetString(idxLogin);
                _Usuario.PassUsuario = reader.GetString(idxSenha);
                _Usuario.StatusUsuario = reader.GetInt32(idxAtivo);
                _Usuario.TipoUsuario = reader.GetInt32(idxTipo);

                lstUsuarios.Add(_Usuario);
            }

            connection.FechaConexao();

            return lstUsuarios.ToArray();
        }

        public bool SalvaUsuario(Entities.Usuarios usuario)
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
            return SalvaUsuario(new Entities.Usuarios(IdUsuario, Nome, Email, Login, Senha));
        }

        public bool ExcluiUsuario(Entities.Usuarios usuario)
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
            return ExcluiUsuario(new Entities.Usuarios(IdUsuario));
        }
    }
}