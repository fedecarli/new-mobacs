namespace Softpark.WS.ViewModels
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class AtomicTransporViewModel
    {
        public UnicaLotacaoTransportCadastroViewModel cabecalho { get; set; }
        public FichaVisitaDomiciliarChildCadastroViewModel[] visitas { get; set; } = new FichaVisitaDomiciliarChildCadastroViewModel[0];
        public PrimitiveCadastroDomiciliarViewModel[] domicilios { get; set; } = new PrimitiveCadastroDomiciliarViewModel[0];
        public PrimitiveCadastroIndividualViewModel[] individuos { get; set; } = new PrimitiveCadastroIndividualViewModel[0];
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
