//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SIGSM_Crianca_Seis_a_23_Meses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SIGSM_Crianca_Seis_a_23_Meses()
        {
            this.SIGSM_Marcadores_Consumo_Alimentar = new HashSet<SIGSM_Marcadores_Consumo_Alimentar>();
        }
    
        public long id { get; set; }
        public string a_crianca_ontem_tomou_leite_do_peito { get; set; }
        public string ontem_a_crianca_comeu_fruta_inteira_em_pedaco_amassada { get; set; }
        public string quantas_vezes_ontem_a_crianca_comeu_fruta_inteira_em_pedaco_amassada { get; set; }
        public string ontem_a_crianca_comeu_comida_de_sal { get; set; }
        public string quantas_vezes_ontem_a_crianca_comeu_comida_de_sal { get; set; }
        public string comida_oferecida { get; set; }
        public string ontem_a_crianca_consumiu_outro_leite_nao_de_peito { get; set; }
        public string ontem_a_crianca_consumiu_mingau_com_leite { get; set; }
        public string ontem_a_crianca_consumiu_iogurte { get; set; }
        public string ontem_a_crianca_consumiu_legumes { get; set; }
        public string ontem_a_crianca_consumiu_vegetal_ou_fruta_de_cor_alaranjada { get; set; }
        public string ontem_a_crianca_consumiu_verdura_de_folha { get; set; }
        public string ontem_a_crianca_consumiu_carne { get; set; }
        public string ontem_a_crianca_consumiu_figado { get; set; }
        public string ontem_a_crianca_consumiu_feijao { get; set; }
        public string ontem_a_crianca_consumiu_arroz_batata_inhame_aipim_farinha_macarrao_sem_ser_instataneo { get; set; }
        public string ontem_a_crianca_consumiu_hamburguer_e_ou_embutidos { get; set; }
        public string ontem_a_crianca_consumiu_bebidas_adocadas { get; set; }
        public string ontem_a_crianca_consumiu_macarrao_instantaneo_salgadinhos_de_pacote_biscoitos_salgados { get; set; }
        public string ontem_a_crianca_consumiu_biscoito_recheado_doces { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_Marcadores_Consumo_Alimentar> SIGSM_Marcadores_Consumo_Alimentar { get; set; }
    }
}
