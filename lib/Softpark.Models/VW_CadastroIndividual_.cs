using System;
using System.Text.RegularExpressions;
using Softpark.Infrastructure.Extensions;

namespace Softpark.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class VW_CadastroIndividual
    {
        /// <summary>
        /// 
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DtNasc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NomeMae { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Cns { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NomeCidade { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Get SQL Search Command
        /// </summary>
        /// <param name="search"></param>
        /// <param name="ordCol"></param>
        /// <param name="ordDir"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="has_vuci"></param>
        /// <returns></returns>
        public static string GetSearchCommand(string search, int ordCol, int ordDir, int take, int skip, bool has_vuci)
        {
            var col = ordCol == 1 ? "COALESCE(pf.DtNasc, pf2.DtNasc)" :
                      ordCol == 2 ? "COALESCE(pf.NomeMae, pf2.NomeMae) COLLATE Latin1_General_CI_AI" :
                      ordCol == 3 ? "cns.Numero COLLATE Latin1_General_CI_AI" :
                      ordCol == 4 ? "cid.NomeCidade COLLATE Latin1_General_CI_AI" :
                      "ac.Nome COLLATE Latin1_General_CI_AI";

            var dir = ordDir == 0 ? "ASC" : "DESC";

            search = search != null && !string.IsNullOrEmpty(search.Trim()) ? search.Trim() : null;

            var isGuid = search != null && Guid.TryParse(search, out Guid sGuid);
            var isDate = search != null && Regex.IsMatch(search, "^([0-9]{2}/[0-9]{2}/[0-9]{4})$", RegexOptions.IgnoreCase);
            var isCns = search != null && search.isValidCns();
            var isNum = search != null && decimal.TryParse(search, out decimal cns);

            return search == null ? GetAllCommand(col, dir, has_vuci) :
                   isGuid ? GetGuidCommand(col, dir, has_vuci) :
                   isDate ? GetDateCommand(col, dir, has_vuci) :
                   isCns ? GetCnsCommand(col, dir, has_vuci) :
                   isNum ? GetNumCommand(col, dir, has_vuci) :
                   GetCommonSearch(col, dir, has_vuci);
        }

        private static string GetCommonSearch(string col, string dir, bool has_vuci)
        {
            return GetAllCommand(col, dir, has_vuci, $@"
            AND (cid.NomeCidade COLLATE Latin1_General_CI_AI LIKE '%' + @search + '%'
             OR ac.Nome COLLATE Latin1_General_CI_AI LIKE '%' + @search + '%'
             OR COALESCE(pf.NomeMae, pf2.NomeMae) COLLATE Latin1_General_CI_AI LIKE '%' + @search + '%')");
        }

        private static string GetDateCommand(string col, string dir, bool has_vuci)
        {
            return GetAllCommand(col, dir, has_vuci, $@"
            AND CONVERT(VARCHAR,(CONVERT(DATE,COALESCE(pf.DtNasc, pf2.DtNasc),103)),103) = @search");
        }

        private static string GetCnsCommand(string col, string dir, bool has_vuci)
        {
            return GetAllCommand(col, dir, has_vuci, $@"
            AND cns.Numero COLLATE Latin1_General_CI_AI = @search COLLATE Latin1_General_CI_AI");
        }

        private static string GetNumCommand(string col, string dir, bool has_vuci)
        {
            return GetAllCommand(col, dir, has_vuci, $@"
            AND ac.Codigo = @search");
        }

        private static string GetGuidCommand(string col, string dir, bool has_vuci)
        {
            return GetAllCommand(col, dir, has_vuci, @"
            AND ci.id = @search");
        }

        private static string GetAllCommand(string col, string dir, bool has_vuci, string where = "")
        {
            return $@"
          ;WITH Pagination AS (
	 SELECT DISTINCT ac.Nome COLLATE Latin1_General_CI_AI AS [Nome],
			CONVERT(VARCHAR,(CONVERT(DATE,COALESCE(pf.DtNasc, pf2.DtNasc),103)),103) AS DtNasc,
			COALESCE(pf.NomeMae, pf2.NomeMae) COLLATE Latin1_General_CI_AI AS [NomeMae],
			SUBSTRING(LTRIM(RTRIM(cns.Numero)), 1, 15) COLLATE Latin1_General_CI_AI AS Cns,
			cid.NomeCidade COLLATE Latin1_General_CI_AI AS [NomeCidade],
			CAST(CAST(ac.Codigo AS INT) AS NVARCHAR(MAX)) AS Codigo,
			ROW_NUMBER() OVER (ORDER BY {col} {dir}) AS Line
	   FROM ASSMED_Cadastro AS ac
  LEFT JOIN ASSMED_PesFisica AS pf
		 ON pf.Codigo = ac.Codigo
  LEFT JOIN ASSMED_CadastroPF AS pf2
		 ON pf2.Codigo = ac.Codigo
  LEFT JOIN ASSMED_CadastroDocPessoal AS cns
		 ON ac.Codigo = cns.Codigo
		AND cns.CodTpDocP = 6
  LEFT JOIN Cidade as cid
		 ON pf.MUNICIPIONASC = cid.CodCidade
		 OR pf2.MUNICIPIONASC = cid.CodCidade
  LEFT JOIN api.IdentificacaoUsuarioCidadao AS iden
         ON ac.IdFicha = iden.id
         OR ac.Codigo = iden.Codigo
  LEFT JOIN api.CadastroIndividual AS ci
         ON iden.Id = ci.identificacaoUsuarioCidadao
	  WHERE ac.Nome IS NOT NULL
		AND LEN(RTRIM(LTRIM(ac.Nome COLLATE Latin1_General_CI_AI))) > 0
		AND ac.Nome COLLATE Latin1_General_CI_AI NOT LIKE '%*%'{where}
	            )
         SELECT DISTINCT Nome,
			    DtNasc,
			    NomeMae,
			    Cns,
			    NomeCidade,
			    Codigo
		   FROM Pagination
		  WHERE Line BETWEEN (@skip + 1) AND (@skip + @take)";
        }
    }
}
