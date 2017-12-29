using System;

namespace Softpark.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class VW_CadastroDomiciliar
    {
        /// <summary>
        /// 
        /// </summary>
        public string NomeLogradouro { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Numero { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Complemento { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TelefoneResidencia { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Codigo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? UuidFicha { get; set; }
        
        /// <summary>
        /// Get SQL Search Command
        /// </summary>
        /// <param name="search"></param>
        /// <param name="ordCol"></param>
        /// <param name="ordDir"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public static string GetSearchCommand(string search, int ordCol, int ordDir, int take, int skip)
        {
            var col = ordCol == 1 ? "COALESCE(LTRIM(RTRIM(ae.Numero)) COLLATE Latin1_General_CI_AI, e.numero)" :
                      ordCol == 2 ? "COALESCE(LTRIM(RTRIM(ae.Complemento)) COLLATE Latin1_General_CI_AI, e.complemento)" :
                      ordCol == 3 ? "COALESCE(LTRIM(RTRIM(CAST(tel.DDD AS VARCHAR(3)) + tel.NumTel COLLATE Latin1_General_CI_AI)), e.telefoneResidencia)" :
                      ordCol == 4 ? "cd.id" :
                      "COALESCE(LTRIM(RTRIM(ae.Logradouro)) COLLATE Latin1_General_CI_AI, e.nomeLogradouro)";

            var dir = ordDir == 0 ? "ASC" : "DESC";

            search = search != null && !string.IsNullOrEmpty(search.Trim()) ? search.Trim() : null;
            
            var isUuid = search != null && Guid.TryParse(search, out Guid sGuid);

            return search == null ? GetAllCommand(col, dir) :
                   isUuid ? GetUuidCommand(col, dir) :
                   GetCommonSearch(col, dir);
        }

        private static string GetCommonSearch(string col, string dir)
        {
            return GetAllCommand(col, dir, $@"
            AND (COALESCE(LTRIM(RTRIM(ae.Logradouro)) COLLATE Latin1_General_CI_AI, e.nomeLogradouro) LIKE '%' + @search + '%'
             OR COALESCE(LTRIM(RTRIM(ae.Complemento)) COLLATE Latin1_General_CI_AI, e.complemento) LIKE '%' + @search + '%'
             OR COALESCE(LTRIM(RTRIM(CAST(tel.DDD AS VARCHAR(3)) + tel.NumTel COLLATE Latin1_General_CI_AI)), e.telefoneResidencia) = @search
             OR COALESCE(LTRIM(RTRIM(ae.Numero)) COLLATE Latin1_General_CI_AI, e.numero) = @search)");
        }
        
        private static string GetUuidCommand(string col, string dir)
        {
            return GetAllCommand(col, dir, $@"
            AND cd.id = @search");
        }

        private static string GetAllCommand(string col, string dir, string where = "")
        {
            return $@"
          ;WITH Pagination AS (
		 SELECT COALESCE(LTRIM(RTRIM(ae.Logradouro)) COLLATE Latin1_General_CI_AI, e.nomeLogradouro) AS NomeLogradouro,
                COALESCE(LTRIM(RTRIM(ae.Numero)) COLLATE Latin1_General_CI_AI, e.numero) AS Numero,
                COALESCE(LTRIM(RTRIM(ae.Complemento)) COLLATE Latin1_General_CI_AI, e.complemento) AS Complemento,
                COALESCE(LTRIM(RTRIM(CAST(tel.DDD AS VARCHAR(3)) + tel.NumTel COLLATE Latin1_General_CI_AI)), e.telefoneResidencia) AS TelefoneResidencia,
                cd.id AS UuidFicha,
                ae.Codigo,
				(ROW_NUMBER() OVER (ORDER BY {col} {dir})) AS Line
		   FROM ASSMED_Endereco AS ae
	  LEFT JOIN api.EnderecoLocalPermanencia AS e
			 ON ae.IdFicha = e.id
	 INNER JOIN ASSMED_Cadastro AS c
			 ON ae.Codigo = c.Codigo
	  LEFT JOIN api.IdentificacaoUsuarioCidadao AS i
			 ON c.IdFicha = i.id
	  LEFT JOIN api.CadastroDomiciliar AS cd
			 ON e.id = cd.enderecoLocalPermanencia
	OUTER APPLY (SELECT TOP 1 MAX(ct.IDTelefone) AS [IDTelefone] FROM ASSMED_CadTelefones ct
				  WHERE ct.Codigo = c.Codigo
				    AND ct.TipoTel = 'R'
					AND NumTel IS NOT NULL) AS lt
	  LEFT JOIN ASSMED_CadTelefones AS tel
			 ON tel.IDTelefone = lt.IDTelefone
		  WHERE c.IdFicha IS NULL OR i.statusEhResponsavel = 0{where}
	            )
         SELECT NomeLogradouro,
                Numero,
                Complemento,
                TelefoneResidencia,
                UuidFicha,
                Codigo
		   FROM Pagination
		  WHERE Line BETWEEN (@skip + 1) AND (@skip + @take)";
        }
    }
}
