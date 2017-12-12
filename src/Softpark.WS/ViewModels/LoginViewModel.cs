namespace Softpark.WS.ViewModels
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class LoginViewModel
    {
        public int CodUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string CNS { get; set; }
        public string INE { get; set; }
        public string CBO { get; set; }
        public string CNES { get; set; }
        public string Mensagem { get; set; }
        public bool Sucesso { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}