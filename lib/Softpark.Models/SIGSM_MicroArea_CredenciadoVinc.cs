namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SIGSM_MicroArea_CredenciadoVinc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SIGSM_MicroArea_CredenciadoVinc()
        {
            SIGSM_MicroArea_CredenciadoCidadao = new HashSet<SIGSM_MicroArea_CredenciadoCidadao>();
        }

        public int id { get; set; }

        public int idMicroAreaUnidade { get; set; }

        public int NumContrato { get; set; }

        public int CodCred { get; set; }

        public int ItemVinc { get; set; }

        public virtual AS_Credenciados AS_Credenciados { get; set; }

        public virtual AS_CredenciadosVinc AS_CredenciadosVinc { get; set; }

        public virtual ASSMED_Contratos ASSMED_Contratos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIGSM_MicroArea_CredenciadoCidadao> SIGSM_MicroArea_CredenciadoCidadao { get; set; }

        public virtual SIGSM_MicroArea_Unidade SIGSM_MicroArea_Unidade { get; set; }
    }
}
