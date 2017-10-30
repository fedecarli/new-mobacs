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
        public Guid UuidFicha { get; set; }
        
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
            var col = ordCol == 1 ? "e.numero" :
                      ordCol == 2 ? "e.complemento" :
                      ordCol == 3 ? "e.telefoneResidencia" :
                      ordCol == 4 ? "m.id" :
                      "e.nomeLogradouro";

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
            AND (e.nomeLogradouro LIKE '%' + @search + '%'
             OR e.complemento LIKE '%' + @search + '%'
             OR e.telefoneResidencia = @search
             OR e.numero = @search)");
        }
        
        private static string GetUuidCommand(string col, string dir)
        {
            return GetAllCommand(col, dir, $@"
            AND m.id = @search");
        }

        private static string GetAllCommand(string col, string dir, string where = "")
        {
            return $@"
          ;WITH Pagination AS (
		 SELECT COALESCE(e.nomeLogradouro, '') AS NomeLogradouro,
                COALESCE(e.numero, '') AS Numero,
                COALESCE(e.complemento, '') AS Complemento,
                COALESCE(e.telefoneResidencia, '') AS TelefoneResidencia,
                m.id AS UuidFicha,
				(ROW_NUMBER() OVER (ORDER BY {col} {dir})) AS Line
		   FROM api.CadastroDomiciliar AS m
	 INNER JOIN api.EnderecoLocalPermanencia AS e
			 ON m.enderecoLocalPermanencia = e.id
	  LEFT JOIN api.CadastroDomiciliar AS c
			 ON m.id = c.uuidFichaOriginadora
			AND m.id <> c.id
		  WHERE c.id IS NULL{where}
	            )
         SELECT NomeLogradouro,
                Numero,
                Complemento,
                TelefoneResidencia,
                UuidFicha
		   FROM Pagination
		  WHERE Line BETWEEN (@skip + 1) AND (@skip + @take)";
        }
    }
}
