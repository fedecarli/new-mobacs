namespace Softpark.Models
{
    using System;
    
    public partial class AnimalNoDomicilio
    {
        public Guid id_cadastro_domiciliar { get; set; }
        public int id_tp_animal { get; set; }
    
        public virtual CadastroDomiciliar CadastroDomiciliar { get; set; }
    }
}
