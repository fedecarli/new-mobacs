using Softpark.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Softpark.WS.ViewModels
{
    /// <summary>
    /// Dados do Profissional
    /// </summary>
    public class DadosAcessoViewModel
    {
        /// <summary>
        /// Token de Acesso
        /// </summary>
        public Guid TokenAcesso { get; set; }

        /// <summary>
        /// Dados do profissional
        /// </summary>
        public VW_Profissional DadosAtrelados { get; set; }

        /// <summary>
        /// Data limite de validade do token (atualizado à cada consulta que requer o uso do token)
        /// </summary>
        public DateTime ValidoAte { get; internal set; }
    }

    /// <summary>
    /// Dados de autenticação de ACSs
    /// </summary>
    public class AutenticacaoViewModel
    {
        /// <summary>
        /// Nome de usuário ou CNS do profissional
        /// </summary>
        [Required]
        public string Usuario { get; set; }

        /// <summary>
        /// Senha de acesso ao sistema
        /// </summary>
        [Required]
        public string Senha { get; set; }

        /// <summary>
        /// Código da Unidade
        /// </summary>
        [Required]
        public string CNES { get; set; }
    }
}
