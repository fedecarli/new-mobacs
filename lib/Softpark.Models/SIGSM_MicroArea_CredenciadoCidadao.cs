namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SIGSM_MicroArea_CredenciadoCidadao
    {
        public int id { get; set; }

        public int NumContrato { get; set; }

        public decimal Codigo { get; set; }

        public int idMaCredVinc { get; set; }

        public bool RealizarDownload { get; set; }

        public DateTime? DownloadDomiciliar { get; set; }

        public DateTime? DownloadIndividual { get; set; }

        public virtual ASSMED_Cadastro ASSMED_Cadastro { get; set; }

        public virtual ASSMED_Contratos ASSMED_Contratos { get; set; }

        public virtual SIGSM_MicroArea_CredenciadoVinc SIGSM_MicroArea_CredenciadoVinc { get; set; }
    }
}
