using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Softpark.WS.ViewModels
{
    public class UsuarioLoginViewModelToken : UsuarioLoginViewModel
    {
        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        public bool IsValid()
        {
            return (DateTime.Now - GeneratedAt).TotalHours <= 1;
        }
    }

    public class UsuarioLoginViewModel
    {
        [Display(Name = "Login")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Login ou Senha inválidos."), StringLength(80, MinimumLength = 3, ErrorMessage = "Nome de usuário ou senha inválidos.")]
        public string Login { get; set; }

        [Display(Name = "Senha")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Login ou Senha inválidos."), StringLength(80, MinimumLength = 3, ErrorMessage = "Nome de usuário ou senha inválidos.")]
        public string Senha { get; set; }

        public UsuarioLoginViewModelToken ForToken()
        {
            return new UsuarioLoginViewModelToken
            {
                Login = Login,
                Senha = Senha
            };
        }
    }

    public class UsuarioLoginUnidadeViewModel
    {
        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public int Setor { get; set; }
    }

    public class UsuarioLoginUnidadeViewModelPost : UsuarioLoginUnidadeViewModel
    {
        [Display(Description = "Usuário")]
        public string NomeUsuario { get; set; }

        public ICollection<SetorDDLViewModel> Setores { get; set; }

        public IEnumerable<SelectListItem> Items => Setores.Select(x => new SelectListItem { Selected = Setor == x.Value, Text = x.Text, Value = x.Value.ToString() });
    }

    public class SetorDDLViewModel
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}
