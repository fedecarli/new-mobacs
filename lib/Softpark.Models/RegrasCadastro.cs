using Softpark.Infrastructure.Extensions;
using Softpark.Infrastructure.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Softpark.Models
{
    /// <summary>
    /// Regras de cadastro
    /// </summary>
    public static class RegrasCadastro
    {
        /// <summary>
        /// limpa string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string text)
        {
            text = Regex.Replace(Regex.Replace(text, "([Á-Ä])", "A"), "([á-ä])", "a");
            text = Regex.Replace(Regex.Replace(text, "([É-Ë])", "E"), "([é-ë])", "e");
            text = Regex.Replace(Regex.Replace(text, "([Í-Ï])", "I"), "([í-ï])", "i");
            text = Regex.Replace(Regex.Replace(text, "([Ó-Ö])", "O"), "([ó-ö])", "o");
            text = Regex.Replace(Regex.Replace(text, "([Ú-Ü])", "U"), "([ú-ü])", "u");
            text = Regex.Replace(Regex.Replace(text, "([Ç])", "C"), "([ç])", "c");

            return text;
        }

        /// <summary>
        /// Valida as acentuações em nomes
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsValidDiatricts(this string text)
        {
            return !Regex.IsMatch(text, "([^a-zá-äé-ëí-ïó-öú-üA-ZÁ-ÄÉ-ËÍ-ÏÓ-ÖÚ-ÜÇç '])");
        }

        /// <summary>
        /// divide string em palavras
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string[] SplitWords(this string text)
        {
            return text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// valida nome
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public static List<string> NomeValido(this string nome, string form, string field, bool required)
        {
            var errors = new List<string>();

            if (nome == null && required)
                errors.Add($"{form}, o {field} é obrigatório.");
            else if (nome != null)
            {
                nome = nome.Trim().RemoveDiacritics().ToUpperInvariant();
                var words = nome.SplitWords().ToArray();

                if (nome.Length < 3 || nome.Length > 70)
                    errors.Add($"{form}, o {field} deve ter ao menos 3 caracteres e no máximo 70 caracteres.");

                if (words.Length == 1)
                    errors.Add($"{form}, o {field} deve ter ao menos 2 termos (ex. JOSÉ MARIA é admissível).");

                var ws = words.Skip(1).Where(x => x.Length == 1).ToArray();

                if (ws.Length > 0 && ws.Any(x => x != "E" && x != "Y"))
                    errors.Add($"{form}, no {field}, o termo com apenas uma letra devem ser E ou Y (ex. JOSÉ MARIA E SILVA ou JOSÉ MARIA Y RODRIGUEZ são admissíveis).");

                if (!nome.IsValidDiatricts())
                    errors.Add($"{form}, o {field} só aceita letras, espaços, apóstrofo ('), acentos (agudo, circunflexo, til e trema) e 'ç' (C com cedilha), algarismos não romanos não serão admitidos (ex. JOÃO D'ÁVILA é admissível).");

                if (words.Length >= 2 && words[0].Length == 1 && words[1].Length == 1)
                    errors.Add($"{form}, o {field} não deve começar com dois termos com uma única letra (ex. A A DA SILVA não é admissível).");

                if (words.Length == 2 && words.All(x => x.Length == 2))
                    errors.Add($"{form}, o {field} não deve possuir dois termos com apenas duas letras cada (ex. AD JR não é admissível).");
            }

            // TODO - é preciso adicionar as regras que estão no documento - http://esusab.github.io/integracao/docs/dicionario/Especificacao_CADSUS.pdf

            return errors;
        }

        /// <summary>
        /// valida cabeçalho
        /// </summary>
        /// <param name="header"></param>
        /// <param name="domain"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this UnicaLotacaoTransport header, DomainContainer domain)
        {
            var errors = new List<string>();

            if (header.profissionalCNS == null || !header.profissionalCNS.isValidCns())
                errors.Add("O CNS do profissional é inválido.");
            else
            {
                var profissional = domain.VW_Profissional.Where(x => x.CNS != null && x.CNS.Trim() == header.profissionalCNS.Trim()).ToArray();

                if (profissional.Length == 0)
                    errors.Add("O profissional informado não foi encontrado.");
                else
                {
                    if (profissional.All(x => x.CBO == null || x.CBO.Trim() != header?.cboCodigo_2002?.Trim()))
                        errors.Add("O CBO do profissional não foi encontrado.");

                    if (profissional.All(x => x.CNES == null || x.CNES.Trim() != header?.cnes?.Trim()))
                        errors.Add("A unidade do profissional não foi encontrada.");

                    if (header.ine != null && profissional.All(x => x.INE == null || x.INE?.Trim() != header?.ine?.Trim()))
                        errors.Add("A equipe do profissional não foi encontrada.");

                    var validEpoch = Epoch.ValidateESUSDateTime(header.dataAtendimento);

                    if (validEpoch != ValidationResult.Success)
                        errors.Add("A data do atendimento é inválida.");

                    if (domain.Cidade.All(x => x.CodIbge == null || x.CodIbge.Trim() != header.codigoIbgeMunicipio.Trim()))
                        errors.Add("O município de atendimento não foi encontrado.");
                }
            }

            ThrowErrors(errors);
        }

        private static void ThrowErrors(List<string> errors)
        {
            if (errors.Count > 0)
                throw new ValidationException(errors.Aggregate("", (a, b) => a + b + "<br/>\n"));
        }

        /// <summary>
        /// valida ficha master
        /// </summary>
        /// <param name="master"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this FichaVisitaDomiciliarMaster master)
        {
            var errors = new List<string>();

            if (master.UnicaLotacaoTransport == null)
                errors.Add("O cabeçalho de atendimento não foi encontrado.");

            if (master.uuidFicha == null || master.uuidFicha.Trim().Length != 44)
                errors.Add("O código da ficha é inválido.");

            if (master.uuidFicha != null && master.uuidFicha.Trim().Length == 44 && master.uuidFicha.Substring(0, 7) != master
                .UnicaLotacaoTransport.cnes)
                errors.Add("O código da ficha é inválido. O CNES presente no código não corresponde ao cabeçalho.");

            ThrowErrors(errors);
        }

        /// <summary>
        /// valida ficha child
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this FichaVisitaDomiciliarChild child)
        {
            var errors = new List<string>();

            var master = child.FichaVisitaDomiciliarMaster;

            if (master == null)
                errors.Add("Ficha master não encontrada.");

            if (child.turno < 1 || child.turno > 3)
                errors.Add("Turno inválido.");

            var proibido = (new long[] { 2, 3, 4, 5, 6, 12 }).Contains(child.tipoDeImovel);

            var obrigatorio = (new long[] { 1, 7, 8, 9, 10, 11, 99 }).Contains(child.tipoDeImovel) &&
                (child.SIGSM_MotivoVisita.Any(x => x.observacoes == "#BUSCA_ATIVA" || x.observacoes == "#ACOMPANHAMENTO"
                || x.codigo == 25 || x.codigo == 31) || (child.pesoAcompanhamentoNutricional ?? 0 + child.alturaAcompanhamentoNutricional ?? 0) > 0);

            if (proibido && child.numProntuario != null)
                errors.Add("O número do prontuário não deve ser fornecido para o tipo de imóvel selecionado.");

            if (child.numProntuario != null && !Regex.IsMatch(child.numProntuario, "^([a-zA-Z0-9]+)$"))
                errors.Add("O número do prontuário é inválido.");

            if (child.cnsCidadao != null && proibido)
                errors.Add("O CNS do cidadão não deve ser fornecido para o tipo de imóvel selecionado.");

            if (child.cnsCidadao != null && !child.cnsCidadao.isValidCns())
                errors.Add("O CNS do cidadão é inválido");

            if (child.dtNascimento != null && proibido)
                errors.Add("A data de nascimento do cidadão não deve ser fornecida para o tipo de imóvel selecionado.");

            if (child.dtNascimento != null && !child.dtNascimento.Value.IsValidBirthDateTime(child.FichaVisitaDomiciliarMaster.UnicaLotacaoTransport.dataAtendimento))
                errors.Add("A data de nascimento do cidadão é inválida.");

            if (child.dtNascimento == null && obrigatorio)
                errors.Add("A data de nascimento do cidadão é obrigatória.");

            if (child.sexo != null && proibido)
                errors.Add("O sexo do cidadão não deve ser informado para o tipo de imóvel selecionado.");

            if (child.sexo == null && obrigatorio)
                errors.Add("O sexo do cidadão é obrigatório.");

            if (child.sexo != null && !(new long?[] { 0, 1, 4 }).Contains(child.sexo))
                errors.Add("O sexo do cidadão é inválido.");

            if (child.SIGSM_MotivoVisita.Count > 0)
            {
                if (child.desfecho == 2 || child.desfecho == 3)
                    errors.Add("Os motivos de visita não devem ser informados para o desfecho selecionado.");

                var motivos = new long[] { 1, 34, 35, 36, 37, 27, 31, 28 };

                if (proibido && child.SIGSM_MotivoVisita.Any(x => !motivos.Contains(x.codigo)))
                    errors.Add("Um ou mais motivos de visitas não podem ser informados para o tipo de imóvel selecionado.");

                if (child.SIGSM_MotivoVisita.Count > 36)
                    errors.Add("Só é possível selecionar 36 motivos de visita.");
            }

            if (child.desfecho < 1 || child.desfecho > 3)
                errors.Add("O desfecho informado é inválido.");

            if (child.stForaArea && child.microarea != null)
                errors.Add("A microárea não pode ser informada quando a visita for fora de área.");

            if (child.microarea != null && (child.microarea.Trim().Length != 2 || !Regex.IsMatch(child.microarea.Trim(), "^([0-9][0-9])$")))
                errors.Add("A microárea deve ser informada com 2 numéros.");

            if (child.tipoDeImovel < 1 || (child.tipoDeImovel > 12 && child.tipoDeImovel != 99))
                errors.Add("O tipo de imóvel informado é inválido.");

            if (child.pesoAcompanhamentoNutricional != null)
            {
                if (proibido)
                    errors.Add("O peso não deve ser informado para o tipo de imóvel selecionado.");

                if (child.desfecho == 3 || child.desfecho == 2)
                    errors.Add("O peso não deve ser informado para o desfecho selecionado.");

                var parts = child.pesoAcompanhamentoNutricional.ToString().Replace(',', '.').Split('.');

                var validParts = parts.Length < 2 || (parts.Length == 2 && parts[1].Length <= 3);
                var validSize = child.pesoAcompanhamentoNutricional >= 0.5m && child.pesoAcompanhamentoNutricional <= 500;

                if (!validParts || !validSize)
                    errors.Add("O peso é inválido, informe um valor entre '0.500' e '500.000'.");
            }

            if (child.alturaAcompanhamentoNutricional != null)
            {
                if (proibido)
                    errors.Add("A altura não deve ser informada para o tipo de imóvel selecionado.");

                if (child.desfecho == 3 || child.desfecho == 2)
                    errors.Add("A altura não deve ser informada para o desfecho selecionado.");

                var parts = child.alturaAcompanhamentoNutricional.ToString().Replace(',', '.').Split('.');

                var validParts = parts.Length < 2 || (parts.Length == 2 && parts[1].Length <= 1);
                var validSize = child.alturaAcompanhamentoNutricional >= 20 && child.alturaAcompanhamentoNutricional <= 250;

                if (!validParts || !validSize)
                    errors.Add("A altura é inválida, informe um valor entre '20.0' e '250.0'.");
            }

            ThrowErrors(errors);
        }

        /// <summary>
        /// valida condições de saúde
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="cad"></param>
        /// <param name="domain"></param>
        /// <exception cref="ValidationException"></exception>
        public static List<string> Validar(this CondicoesDeSaude cond, CadastroIndividual cad, DomainContainer domain)
        {
            var errors = new List<string>();

            if (cond.descricaoCausaInternacaoEm12Meses != null)
            {
                if (!cond.statusTeveInternadoem12Meses)
                    errors.Add("Nas condições de saúde, a descrição da causa de internação não deve ser preenchida se não houve internação.");

                if (cond.descricaoCausaInternacaoEm12Meses.Length > 100)
                    errors.Add("Nas condições de saúde, a descrição da causa de internação aceita no máximo 100 caracteres.");
            }

            if (cond.descricaoOutraCondicao1 != null && cond.descricaoOutraCondicao1.Length > 100)
                errors.Add("Nas condições de saúde, a descrição da outra condição (1) aceita no máximo 100 caracteres.");

            if (cond.descricaoOutraCondicao2 != null && cond.descricaoOutraCondicao2.Length > 100)
                errors.Add("Nas condições de saúde, a descrição da outra condição (2) aceita no máximo 100 caracteres.");

            if (cond.descricaoOutraCondicao3 != null && cond.descricaoOutraCondicao3.Length > 100)
                errors.Add("Nas condições de saúde, a descrição da outra condição (3) aceita no máximo 100 caracteres.");

            if (cond.descricaoPlantasMedicinaisUsadas != null)
            {
                if (!cond.statusUsaPlantasMedicinais)
                    errors.Add("Nas condições de saúde, a descrição de plantas medicinais não deve ser preenchida se o cidadão não faz uso de plantas medicinais.");

                if (cond.descricaoPlantasMedicinaisUsadas.Length > 100)
                    errors.Add("Nas condições de saúde, a descrição de plantas medicinais aceita no máximo 100 caracteres.");
            }

            if (!cond.statusTeveDoencaCardiaca && cond.DoencaCardiaca.Count > 0)
                errors.Add("Nas condições de saúde, as doenças cardíacas não devem ser informadas caso o cidadão não tenha tido doença cardíaca.");
            else if (cond.statusTeveDoencaCardiaca && cond.DoencaCardiaca.Count == 0)
                errors.Add("Nas condições de saúde, as doenças cardíacas devem ser informadas caso o cidadão tenha tido doença cardíaca.");
            else if (cond.DoencaCardiaca.Count > 0 && cond.DoencaCardiaca.Any(x => x == null))
                errors.Add("Nas condições de saúde, uma ou mais doenças cardíacas não foram encontradas.");

            if (!cond.statusTemDoencaRespiratoria && cond.DoencaRespiratoria.Count > 0)
                errors.Add("Nas condições de saúde, as doenças respiratórias não devem ser informadas caso o cidadão não tenha tido doença respiratória.");
            else if (cond.statusTemDoencaRespiratoria && cond.DoencaRespiratoria.Count == 0)
                errors.Add("Nas condições de saúde, as doenças respiratórias devem ser informadas caso o cidadão tenha tido doença respiratória.");
            else if (cond.DoencaRespiratoria.Count > 0 && cond.DoencaRespiratoria.Any(x => x == null))
                errors.Add("Nas condições de saúde, uma ou mais doenças respiratórias não foram encontradas.");

            if (!cond.statusTemTeveDoencasRins && cond.DoencaRins.Count > 0)
                errors.Add("Nas condições de saúde, as doenças renais não devem ser informadas caso o cidadão não tenha tido doença renal.");
            else if (cond.statusTemTeveDoencasRins && cond.DoencaRins.Count == 0)
                errors.Add("Nas condições de saúde, as doenças renais devem ser informadas caso o cidadão tenha tido doença renal.");
            else if (cond.DoencaRins.Count > 0 && cond.DoencaRins.Any(x => x == null))
                errors.Add("Nas condições de saúde, uma ou mais doenças renais não foram encontradas.");

            if (cond.maternidadeDeReferencia != null)
            {
                if (!cond.statusEhGestante)
                    errors.Add("Nas condições de saúde, a maternidade não deve ser preenchida caso o cidadão não seja gestante.");

                if (cond.maternidadeDeReferencia.Length > 100)
                    errors.Add("Nas condições de saúde, a maternidade aceita no máximo 100 caracteres.");
            }

            if (cond.situacaoPeso != null && domain.TP_Consideracao_Peso.All(x => x.codigo != cond.situacaoPeso))
                errors.Add("Nas condições de saúde, a situação de peso não foi encontrada.");

            var nasc = cad.IdentificacaoUsuarioCidadao1?.dataNascimentoCidadao;
            var sexo = cad.IdentificacaoUsuarioCidadao1?.sexoCidadao;

            if (cond.statusEhGestante)
            {
                if (sexo != 1)
                    errors.Add(
                        "Nas condições de saúde, não é possível definir uma condição de gestante para um cidadão do sexo masculino ou sem informação sobre o sexo.");

                if (nasc == null)
                    errors.Add("Nas condições de saúde, a identificação do cidadão deve conter a data de nascimento para em caso de gestantes.");

                var atend = cad.UnicaLotacaoTransport.dataAtendimento;

                if (atend.Date.AddYears(-60) > nasc || atend.Date.AddYears(-9) < nasc)
                    errors.Add("Nas condições de saúde, não é possível definir uma condição de gestante para um cidadão com menos de 9 anos ou com mais de 60 anos.");
            }

            return errors;
        }

        /// <summary>
        /// valida situação de rua
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="domain"></param>
        /// <exception cref="ValidationException"></exception>
        public static List<string> Validar(this EmSituacaoDeRua cond, DomainContainer domain)
        {
            var errors = new List<string>();

            if (cond.grauParentescoFamiliarFrequentado != null && (!cond.statusSituacaoRua || !cond.statusVisitaFamiliarFrequentemente))
                errors.Add("Em situação de rua, o grau de parentesco do cidadão não pode ser preenchido caso o cidadão não se encontre em situação de rua ou não a tenha visita familiar frequente.");

            if (cond.HigienePessoalSituacaoRua.Count > 0 && !cond.statusSituacaoRua)
                errors.Add("Em situação de rua, a higiene pessoal não pode ser informada caso o cidadão não se encontre em situação de rua.");
            else if (cond.HigienePessoalSituacaoRua.Count == 0 && cond.statusTemAcessoHigienePessoalSituacaoRua)
                errors.Add("Em situação de rua, ao menos uma condição de higiene pessoal deve ser informada caso o cidadão possua acesso à higiene pessoal.");

            if (cond.HigienePessoalSituacaoRua.Count > 4)
                errors.Add("Em situação de rua, só é possível informar até 4 condições de higiene pessoal.");

            if (cond.HigienePessoalSituacaoRua.Any(x => domain.TP_Higiene_Pessoal.All(y => y.codigo != x.codigo_higiene_pessoal)))
                errors.Add("Em situação de rua, uma ou mais condições de higiene pessoal não foram encontradas.");

            if (cond.OrigemAlimentoSituacaoRua.Count > 0 && !cond.statusSituacaoRua)
                errors.Add("Em situação de rua, a origem de alimentos não pode ser informada caso o cidadão não se encontre em situação de rua.");

            if (cond.OrigemAlimentoSituacaoRua.Count > 5)
                errors.Add("Em situação de rua, só é possível informar até 5 origens de alimentação.");

            if (cond.OrigemAlimentoSituacaoRua.Any(x => domain.TP_Origem_Alimentacao.All(y => y.codigo != x.id_tp_origem_alimento)))
                errors.Add("Em situação de rua, uma ou mais origens de alimentação não foram encontradas.");

            if (cond.outraInstituicaoQueAcompanha != null)
            {
                if (!cond.statusSituacaoRua || !cond.statusAcompanhadoPorOutraInstituicao)
                    errors.Add("Em situação de rua, a outra instituição de acompanhamento não deve ser preenchida caso o cidadão não seja acompanhado por outra instituição.");

                if (cond.outraInstituicaoQueAcompanha.Length > 100)
                    errors.Add("Em situação de rua, a outra instituição de acompanhamento aceita no máximo 100 caracteres.");
            }

            if (cond.quantidadeAlimentacoesAoDiaSituacaoRua != null && !cond.statusSituacaoRua)
                errors.Add("Em situação de rua, a quantidade de alimentações ao dia não deve ser informada caso o cidadão não se encontre em situação de rua.");

            if (cond.quantidadeAlimentacoesAoDiaSituacaoRua != null && domain.TP_Quantas_Vezes_Alimentacao.All(x => x.codigo != cond.quantidadeAlimentacoesAoDiaSituacaoRua))
                errors.Add("Em situação de rua, a quantidade de alimentações ao dia é inválida.");

            if (cond.statusAcompanhadoPorOutraInstituicao && !cond.statusSituacaoRua)
                errors.Add("Em situação de rua, o acompanhamento por outra instituição não deve ser informado caso o cidadão não se encontre em situação de rua.");

            if (cond.statusPossuiReferenciaFamiliar && !cond.statusSituacaoRua)
                errors.Add("Em situação de rua, a referência familiar não deve ser informada caso o cidadão não se encontre em situação de rua.");

            if (cond.statusRecebeBeneficio && !cond.statusSituacaoRua)
                errors.Add("Em situação de rua, o recebimento de benefício não deve ser informado caso o cidadão não se encontre em situação de rua.");

            if (cond.statusTemAcessoHigienePessoalSituacaoRua && !cond.statusSituacaoRua)
                errors.Add("Em situação de rua, o acesso à higiene pessoal não deve ser informado caso o cidadão não se encontre em situação de rua.");

            if (cond.statusVisitaFamiliarFrequentemente && !cond.statusSituacaoRua)
                errors.Add("Em situação de rua, a visita familiar não deve ser informada caso o cidadão não se encontre em situação de rua.");

            if (cond.tempoSituacaoRua != null && !cond.statusSituacaoRua)
                errors.Add("Em situação de rua, o tempo em situação de rua não deve ser informado caso o cidadão não se encontre em situação de rua.");

            if (cond.tempoSituacaoRua != null && domain.TP_Sit_Rua.All(x => x.codigo != cond.tempoSituacaoRua))
                errors.Add("Em situação de rua, o tempo em situação de rua é inválido.");

            return errors;
        }

        /// <summary>
        /// valida os dados de um cidadão
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="cad"></param>
        /// <param name="domain"></param>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="Exception"></exception>
        public static List<string> Validar(this IdentificacaoUsuarioCidadao cond, CadastroIndividual cad, DomainContainer domain)
        {
            var header = cad.UnicaLotacaoTransport;

            var errors = new List<string>();

            if (cond.nomeSocial != null)
            {
                if (cond.nomeSocial.Length > 70)
                    errors.Add("Em dados pessoais, o Nome Social não deve ter no máximo 70 caracteres.");

                if (!cond.nomeSocial.IsValidDiatricts())
                    errors.Add("Em dados pessoais, o Nome Social é inválido, é permitido apenas letras, espaços e apóstrofo (').");
            }

            if (cond.codigoIbgeMunicipioNascimento != null)
            {
                if (cond.nacionalidadeCidadao != 1)
                    errors.Add("Em dados pessoais, o município de nascimento só deve ser informado para brasileiros.");

                if (domain.Cidade.All(x => x.CodIbge != cond.codigoIbgeMunicipioNascimento))
                    errors.Add("Em dados pessoais, o município de nascimento é inválido, não está cadastrado ou não foi encontrado o código do IBGE para o município.");
            }

            if (cond.dataNascimentoCidadao != null && !cond.dataNascimentoCidadao.Value.IsValidBirthDateTime(cad.UnicaLotacaoTransport.dataAtendimento))
                errors.Add("Em dados pessoais, a data de nascimento do cidadão é inválida, a data deve ser anterior ou igual à data de atendimento e não deve ter mais de 130 anos antes da data de atendimento.");

            try
            {
                if (cond.emailCidadao != null && ((cond.emailCidadao.Length < 6 || cond.emailCidadao.Length > 100) ||
                    (new System.Net.Mail.MailAddress(cond.emailCidadao)).Address != cond.emailCidadao))
                    throw new Exception();
            }
            catch
            {
                errors.Add("Em dados pessoais, o email do cidadão é inválido.");
            }

            if (domain.TP_Nacionalidade.All(x => x.codigo != cond.nacionalidadeCidadao))
                errors.Add("Em dados pessoais, a nacionalidade informada é inválida.");

            errors.AddRange(NomeValido(cond.nomeCidadao, "Em dados pessoais", "Nome do cidadão", true));

            if (cond.nomeMaeCidadao != null)
            {
                if (cond.desconheceNomeMae)
                    errors.Add("Em dados pessoais, o Nome da mãe do cidadão não deve ser informado se o cidadão desconhece o nome.");

                errors.AddRange(cond.nomeMaeCidadao.NomeValido("Em dados pessoais", "Nome da mãe do cidadão", false));
            }

            if (cond.nomePaiCidadao != null)
            {
                if (cond.desconheceNomePai)
                    errors.Add("Em dados pessoais, o Nome do pai do cidadão não deve ser informado se o cidadão desconhece o nome.");

                errors.AddRange(cond.nomePaiCidadao.NomeValido("Em dados pessoais", "Nome do pai do cidadão", false));
            }

            if (cond.cnsCidadao != null && !cond.cnsCidadao.isValidCns())
                errors.Add("Em dados pessoais, o CNS do cidadão está incorreto.");

            if (cond.cnsResponsavelFamiliar != null && cond.cnsResponsavelFamiliar != cond.cnsCidadao)
            {
                if (cond.statusEhResponsavel)
                    errors.Add("Em dados pessoais, o CNS do responsável familiar não deve ser informado ou deve ser igual ao CNS do cidadão quando ele for o responsável.");

                if (!cond.cnsResponsavelFamiliar.isValidCns())
                    errors.Add("Em dados pessoais, o CNS do responsável familiar é inválido.");
            }
            else if (!cond.statusEhResponsavel)
                errors.Add("Em dados pessoais, o CNS do responsável familiar deve ser informado quando o cidadão não for o responsável.");

            if (cond.telefoneCelular != null && (cond.telefoneCelular.Length < 10 || cond.telefoneCelular.Length > 11 ||
                Regex.IsMatch(cond.telefoneCelular, "([^0-9])")))
                errors.Add("Em dados pessoais, o telefone celular do cidadão é inválido.");

            if (cond.numeroNisPisPasep != null &&
                (cond.numeroNisPisPasep.Length != 11 || Regex.IsMatch(cond.numeroNisPisPasep ?? "-", "([^0-9])")))
                errors.Add("Em dados pessoais, o número do NIS/PIS/PASEP do cidadão é inválido.");

            if (cond.paisNascimento != null)
            {
                if ((cond.paisNascimento != 31 && cond.nacionalidadeCidadao == 1) || (cond.paisNascimento == 31 && cond.nacionalidadeCidadao != 1))
                    errors.Add("Em dados pessoais, o país de nascimento não corresponde à nacionalidade do cidadão.");
                else if (cond.nacionalidadeCidadao == 2)
                    errors.Add("Em dados pessoais, o país de nascimento não deve ser informado para um cidadão naturalizado.");
            }
            else if (cond.nacionalidadeCidadao == 3)
                errors.Add("Em dados pessoais, o país de nascimento do cidadão é obrigatório.");
            else if (cond.nacionalidadeCidadao == 1 && (cond.paisNascimento != null && cond.paisNascimento > 0))
                errors.Add("Em dados pessoais, o país de nascimento do cidadão é inválido para a nacionalidade informada.");

            if (domain.TP_Raca_Cor.All(x => x.id_tp_raca_cor != cond.racaCorCidadao))
                errors.Add("Em dados pessoais, a Raça/Cor do cidadão é inválida.");

            if (domain.TP_Sexo.All(x => x.codigo != cond.sexoCidadao))
                errors.Add("Em dados pessoais, o Sexo do cidadão é inválido.");

            if (cond.racaCorCidadao == 5 && (cond.etnia == null || domain.Etnia.All(x => x.CodEtnia != cond.etnia)))
                errors.Add("Em dados pessoais, a etnia do cidadão é inválida.");

            if (cond.racaCorCidadao != 5 && cond.etnia != null)
                errors.Add("Em dados pessoais, a etnia do cidadão só deve ser informada para indígenas.");

            if (cond.dtNaturalizacao == null && cond.nacionalidadeCidadao == 2)
                errors.Add("Em dados pessoais, a data de naturalização é obrigatória.");

            if (cond.dtNaturalizacao != null && cond.nacionalidadeCidadao != 2)
                errors.Add("Em dados pessoais, a data de naturalização só deve ser informada para cidadões naturalizados.");

            if (cond.dtNaturalizacao != null && (cond.dtNaturalizacao > header.dataAtendimento ||
                cond.dtNaturalizacao < cond.dataNascimentoCidadao))
                errors.Add("Em dados pessoais, a data de naturalização é inválida, informe uma data anterior à data de atendimento e posterior à data de nascimento.");

            if (cond.portariaNaturalizacao != null && cond.nacionalidadeCidadao != 2)
                errors.Add("Em dados pessoais, a portaria da naturalização não pode ser informada.");

            if (cond.portariaNaturalizacao == null && cond.nacionalidadeCidadao == 2)
                errors.Add("Em dados pessoais, a portaria da naturalização é obrigatória.");

            if (cond.portariaNaturalizacao != null && cond.portariaNaturalizacao.Length > 16)
                errors.Add("Em dados pessoais, a portaria da naturalização aceita no máximo 16 caracteres.");

            if (cond.dtEntradaBrasil != null && cond.nacionalidadeCidadao != 3)
                errors.Add("Em dados pessoais, a data de entrada no Brasil não deve ser preenchida.");

            if (cond.dtEntradaBrasil == null && cond.nacionalidadeCidadao == 3)
                errors.Add("Em dados pessoais, a data de entrada no Brasil é obrigatória.");

            if (cond.dtEntradaBrasil != null && (cond.dtEntradaBrasil > header.dataAtendimento ||
                cond.dtEntradaBrasil < cond.dataNascimentoCidadao))
                errors.Add("Em dados pessoais, a data de entrada no brasil é inválida.");

            if (cond.microarea != null && cond.stForaArea)
                errors.Add("Em dados pessoais, a microárea não deve ser informada quando o cadastro for fora de área.");

            if (cond.microarea == null && !cond.stForaArea)
                errors.Add("Em dados pessoais, a microárea é obrigatória.");

            if (cond.microarea != null && (cond.microarea.Length != 2 || !Regex.IsMatch(cond.microarea, "^([0-9][0-9])$")))
                errors.Add("Em dados pessoais, o código da microárea é inválido. Informe 2 caracteres numéricos.");

            return errors;
        }

        /// <summary>
        /// valida informações sociodemograficas
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="cad"></param>
        /// <param name="domain"></param>
        /// <exception cref="ValidationException"></exception>
        public static List<string> Validar(this InformacoesSocioDemograficas cond, CadastroIndividual cad, DomainContainer domain)
        {
            var errors = new List<string>();

            if (cond.DeficienciasCidadao.Count == 0 && cond.statusTemAlgumaDeficiencia)
                errors.Add("Em informações sociodemográficas, ao menos uma deficiência deve ser informada.");

            if (cond.DeficienciasCidadao.Count > 0 && !cond.statusTemAlgumaDeficiencia)
                errors.Add("Em informações sociodemográficas, nenhuma deficiência pode ser informada se o cidadão não for deficiente.");

            if (cond.DeficienciasCidadao.Count > 5)
                errors.Add("Em informações sociodemográficas, podem ser informadas até 5 deficiências.");

            if (cond.DeficienciasCidadao.Any(x => domain.TP_Deficiencia.All(y => y.codigo != x.id_tp_deficiencia_cidadao)))
                errors.Add("Em informações sociodemográficas, uma ou mais deficiências estão incorretas.");

            if (cond.grauInstrucaoCidadao != null && domain.TP_Curso.All(x => x.codigo != cond.grauInstrucaoCidadao))
                errors.Add("Em informações sociodemográficas, o grau de instrução do cidadão é inválido.");

            if (cond.ocupacaoCodigoCbo2002 != null && domain.AS_ProfissoesTab.ToList().All(x => x.CodProfTab == null || x.CodProfTab.Trim() != cond.ocupacaoCodigoCbo2002.Trim()))
                errors.Add("Em informações sociodemográficas, a profissão do cidadão é inválida.");

            if (cond.orientacaoSexualCidadao != null && !cond.statusDesejaInformarOrientacaoSexual)
                errors.Add("Em informações sociodemográficas, a orientação sexual do cidadão não deve ser informada se ele não desejar.");

            if (cond.orientacaoSexualCidadao != null && domain.TP_Orientacao_Sexual.All(x => x.codigo != cond.orientacaoSexualCidadao))
                errors.Add("Em informações sociodemográficas, a orientação sexual do cidadão é inválida.");

            if (cond.povoComunidadeTradicional != null && !cond.statusMembroPovoComunidadeTradicional)
                errors.Add("Em informações sociodemográficas, o povo de comunidade tradicional não deve ser informado se o cidadão não participa de um.");

            if (cond.povoComunidadeTradicional != null && cond.povoComunidadeTradicional.Length > 100)
                errors.Add("Em informações sociodemográficas, o povo de comunidade tradicioinal aceita até 100 caracteres.");

            if (cond.relacaoParentescoCidadao != null && cad.IdentificacaoUsuarioCidadao1.statusEhResponsavel)
                errors.Add("Em informações sociodemográficas, a relação de parentesco não pode ser preenchida se o cidadão for o responsável familiar.");

            if (cond.relacaoParentescoCidadao != null && domain.TP_Relacao_Parentesco.All(x => x.codigo != cond.relacaoParentescoCidadao)
                    && !cad.IdentificacaoUsuarioCidadao1.statusEhResponsavel)
                errors.Add("Em informações sociodemográficas, a relação de parentesco é inválida.");

            if (cond.situacaoMercadoTrabalhoCidadao != null && domain.TP_Sit_Mercado.All(x => x.codigo != cond.situacaoMercadoTrabalhoCidadao))
                errors.Add("Em informações sociodemográficas, a situação no mercado de trabalho é inválida.");

            if (cond.identidadeGeneroCidadao != null && !cond.statusDesejaInformarIdentidadeGenero)
                errors.Add("Em informações sociodemográficas, a identidade de gênero não deve ser informada se o cidadão não desejar.");

            var resp = cad.UnicaLotacaoTransport.dataAtendimento.Date.AddYears(-9);

            if (cond.ResponsavelPorCrianca.Count > 0 && cad.IdentificacaoUsuarioCidadao1.dataNascimentoCidadao < resp)
                errors.Add("Em informações sociodemográficas, não se deve preencher a responsabilidade por crianças para este cidadão, o cidadão deve ter até 9 anos de idade à contar da data de atendimento.");

            if (cond.ResponsavelPorCrianca.Count > 6)
                errors.Add("Em informações sociodemográficas, o limite de responsabilidade por criança é 6.");

            return errors;
        }

        /// <summary>
        /// valida a saida do cadastro
        /// </summary>
        /// <param name="cond"></param>
        /// <exception cref="ValidationException"></exception>
        public static List<string> Validar(this SaidaCidadaoCadastro cond)
        {
            var errors = new List<string>();

            if (cond.dataObito != null && cond.motivoSaidaCidadao != 135)
                errors.Add("Em informações sociodemográficas, a data do óbito não deve ser informada.");

            if (cond.dataObito == null && cond.motivoSaidaCidadao == 135)
                errors.Add("Em informações sociodemográficas, a data do óbito deve ser informada.");

            if (cond.numeroDO != null && cond.motivoSaidaCidadao != 135)
                errors.Add("Em informações sociodemográficas, o número do documento de óbito não deve ser informado.");

            return errors;
        }

        /// <summary>
        /// valida um cadastro individual
        /// </summary>
        /// <param name="cad"></param>
        /// <param name="domain"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this CadastroIndividual cad, DomainContainer domain)
        {
            var errors = new List<string>();
            var empty = new List<string>();

            if (cad.UnicaLotacaoTransport == null)
                errors.Add("O Cabeçalho de atendimento não foi informado.");

            if (cad.statusTermoRecusaCadastroIndividualAtencaoBasica && cad.CondicoesDeSaude1 != null)
                errors.Add("Não é possível informar as condições de saúde para os casos de recusa.");

            errors.AddRange(cad.CondicoesDeSaude1?.Validar(cad, domain) ?? empty);

            if (cad.statusTermoRecusaCadastroIndividualAtencaoBasica && cad.EmSituacaoDeRua1 != null)
                errors.Add("Não é possível informar as situações de rua para os casos de recusa.");

            errors.AddRange(cad.EmSituacaoDeRua1?.Validar(domain) ?? empty);

            if (!cad.statusTermoRecusaCadastroIndividualAtencaoBasica && cad.IdentificacaoUsuarioCidadao1 == null)
                errors.Add("A identificação do cidadão (dados pessoais) é obrigatória.");

            errors.AddRange(cad.IdentificacaoUsuarioCidadao1?.Validar(cad, domain) ?? empty);

            if (!cad.statusTermoRecusaCadastroIndividualAtencaoBasica && cad.InformacoesSocioDemograficas1 == null)
                errors.Add("As informações sociodemográficas do cidadão são obrigatórias.");

            errors.AddRange(cad.InformacoesSocioDemograficas1?.Validar(cad, domain) ?? empty);

            if (!cad.UnicaLotacaoTransport.OrigemVisita.enviarParaThrift)
            {
                cad.uuidFichaOriginadora = cad.id;
                cad.fichaAtualizada = false;
            }

            if (cad.uuidFichaOriginadora == null && cad.fichaAtualizada)
                errors.Add("Informe o Uuid da ficha originadora.");

            errors.AddRange(cad.SaidaCidadaoCadastro1?.Validar() ?? empty);

            ThrowErrors(errors);
        }

        /// <summary>
        /// valida a condição de moradia
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="cad"></param>
        /// <param name="domain"></param>
        /// <exception cref="ValidationException"></exception>
        public static List<string> Validar(this CondicaoMoradia cond, CadastroDomiciliar cad, DomainContainer domain)
        {
            var errors = new List<string>();

            var proibido = (new long[] { 7, 8, 9, 10, 11 }).Contains(cad.tipoDeImovel);

            if (cond.abastecimentoAgua != null && domain.TP_Abastecimento_Agua.All(x => x.codigo != cond.abastecimentoAgua))
                errors.Add("Em condições de moradia, o abastecimento de água é inválido.");

            if (cond.areaProducaoRural != null && (proibido || cond.localizacao == 83))
                errors.Add("Em condições de moradia, a área de produção rural não deve ser definida para o tipo imóvel selecionado.");

            if (cond.destinoLixo != null && domain.TP_Destino_Lixo.All(x => x.codigo != cond.destinoLixo))
                errors.Add("Em condições de moradia, o destino do lixo é inválido.");

            if (cond.formaEscoamentoBanheiro != null && domain.TP_Escoamento_Esgoto.All(x => x.codigo != cond.formaEscoamentoBanheiro))
                errors.Add("Em condições de moradia, a forma de escoamento do banheiro ou sanitário é inválida.");

            if (cond.localizacao != null && domain.TP_Localizacao.All(x => x.codigo != cond.localizacao))
                errors.Add("Em condições de moradia, a localização da moradia é inválida.");

            if (cond.materialPredominanteParedesExtDomicilio != null && proibido)
                errors.Add("Em condições de moradia, o material predominante das paredes externas não pode ser informado para o tipo de imóvel selecionado.");

            if (cond.materialPredominanteParedesExtDomicilio != null && domain.TP_Construcao_Domicilio.All(x => x.codigo != cond.materialPredominanteParedesExtDomicilio))
                errors.Add("Em condições de moradia, o material predominante das paredes externas é inválido.");

            if (cond.nuComodos != null && proibido)
                errors.Add("Em condições de moradia, a quantidade de cômodos não pode ser informada para o tipo de imóvel selecionado.");

            if (cond.nuComodos == 0 || cond.nuComodos > 99)
                errors.Add("Em condições de moradia, a quantidade de cômodos é inválida, informe de 0 a 99 cômodos");

            if (cond.nuMoradores != null && (cond.nuMoradores < cad.FamiliaRow.Sum(x => x.numeroMembrosFamilia) || cond.nuMoradores < cad.FamiliaRow.Count))
                errors.Add("Em condições de moradia, o número de moradores não pode ser menor que o total de membros das famílias cadastradas.");

            if (cond.situacaoMoradiaPosseTerra != null)
            {
                if (proibido)
                    errors.Add("Em condições de moradia, a situação de moradia não pode ser informada para o tipo de imóvel selecionado.");

                if (domain.TP_Situacao_Moradia.All(x => x.codigo != cond.situacaoMoradiaPosseTerra))
                    errors.Add("Em condições de moradia, a situação de moradia é inválida.");
            }

            if (cond.tipoAcessoDomicilio != null)
            {
                if (proibido)
                    errors.Add("Em condições de moradia, o tipo de acesso ao domicílio não pode ser informado para o tipo de imóvel selecionado.");

                if (domain.TP_Acesso_Domicilio.All(x => x.codigo != cond.tipoAcessoDomicilio))
                    errors.Add("Em condições de moradia, o tipo de acesso ao domicílio é inválido.");
            }

            if (cond.tipoDomicilio != null)
            {
                if (proibido)
                    errors.Add("Em condições de moradia, o tipo de domicílio não pode ser informado para o tipo de imóvel selecionado.");

                if (domain.TP_Domicilio.All(x => x.codigo != cond.tipoDomicilio))
                    errors.Add("Em condições de moradia, o tipo de domicílio é inválido.");
            }

            if (cond.aguaConsumoDomicilio != null && domain.TP_Tratamento_Agua.All(x => x.codigo != cond.aguaConsumoDomicilio))
                errors.Add("Em condições de moradia, o tipo de água de consumo no domicílio é inválido.");

            return errors;
        }

        /// <summary>
        /// valida uma familia
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="pos"></param>
        /// <param name="cad"></param>
        /// <exception cref="ValidationException"></exception>
        public static List<string> Validar(this FamiliaRow cond, int pos, CadastroDomiciliar cad)
        {
            var errors = new List<string>();

            if (cond.dataNascimentoResponsavel != null && !cond.dataNascimentoResponsavel.Value.IsValidBirthDateTime(cad.UnicaLotacaoTransport.dataAtendimento))
                errors.Add($"Nas famílias, a data de nascimento do responsável está incorreta para a {pos}&ordf; família.");

            if (cond.numeroCnsResponsavel == null || !cond.numeroCnsResponsavel.isValidCns())
                errors.Add($"Nas famílias, o CNS do responsável está incorreto para a {pos}&ordf; família.");

            if (cond.numeroMembrosFamilia != null && (cond.numeroMembrosFamilia < 1 || cond.numeroMembrosFamilia > 99))
                errors.Add($"Nas famílias, o número de membros está incorreto para a {pos}&ordf; família, informe no mínimo d1 e no máximo 99 membros.");

            return errors;
        }

        /// <summary>
        /// valida um endereço
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="cad"></param>
        /// <param name="domain"></param>
        /// <exception cref="ValidationException"></exception>
        public static List<string> Validar(this EnderecoLocalPermanencia cond, CadastroDomiciliar cad, DomainContainer domain)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(cond.bairro) || string.IsNullOrWhiteSpace(cond.bairro) || cond.bairro.Length < 1 || cond.bairro.Length > 72)
                errors.Add("Em local de permanência, o bairro deve ter entre 1 e 72 caracteres.");

            if (string.IsNullOrEmpty(cond.cep) || string.IsNullOrWhiteSpace(cond.cep) || !Regex.IsMatch(cond.cep, "^([0-9]{8})$"))
                errors.Add("Em local de permanência, o cep deve ter 8 caracteres numéricos.");

            if (cond.codigoIbgeMunicipio == null || domain.Cidade.All(x => x.CodIbge != cond.codigoIbgeMunicipio))
                errors.Add("Em local de permanência, o município do local de permanência é inválido ou não está cadastrado ou o código do IBGE não foi encontrado.");

            if (cond.complemento != null && cond.complemento.Length > 30)
                errors.Add("Em local de permanência, o complemento deve ter até 30 caracteres.");

            if (cond.nomeLogradouro == null || cond.nomeLogradouro.Length < 0 || cond.nomeLogradouro.Length > 72)
                errors.Add("Em local de permanência, o Logradouro deve ter entre 1 e 72 caracteres.");

            if (cond.numero != null && cond.stSemNumero)
                errors.Add("Em local de permanência, o número não pode ser definido para um endereço sem número.");

            if (cond.numero == null && !cond.stSemNumero)
                errors.Add("Em local de permanência, informe o número do endereço.");

            if (cond.numero != null && (Regex.IsMatch(cond.numero, "([^0-9])") || cond.numero.Length < 1 || cond.numero.Length > 10))
                errors.Add("Em local de permanência, o número do endereço deve ter entre 1 e 10 caracteres numéricos.");

            if (cond.numeroDneUf == null || domain.UF.OrderBy(x => x.DesUF).Select(RowNumberPad('0', 2)).All(x => x != cond.numeroDneUf))
                errors.Add("Em local de permanência, o estado (UF) é inválido.");

            if (cond.telefoneContato != null && (cond.telefoneContato.Length < 10 || cond.telefoneContato.Length > 11))
                errors.Add("Em local de permanência, o telefone de contato é inválido, o número deve ter entre 10 e 11 caracteres numéricos incluíndo o DDD.");

            if (cond.telefoneResidencia != null && (cond.telefoneResidencia.Length < 10 || cond.telefoneResidencia.Length > 11))
                errors.Add("Em local de permanência, o telefone da residência é inválido, o número deve ter entre 10 e 11 caracteres numéricos incluíndo o DDD.");

            if (cond.tipoLogradouroNumeroDne == null || domain.TB_MS_TIPO_LOGRADOURO.All(x => x.CO_TIPO_LOGRADOURO == null || x.CO_TIPO_LOGRADOURO.Trim() != cond.tipoLogradouroNumeroDne))
                errors.Add("Em local de permanência, o tipo de logradouro é inválido.");

            if (cond.pontoReferencia != null && cond.pontoReferencia.Length > 40)
                errors.Add("Em local de permanência, o ponto de referência deve ter até 40 caracteres.");

            if (cond.microarea != null && cond.stForaArea)
                errors.Add("Em local de permanência, a microárea não deve ser informada para um cadastro fora de área.");

            if (cond.microarea == null && !cond.stForaArea)
                errors.Add("Em local de permanência, a microárea deve ser informada.");

            if (cond.microarea != null && !Regex.IsMatch(cond.microarea, "^([0-9][0-9])$"))
                errors.Add("Em local de permanência, a microárea é inválida. Informe 2 digitos numéricos.");

            return errors;
        }

        /// <summary>
        /// PadLeft
        /// </summary>
        /// <param name="c"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        private static Func<UF, string> RowNumberPad(char c, int l)
        {
            var row = 1;

            return (i) => (row++).ToString().PadLeft(l, c);
        }

        /// <summary>
        /// valida um cadastro domiciliar
        /// </summary>
        /// <param name="cad"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public static async Task Validar(this CadastroDomiciliar cad, DomainContainer domain)
        {
            var errors = new List<string>();
            var empty = new List<string>();

            if (await domain.TP_Imovel.AllAsync(x => x.codigo != cad.tipoDeImovel))
                errors.Add("O tipo de imóvel selecionado é inválido.");

            var proibido = (new long[] { 2, 3, 4, 5, 6, 12 }).Contains(cad.tipoDeImovel);

            cad.id = Guid.NewGuid();
            if (cad.AnimalNoDomicilio.Count > 0)
            {
                if (cad.tipoDeImovel != 1)
                    errors.Add("Os animais no domicílio apenas podem ser atribuídos para imóvel do tipo Domicílio.");

                if (cad.statusTermoRecusa)
                    errors.Add("Os animais no domicílio não podem ser atribuídos à cadastros com termo de recusa.");

                if (cad.AnimalNoDomicilio.Count > 4)
                    errors.Add("São permitidos até quatro tipos de animais por domicílio.");
            }

            if (cad.CondicaoMoradia1 != null && (proibido || cad.statusTermoRecusa))
                errors.Add("A condição de moradia não pode ser definida para o tipo de imóvel selecionado.");

            errors.AddRange(cad.CondicaoMoradia1?.Validar(cad, domain) ?? empty);

            if (cad.EnderecoLocalPermanencia1 != null && cad.statusTermoRecusa)
                errors.Add("O endereço do local de permanência não deve ser informado para cadastros com termo de recusa.");

            errors.AddRange(cad.EnderecoLocalPermanencia1?.Validar(cad, domain) ?? empty);

            if (cad.FamiliaRow.Count > 0 && cad.statusTermoRecusa)
                errors.Add("Não devem ser informas as famalias para cadastros com termo de recusa.");

            if (cad.FamiliaRow.Count > 0 && cad.tipoDeImovel != 1 && cad.FamiliaRow.Any(x => !x.stMudanca))
                errors.Add("Todas as famílias devem ter o status de mudança para imóvel que não são domicílio.");

            var i = 0;
            errors.AddRange(cad.FamiliaRow.SelectMany(x => x?.Validar(++i, cad) ?? empty));

            if (cad.quantosAnimaisNoDomicilio != null)
            {
                if (cad.tipoDeImovel != 1 || !cad.stAnimaisNoDomicilio || cad.statusTermoRecusa)
                    errors.Add("Não pode ser informada a quantidade de animais para cadastros que não são domicílios.");
                else
                {
                    if (cad.quantosAnimaisNoDomicilio <= 0)
                        errors.Add("Informe o número de animais no domicílio.");

                    if (cad.quantosAnimaisNoDomicilio < cad.AnimalNoDomicilio.Count || cad.quantosAnimaisNoDomicilio > 99)
                        errors.Add($"O número de animais no domicílio deve ser maior ou igual à {cad.AnimalNoDomicilio.Count} e menor que 99.");
                }
            }

            if (cad.stAnimaisNoDomicilio && (cad.tipoDeImovel != 1 || cad.statusTermoRecusa))
                errors.Add("Não podem ser informados animais em domicílio para cadastros que não são domicílios ou com termo de recusa.");

            if (!cad.UnicaLotacaoTransport.OrigemVisita.enviarParaThrift)
            {
                cad.uuidFichaOriginadora = cad.id;
                cad.fichaAtualizada = false;
            }

            if (cad.uuidFichaOriginadora == null && cad.fichaAtualizada)
                errors.Add("Informe o Uuid da ficha originadora.");

            if (cad.InstituicaoPermanencia1 != null && !((new long[] { 7, 8, 9, 10, 11 }).Contains(cad.tipoDeImovel) || cad.statusTermoRecusa))
                errors.Add("A instituição de permanência não pode ser informada para o tipo de imóvel selecionado.");
            
            errors.AddRange(cad.InstituicaoPermanencia1?.Validar(cad)??empty);
        }

        /// <summary>
        /// valida uma instituição
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="cad"></param>
        /// <exception cref="ValidationException"></exception>
        public static List<string> Validar(this InstituicaoPermanencia cond, CadastroDomiciliar cad)
        {
            var errors = new List<string>();

            if (cond.nomeInstituicaoPermanencia != null && cond.nomeInstituicaoPermanencia.Length > 100)
                errors.Add("Em instituição de permanência, o nome da instituição aceita até 100 caracteres.");

            errors.AddRange(NomeValido(cond.nomeResponsavelTecnico, "Em instituição de permanência", "Nome do responsável técnico", true));

            if (cond.cnsResponsavelTecnico != null && !cond.cnsResponsavelTecnico.isValidCns())
                errors.Add("Em instituição de permanência, o CNS do responsável técnico é inválido.");

            if (cond.cargoInstituicao != null && cond.cargoInstituicao.Length > 100)
                errors.Add("Em instituição de permanência, o cargo do responsável técnico é inválido.");

            if (cond.telefoneResponsavelTecnico != null && (Regex.IsMatch(cond.telefoneResponsavelTecnico, "([^0-9])") || cond.telefoneResponsavelTecnico.Length < 10 || cond.telefoneResponsavelTecnico.Length > 11))
                errors.Add("Em instituição de permanência, o telefone do responsável técnico é inválido.");

            return errors;
        }
    }
}
