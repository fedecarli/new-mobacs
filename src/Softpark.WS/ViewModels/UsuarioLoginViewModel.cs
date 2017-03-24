using System.ComponentModel.DataAnnotations;

namespace Softpark.WS.ViewModels
{
    public class UsuarioLoginViewModel
    {
        [Display(Name = "Login")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Login ou Senha inválidos."), StringLength(80, MinimumLength = 3, ErrorMessage = "Nome de usuário ou senha inválidos.")]
        public string Login { get; set; }

        [Display(Name = "Senha")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Login ou Senha inválidos."), StringLength(80, MinimumLength = 3, ErrorMessage = "Nome de usuário ou senha inválidos.")]
        public string Senha { get; set; }
    }
}
