using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Softpark.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Table("SYSLANGUAGES", Schema = "sys")]
    public class Language
    {
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int lcid;
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string name;
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class DomainContainer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public DomainContainer(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        private static DomainContainer _current;

        /// <summary>
        /// 
        /// </summary>
        public Language Language
            => _current.Set<Language>().SqlQuery("SELECT lcid, name FROM SYS.SYSLANGUAGES WHERE NAME =(SELECT @@LANGUAGE)").SingleOrDefault();

        /// <summary>
        /// 
        /// </summary>
        public static DomainContainer Current => InstallApp();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DomainContainer InstallApp()
        {
            var db = _current ?? (_current = new DomainContainer());

            //var dir = HttpContext.Current.Server.MapPath("~/installation");

            //var path = Directory.GetFiles(dir, "api.sql").First();
            //var locked = Directory.GetFiles(dir, "lock").Any();

            //if (File.Exists(path) && !locked)
            //{
            //    db.Database.ExecuteSqlCommand(File.ReadAllText(path));

            //    File.Create($"{dir}/lock").Close();
            //}

            return db;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Reopen()
        {
            _current = new DomainContainer();
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public virtual DbRawSqlQuery<VW_Profissional> VW_Profissional =>
            Database.SqlQuery<VW_Profissional>("SELECT CBO, CNES, CNS, INE, Equipe, Nome, Profissao, Unidade FROM [api].[VW_Profissional]");

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public virtual IQueryable<VW_IdentificacaoUsuarioCidadao_bkp> VW_IdentificacaoUsuarioCidadao_bkp
            => Database
                .SqlQuery<VW_IdentificacaoUsuarioCidadao_bkp>("SELECT id, nomeSocial, codigoIbgeMunicipioNascimento, dataNascimentoCidadao, desconheceNomeMae, emailCidadao, nacionalidadeCidadao, nomeCidadao, nomeMaeCidadao, cnsCidadao, cnsResponsavelFamiliar, telefoneCelular, numeroNisPisPasep, paisNascimento, racaCorCidadao, sexoCidadao, statusEhResponsavel, etnia, num_contrato, nomePaiCidadao, desconheceNomePai, dtNaturalizacao, portariaNaturalizacao, dtEntradaBrasil, microarea, stForaArea, CBO, CNES, INE, Codigo FROM [api].[VW_IdentificacaoUsuarioCidadao] WHERE id IS NOT NULL")
                .AsQueryable();

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public virtual DbRawSqlQuery<VW_ultimo_cadastroDomiciliar_bkp> VW_ultimo_cadastroDomiciliar_bkp
            => Database
                .SqlQuery<VW_ultimo_cadastroDomiciliar_bkp>("SELECT idCadastroDomiciliar, headerTransport, token, idAuto FROM [api].[VW_ultimo_cadastroDomiciliar]");

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public virtual IQueryable<VW_profissional_cns_bkp> VW_profissional_cns_bkp
            => Database
                .SqlQuery<VW_profissional_cns_bkp>("SELECT Codigo, idProfissional, cnsProfissional, cnsCidadao, IdCidadao, CNES, CBO, INE, CodigoCidadao FROM [api].[VW_profissional_cns]")
                .AsQueryable();

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public virtual IQueryable<VW_ultimo_cadastroIndividual_bkp> VW_ultimo_cadastroIndividual_bkp
            => Database
                .SqlQuery<VW_ultimo_cadastroIndividual_bkp>(
                    "SELECT Codigo, idCadastroIndividual, headerTransport, token FROM [api].[VW_ultimo_cadastroIndividual]")
                .AsQueryable();
    }
}
