using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Softpark.WS.ViewModels
{
    /// <summary>
    /// ViewModel de token de login
    /// </summary>
    public class UsuarioLoginViewModelToken : UsuarioLoginViewModel
    {
        /// <summary>
        /// Data da geração
        /// </summary>
        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Validador
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return (DateTime.Now - GeneratedAt).TotalHours <= 1;
        }
    }

    /// <summary>
    /// ViewModel de login
    /// </summary>
    public class UsuarioLoginViewModel
    {
        /// <summary>
        /// Login
        /// </summary>
        [Display(Name = "Login")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Login ou Senha inválidos."), StringLength(80, MinimumLength = 3, ErrorMessage = "Nome de usuário ou senha inválidos.")]
        public string Login { get; set; }

        /// <summary>
        /// Senha
        /// </summary>
        [Display(Name = "Senha")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Login ou Senha inválidos."), StringLength(80, MinimumLength = 3, ErrorMessage = "Nome de usuário ou senha inválidos.")]
        public string Senha { get; set; }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <returns></returns>
        public UsuarioLoginViewModelToken ForToken()
        {
            return new UsuarioLoginViewModelToken
            {
                Login = Login,
                Senha = Senha
            };
        }
    }

    /// <summary>
    /// ViewModel de unidade
    /// </summary>
    public class UsuarioLoginUnidadeViewModel
    {
        /// <summary>
        /// ID do usuário
        /// </summary>
        [Required]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// Setor
        /// </summary>
        [Required]
        public int Setor { get; set; }
    }

    /// <summary>
    /// ViewModel de registro
    /// </summary>
    public class UsuarioLoginUnidadeViewModelPost : UsuarioLoginUnidadeViewModel
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        [Display(Description = "Usuário")]
        public string NomeUsuario { get; set; }

        /// <summary>
        /// Setores
        /// </summary>
        public ICollection<SetorDDLViewModel> Setores { get; set; }

        /// <summary>
        /// Dados dos setores
        /// </summary>
        public IEnumerable<SelectListItem> Items => Setores.Select(x => new SelectListItem { Selected = Setor == x.Value, Text = x.Text, Value = x.Value.ToString() });
    }

    /// <summary>
    /// ViewModel de seleção
    /// </summary>
    public class SetorDDLViewModel
    {
        /// <summary>
        /// Valor
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// Descrição
        /// </summary>
        public string Text { get; set; }
    }
}
