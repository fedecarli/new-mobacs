namespace Softpark.WS.ViewModels
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class HWV
    {
        public UnicaLotacaoTransportCadastroViewModel Cabecalho { get; set; }
        public FichaVisitaDomiciliarChildCadastroViewModel[] Visitas { get; set; } = new FichaVisitaDomiciliarChildCadastroViewModel[0];
    }
    
    public class HWD
    {
        public UnicaLotacaoTransportCadastroViewModel Cabecalho { get; set; }
        public CadastroDomiciliarViewModel[] Domicilios { get; set; } = new CadastroDomiciliarViewModel[0];
    }
    
    public class HWI
    {
        public UnicaLotacaoTransportCadastroViewModel Cabecalho { get; set; }
        public CadastroIndividualViewModel[] Individuos { get; set; } = new CadastroIndividualViewModel[0];
    }
    
    public class AtomicTransporViewModel
    {
        public HWV[] FichasDeVisitas { get; set; } = new HWV[0];
        public HWD[] CadastrosDomiciliares { get; set; } = new HWD[0];
        public HWI[] CadastrosIndividuais { get; set; } = new HWI[0];
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}