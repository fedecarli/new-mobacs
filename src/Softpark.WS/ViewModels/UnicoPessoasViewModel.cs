using System;
using System.ComponentModel.DataAnnotations;

namespace Softpark.WS.ViewModels
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class UnicoPessoasViewModel
    {
        public decimal? CodMunicipe { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Nome { get; set; }
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public string MeioNome { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Loteamento { get; set; }
        public string Numero { get; set; }
        public string andar { get; set; }
        public string apto { get; set; }
        public string Complemento { get; set; }
        public string CEP { get; set; }
        public string Fone { get; set; }
        public string Sexo { get; set; }
        public string RG { get; set; }
        public string ComplRG { get; set; }
        public string SiglaUfIdentidade { get; set; }
        public string OrgaoEmissorIdentidade { get; set; }
        public DateTime? DtEmissaoIdentidade { get; set; }
        public string CNS { get; set; }
        public string CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Zona { get; set; }
        public string Secao { get; set; }
        public string TituloEleitor { get; set; }
        public string EstadoCivil { get; set; }
        public string CD_COD_IBGE { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Co_Tipo_Endereco { get; set; }
        public DateTime? DataDeCadastro { get; set; }
        public string Cor { get; set; }
        public string mae { get; set; }
        public string Origem { get; set; }
        public string Quem { get; set; }
        public string Falecido { get; set; }
        public DateTime? DTFalecimento { get; set; }
        public string Fone1 { get; set; }
        public string Fone2 { get; set; }
        public string Fone3 { get; set; }
        public string Fone4 { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public decimal cd_responsavel { get; set; }
        public string Nome_Responsavel { get; set; }
        public string NIS { get; set; }
        public string PrimeiroNomeSOUNDEX { get; set; }
        public string UltimoNomeSOUNDEX { get; set; }
        public string Ativo { get; set; }
        public string Contato1 { get; set; }
        public string Contato2 { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Homologado { get; set; }
        public string QuemHomologo { get; set; }
        public DateTime? DataHomologacao { get; set; }
        public string UnidadeHomologou { get; set; }
        public byte[] Foto { get; set; }
        public string Status { get; set; }
        public string Matricula { get; set; }
        public int? CodBairro { get; set; }
        public int? CodRua { get; set; }
        public string NumConvenio { get; set; }
        public int? CodConvenio { get; set; }
        public int? Seq_Imovel { get; set; }
        public int? SeqImovelAnt { get; set; }
        public int? ID_Estado_Civil { get; set; }
        public int? ID_RACA { get; set; }
        public int? ID_SEXO { get; set; }
        public int? COD_Unidade { get; set; }
        public string COD_INE { get; set; }
        public string NU_FAMILIA { get; set; }
        public string NU_MICRO_AREA { get; set; }
        public string NU_AREA { get; set; }
        public string NU_SEGMENTO { get; set; }
        public int? NU_LONGITUDE { get; set; }
        public int? NU_LATITUDE { get; set; }
        public int? ID_ESCOLARIDADE { get; set; }
        public int? ID_PAIS { get; set; }
        public string ST_SEM_DOCUMENTO { get; set; }
        public int? ID_OCUPACAO { get; set; }
        public int? ID_NACIONALIDADE { get; set; }
        public int? CodHomologacao { get; set; }
        public string AGUA_RGI { get; set; }
        public string ENERGIA_INSTALACAO { get; set; }
        public string Declaracao_Proprietario { get; set; }
        public int? CodPessoaServidor { get; set; }
        public DateTime? PrazoAtendimento { get; set; }
        public int? RegistroImovel { get; set; }
        public string Contrato_Locacao { get; set; }
        public string NaoTemEmail { get; set; }
        public DateTime? Dt_RegraHomologacao { get; set; }
        public int? CodHomologacaoOld { get; set; }
        public string MotivoHomologacao { get; set; }
        public string IC_BolsaFamilia { get; set; }
        public int? CodHomologacao06112016 { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}