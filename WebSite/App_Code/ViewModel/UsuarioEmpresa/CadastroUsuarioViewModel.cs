using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebSite.App_Code.Models;
using System.Windows.Input;
using WebSite.App_Code.Services.UsuarioEmpresas;

namespace WebSite.App_Code.ViewModel.CadastroEmpresa
{
    public class CadastroUsuarioViewModel : BaseViewModel
    {
        private IUsuarioEmpresaService uService = new UsuarioEmpresaService();

        private UsuarioEmpresa UsuarioEmpresa;
        public ICommand GravarCommand { get; set; }
        public ICommand NovoCommand { get; set; }


        public CadastroUsuarioViewModel()
        {
            RegistrarCommands();
            UsuarioEmpresa = new UsuarioEmpresa();
        }

        private void RegistrarCommands()
        {
            GravarCommand = new Command(async () =>
            {
                await GravarAsync();
                MessagingCenter.Send<string>("Dado salvo com sucesso.", "InformacaoCRUD");
            });
        }

        private async Task GravarAsync()
        {
            var ehNovoUsuario = (UsuarioEmpresa.IdEP == 0 ? true : false);
            UsuarioEmpresa.TipoUsuario = 1;
            UsuarioEmpresa.StatusUsuario = 1;
            await uService.PostUsuarioPessoaAsync(UsuarioEmpresa);

            //Chamada ao método que limpa os campos da tela
            AtualizarPropriedadesParaVisao(ehNovoUsuario);
        }

        //Método que limpa as propriedades da ViewModel, que por sua vez, limpa a View
        private void AtualizarPropriedadesParaVisao(bool ehNovoObjeto)
        {
            if (ehNovoObjeto)
            {
                this.Nome = string.Empty;
                this.Sobrenome = string.Empty;
                this.Usuario = string.Empty;
                this.Senha = string.Empty;
                this.Nome = string.Empty;
                this.Sobrenome = string.Empty;
                this.CPF = string.Empty;
                this.Telefone = string.Empty;
                this.Email = string.Empty;
                this.Endereco = string.Empty;
                this.UsuarioEmpresa = new UsuarioEmpresa();
            }
        }

        public string Usuario
        {
            get { return this.UsuarioEmpresa.UserUsuario; }
            set
            {
                this.UsuarioEmpresa.UserUsuario = value;
                OnPropertyChanged();
            }
        }

        public string Nome
        {
            get { return this.UsuarioEmpresa.NomeEP; }
            set
            {
                //Atribuirá valor a propriedade
                this.UsuarioEmpresa.NomeEP = value;

                //Atuálizará a propriedade ligada a view
                //Método presente na classe herdada
                OnPropertyChanged();
            }
        }

        public string Sobrenome
        {
            get { return this.UsuarioEmpresa.SnomeEP; }
            set
            {
                this.UsuarioEmpresa.SnomeEP = value;
                OnPropertyChanged();
            }
        }

        public string Senha
        {
            get { return this.UsuarioEmpresa.PassUsuario; }
            set
            {
                this.UsuarioEmpresa.PassUsuario = value;
                OnPropertyChanged();
            }
        }

        public string CPF
        {
            get { return this.UsuarioEmpresa.CGCEP; }
            set
            {
                this.UsuarioEmpresa.CGCEP = value;
                OnPropertyChanged();
            }
        }

        public string Telefone
        {
            get { return this.UsuarioEmpresa.TelEP; }
            set
            {
                this.UsuarioEmpresa.TelEP = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return this.UsuarioEmpresa.EmailEP; }
            set
            {
                this.UsuarioEmpresa.EmailEP = value;
                OnPropertyChanged();
            }
        }

        public string Endereco
        {
            get { return this.UsuarioEmpresa.EndEP; }
            set
            {
                this.UsuarioEmpresa.EndEP = value;
                OnPropertyChanged();
            }
        }

    }
}