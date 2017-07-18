using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Softpark.WS.ViewModels
{
    /// <summary>
    /// ViewModel do transporte atômico
    /// </summary>
    public class AtomicTransporViewModel
    {
#pragma warning disable IDE1006 // Naming Styles
        /// <summary>
        /// cabeçalho
        /// </summary>
        public UnicaLotacaoTransportCadastroViewModel cabecalho { get; set; }
        /// <summary>
        /// visitas
        /// </summary>
        public FichaVisitaDomiciliarChildCadastroViewModel[] visitas { get; set; } = new FichaVisitaDomiciliarChildCadastroViewModel[0];
        /// <summary>
        /// domicílios
        /// </summary>
        public PrimitiveCadastroDomiciliarViewModel[] domicilios { get; set; } = new PrimitiveCadastroDomiciliarViewModel[0];
        /// <summary>
        /// individuos
        /// </summary>
        public PrimitiveCadastroIndividualViewModel[] individuos { get; set; } = new PrimitiveCadastroIndividualViewModel[0];
#pragma warning restore IDE1006 // Naming Styles
    }
}
