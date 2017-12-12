using System.Text.RegularExpressions;

namespace Softpark.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class VW_FichasMaster
    {
        /// <summary>
        /// 
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public string Profissional { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public string Profissao { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public string Equipe { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public string Unidade { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public string Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UuidFicha { get; set; }

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
            var col = ordCol == 1 ? nameof(Profissional) :
                      ordCol == 2 ? nameof(Profissao) :
                      ordCol == 3 ? nameof(Equipe) :
                      ordCol == 4 ? nameof(Unidade) :
                      ordCol == 5 ? nameof(Status) :
                      nameof(Data);

            var dir = ordDir == 0 ? "ASC" : "DESC";

            search = search != null && !string.IsNullOrEmpty(search.Trim()) ? search.Trim() : null;

            var isUuid = search != null && Regex.IsMatch(search, "^([0-9]{7}-[0-9A-F]{8}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{12})$", RegexOptions.IgnoreCase);

            var isDate = search != null && Regex.IsMatch(search, "^([0-9]{2}/[0-9]{2}/[0-9]{4})$", RegexOptions.IgnoreCase);

            return search == null ? GetAllCommand(col, dir) :
                   isUuid ? GetUuidCommand(col, dir) :
                   isDate ? GetDateCommand(col, dir) :
                   GetCommonSearch(col, dir);
        }

        private static string GetCommonSearch(string col, string dir)
        {
            return GetAllCommand(col, dir, $@"
            AND ((CASE WHEN o.enviado = 1 THEN 'Enviada' WHEN o.finalizado = 1 THEN 'Finalizada' ELSE 'Pendente' END) LIKE '%' + @search + '%'
             OR p.Nome LIKE '%' + @search + '%'
             OR p.CNS = @search)");
        }

        private static string GetDateCommand(string col, string dir)
        {
            return GetAllCommand(col, dir, $@"
            AND CONVERT(VARCHAR,(CONVERT(DATE,u.dataAtendimento,103)),103) = @search");
        }

        private static string GetUuidCommand(string col, string dir)
        {
            return GetAllCommand(col, dir, $@"
            AND m.uuidFicha = @search");
        }

        private static string GetAllCommand(string col, string dir, string where = "")
        {
            var col2 = (col == "Profissional" ? $"p.CNS {dir}, p.Nome " :
                        col == "Profissao" ? $"p.CBO {dir}, p.Profissao " :
                        col == "Equipe" ? $"p.INE {dir}, p.Equipe" :
                        col == "Unidade" ? $"p.CNES {dir}, p.Unidade" :
                        col == "Status" ? "(CASE WHEN o.enviado = 1 THEN 'Enviada' WHEN o.finalizado = 1 THEN 'Finalizada' ELSE 'Pendente' END)" :
                        "u.dataAtendimento");

            return $@"
          ;WITH Pagination AS (
		 SELECT CONVERT(VARCHAR,(CONVERT(DATE,u.dataAtendimento,103)),103) AS [Data],
				(p.CNS + '|' + p.Nome) AS Profissional,
                (p.CBO + '|' + p.Profissao) AS Profissao,
                (p.INE + '|' + p.Equipe) AS Equipe,
                (p.CNES + '|' + p.Unidade) AS Unidade,
				(CASE WHEN o.enviado = 1 THEN 'Enviada' WHEN o.finalizado = 1 THEN 'Finalizada' ELSE 'Pendente' END) AS [Status],
				m.uuidFicha AS UuidFicha,
				(ROW_NUMBER() OVER (ORDER BY {col2} {dir})) AS Line
		   FROM api.FichaVisitaDomiciliarMaster AS m
	 INNER JOIN api.UnicaLotacaoTransport AS u
			 ON m.headerTransport = u.id
	 INNER JOIN api.VW_Profissional AS p
			 ON u.cboCodigo_2002 = p.CBO
			AND u.cnes = p.CNES
			AND u.ine = p.INE
			AND u.profissionalCNS = p.CNS
	 INNER JOIN api.OrigemVisita AS o
			 ON u.token = o.token
          WHERE p.CNES = @cnes{where}
	            )
         SELECT Data,
                Profissional,
                Profissao,
                Equipe,
                Unidade,
                Status,
                UuidFicha
		   FROM Pagination
		  WHERE Line BETWEEN (@skip + 1) AND (@skip + @take)";
        }
    }
}
