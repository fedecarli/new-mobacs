using Softpark.Infrastructure.Extensions;
using Softpark.Infrastructure.Extras;
using System;
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
    /// 
    /// </summary>
    public static class RegrasCadastro
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string text)
        {
            return string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                              UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string[] SplitWords(this string text)
        {
            return text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public static bool NomeValido(this string nome)
        {
            if (nome == null) return false;

            nome = nome.Trim().RemoveDiacritics().ToUpperInvariant();
            var words = nome.SplitWords();
            return !(nome.Length < 3 || nome.Length > 70 || words.Length == 1
            || words.Any(x => x.Length == 1 && (x != "E" && x != "Y"))
            || Regex.IsMatch(nome, "([^a-zA-Z '])") || (words.Length == 2 && words.All(x => x.Length == 2)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this UnicaLotacaoTransport header, DomainContainer domain)
        {
            if (!header.profissionalCNS.isValidCns())
                throw new ValidationException("CNS inválido.");

            var profissional = domain.VW_Profissional.Where(x => x.CNS != null && x.CNS.Trim() == header.profissionalCNS.Trim()).ToArray();

            if (profissional.Length == 0)
                throw new ValidationException("CNS não encontrado.");

            if (profissional.All(x => x.CBO == null || x.CBO.Trim() != header.cboCodigo_2002.Trim()))
                throw new ValidationException("CBO não encontrado.");

            if (profissional.All(x => x.CNES == null || x.CNES.Trim() != header.cnes.Trim()))
                throw new ValidationException("CNES não encontrado.");

            if (header.ine != null && profissional.All(x => x.INE == null || x.INE.Trim() != header.ine.Trim()))
                throw new ValidationException("INE não encontrado.");

            var validEpoch = Epoch.ValidateESUSDateTime(header.dataAtendimento);

            if (validEpoch != ValidationResult.Success)
                throw new ValidationException("Data do Atendimento inválida.");

            if (domain.Cidade.All(x => x.CodIbge == null || x.CodIbge.Trim() != header.codigoIbgeMunicipio.Trim()))
                throw new ValidationException("Município não encontrado.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="master"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this FichaVisitaDomiciliarMaster master)
        {
            if (master.UnicaLotacaoTransport == null)
                throw new ValidationException("Cabeçalho não encontrado.");

            if (master.uuidFicha == null || master.uuidFicha.Trim().Length != 44)
                throw new ValidationException("Informe o Uuid da ficha.");

            if (master.uuidFicha != null && master.uuidFicha.Trim().Length == 44 && master.uuidFicha.Substring(0, 7) != master
                .UnicaLotacaoTransport.cnes)
                throw new ValidationException("Uuid inválido. O CNES informado não corresponde ao cabeçalho.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this FichaVisitaDomiciliarChild child)
        {
            var master = child.FichaVisitaDomiciliarMaster;

            if (master == null)
                throw new ValidationException("Ficha master não encontrada.");

            if (child.turno < 1 || child.turno > 3)
                throw new ValidationException("Turno inválido.");

            var proibido = (new long[] { 2, 3, 4, 5, 6, 12 }).Contains(child.tipoDeImovel);

            var obrigatorio = (new long[] { 1, 7, 8, 9, 10, 11, 99 }).Contains(child.tipoDeImovel) &&
                (child.SIGSM_MotivoVisita.Any(x => x.observacoes == "#BUSCA_ATIVA" || x.observacoes == "#ACOMPANHAMENTO"
                || x.codigo == 25 || x.codigo == 31) || (child.pesoAcompanhamentoNutricional ?? 0 + child.alturaAcompanhamentoNutricional ?? 0) > 0);

            if (proibido && child.numProntuario != null)
                throw new ValidationException("O número do prontuário não deve ser fornecido para o tipo de imóvel selecionado.");

            if (child.numProntuario != null && !Regex.IsMatch(child.numProntuario, "^([a-zA-Z0-9]+)$"))
                throw new ValidationException("Número do Prontuário inválido.");

            if (child.cnsCidadao != null && proibido)
                throw new ValidationException("O CNS não deve ser fornecido para o tipo de imóvel selecionado.");

            if (child.cnsCidadao != null && !child.cnsCidadao.isValidCns())
                throw new ValidationException("CNS inválido");

            if (child.dtNascimento != null && proibido)
                throw new ValidationException("A data de nascimento não deve ser fornecido para o tipo de imóvel selecionado.");

            if (child.dtNascimento != null && !child.dtNascimento.Value.IsValidBirthDateTime(child.FichaVisitaDomiciliarMaster.UnicaLotacaoTransport.dataAtendimento))
                throw new ValidationException("Data de nascimento inválida.");

            if (child.dtNascimento == null && obrigatorio)
                throw new ValidationException("A data de nascimento é obrigatória.");

            if (child.sexo != null && proibido)
                throw new ValidationException("O sexo não deve ser fornecido para o tipo de imóvel selecionado.");

            if (child.sexo == null && obrigatorio)
                throw new ValidationException("O sexo é obrigatório.");

            if (child.sexo != null && !(new long?[] { 0, 1, 4 }).Contains(child.sexo))
                throw new ValidationException("O sexo informado é inválido.");

            if (child.SIGSM_MotivoVisita.Count > 0)
            {
                if (child.desfecho == 2 || child.desfecho == 3)
                    throw new ValidationException("Os motivos de visita não devem ser informados para o desfecho selecionado.");

                var motivos = new long[] { 1, 34, 35, 36, 37, 27, 31, 28 };

                if (proibido && child.SIGSM_MotivoVisita.Any(x => !motivos.Contains(x.codigo)))
                    throw new ValidationException("Um ou mais motivos de visitas não podem ser informados para o tipo de imóvel selecionado.");

                if (child.SIGSM_MotivoVisita.Count > 36)
                    throw new ValidationException("Somente 36 motivos de visita são permitidos.");
            }

            if (child.desfecho < 1 || child.desfecho > 3)
                throw new ValidationException("O desfecho informado é inválido.");

            if (child.stForaArea && child.microarea != null)
                throw new ValidationException("A microárea não pode ser informada quando a visita for fora de área.");

            if (child.microarea != null && (child.microarea.Trim().Length != 2 || !Regex.IsMatch(child.microarea.Trim(), "^([0-9][0-9])$")))
                throw new ValidationException("A microárea deve ser informada com 2 caracteres numéricos.");

            if (child.tipoDeImovel < 1 || (child.tipoDeImovel > 12 && child.tipoDeImovel != 99))
                throw new ValidationException("O tipo de imóvel informado é inválido.");

            if (child.pesoAcompanhamentoNutricional != null)
            {
                if (proibido)
                    throw new ValidationException("O peso não deve ser informado para o tipo de imóvel selecionado.");

                if (child.desfecho == 3 || child.desfecho == 2)
                    throw new ValidationException("O peso não deve ser informado para o desfecho selecionado.");

                var parts = child.pesoAcompanhamentoNutricional.ToString().Replace(',', '.').Split('.');

                var validParts = parts.Length < 2 || (parts.Length == 2 && parts[1].Length <= 3);
                var validSize = child.pesoAcompanhamentoNutricional >= 0.5m && child.pesoAcompanhamentoNutricional <= 500;

                if (!validParts || !validSize)
                    throw new ValidationException("O peso é inválido.");
            }

            if (child.alturaAcompanhamentoNutricional == null) return;

            {
                if (proibido)
                    throw new ValidationException("A altura não deve ser informada para o tipo de imóvel selecionado.");

                if (child.desfecho == 3 || child.desfecho == 2)
                    throw new ValidationException("A altura não deve ser informada para o desfecho selecionado.");

                var parts = child.alturaAcompanhamentoNutricional.ToString().Replace(',', '.').Split('.');

                var validParts = parts.Length < 2 || (parts.Length == 2 && parts[1].Length <= 1);
                var validSize = child.alturaAcompanhamentoNutricional >= 20 && child.alturaAcompanhamentoNutricional <= 250;

                if (!validParts || !validSize)
                    throw new ValidationException("A altura é inválida.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="cad"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this CondicoesDeSaude cond, CadastroIndividual cad, DomainContainer domain)
        {
            if (cond.descricaoCausaInternacaoEm12Meses != null)
            {
                if (!cond.statusTeveInternadoem12Meses)
                    throw new ValidationException("A descrição da causa de internação não deve ser preenchida.");

                if (cond.descricaoCausaInternacaoEm12Meses.Length > 100)
                    throw new ValidationException("A descrição da causa de internação aceita no máximo 100 caracteres.");
            }

            if (cond.descricaoOutraCondicao1 != null && cond.descricaoOutraCondicao1.Length > 100)
                throw new ValidationException("A descrição da outra condição (1) aceita no máximo 100 caracteres.");

            if (cond.descricaoOutraCondicao2 != null && cond.descricaoOutraCondicao2.Length > 100)
                throw new ValidationException("A descrição da outra condição (2) aceita no máximo 100 caracteres.");

            if (cond.descricaoOutraCondicao3 != null && cond.descricaoOutraCondicao3.Length > 100)
                throw new ValidationException("A descrição da outra condição (3) aceita no máximo 100 caracteres.");

            if (cond.descricaoPlantasMedicinaisUsadas != null)
            {
                if (!cond.statusUsaPlantasMedicinais)
                    throw new ValidationException("A descrição de plantas medicinais não deve ser preenchida.");

                if (cond.descricaoPlantasMedicinaisUsadas.Length > 100)
                    throw new ValidationException("A descrição de plantas medicinais aceita no máximo 100 caracteres.");
            }

            if (!cond.statusTeveDoencaCardiaca && cond.DoencaCardiaca.Count > 0)
                throw new ValidationException("As doenças cardíacas não devem ser informadas.");
            else if (cond.statusTeveDoencaCardiaca && cond.DoencaCardiaca.Count == 0)
                throw new ValidationException("As doenças cardíacas devem ser informadas.");
            else if (cond.DoencaCardiaca.Count > 0 && cond.DoencaCardiaca.Any(x => x == null))
                throw new ValidationException("Uma ou mais doenças cardíacas não foram encontradas.");

            if (!cond.statusTemDoencaRespiratoria && cond.DoencaRespiratoria.Count > 0)
                throw new ValidationException("As doenças respiratórias não devem ser informadas.");
            else if (cond.statusTemDoencaRespiratoria && cond.DoencaRespiratoria.Count == 0)
                throw new ValidationException("As doenças respiratórias devem ser informadas.");
            else if (cond.DoencaRespiratoria.Count > 0 && cond.DoencaRespiratoria.Any(x => x == null))
                throw new ValidationException("Uma ou mais doenças respiratórias não foram encontradas.");

            if (!cond.statusTemTeveDoencasRins && cond.DoencaRins.Count > 0)
                throw new ValidationException("As doenças renais não devem ser informadas.");
            else if (cond.statusTemTeveDoencasRins && cond.DoencaRins.Count == 0)
                throw new ValidationException("As doenças renais devem ser informadas.");
            else if (cond.DoencaRins.Count > 0 && cond.DoencaRins.Any(x => x == null))
                throw new ValidationException("Uma ou mais doenças renais não foram encontradas.");

            if (cond.maternidadeDeReferencia != null)
            {
                if (!cond.statusEhGestante)
                    throw new ValidationException("A maternidade não deve ser preenchida.");

                if (cond.maternidadeDeReferencia.Length > 100)
                    throw new ValidationException("A maternidade aceita no máximo 100 caracteres.");
            }

            if (cond.situacaoPeso != null && !domain.TP_Consideracao_Peso.Any(x => x.codigo == cond.situacaoPeso))
                throw new ValidationException("Situação de Peso não encontrada.");

            var nasc = cad.IdentificacaoUsuarioCidadao1?.dataNascimentoCidadao;
            var sexo = cad.IdentificacaoUsuarioCidadao1?.sexoCidadao;

            if (!cond.statusEhGestante) return;

            if (sexo == 0)
            {
                throw new ValidationException(
                    "Não é possível definir uma condição de gestante para um cidadão do sexo masculino.");
            }

            if (nasc == null)
                throw new ValidationException("A identificação do cidadão deve conter a data de nascimento.");

            var atend = cad.UnicaLotacaoTransport.dataAtendimento;

            if (atend.Date.AddYears(-60) > nasc || atend.Date.AddYears(-9) < nasc)
                throw new ValidationException("Não é possível definir uma condição de gestante para um cidadão com menos de 9 anos ou com mais de 60 anos.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cond"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this EmSituacaoDeRua cond, DomainContainer domain)
        {
            if (cond.grauParentescoFamiliarFrequentado != null && (!cond.statusSituacaoRua || !cond.statusVisitaFamiliarFrequentemente))
                throw new ValidationException("O grau de parentesco não pode ser preenchido para este caso.");

            if (cond.HigienePessoalSituacaoRua.Count > 0 && !cond.statusSituacaoRua)
                throw new ValidationException("A higiene pessoal não pode ser informada para este caso.");
            else if (cond.HigienePessoalSituacaoRua.Count == 0 && cond.statusTemAcessoHigienePessoalSituacaoRua)
                throw new ValidationException("Ao menos uma condição de higiene pessoal deve ser informada.");

            if (cond.HigienePessoalSituacaoRua.Count > 4)
                throw new ValidationException("Somente 4 condições de higiene pessoal podem ser informadas por cadastro.");

            if (cond.HigienePessoalSituacaoRua.Any(x => domain.TP_Higiene_Pessoal.All(y => y.codigo != x.codigo_higiene_pessoal)))
                throw new ValidationException("Uma ou mais condições de higiene pessoal não foram encontradas.");

            if (cond.OrigemAlimentoSituacaoRua.Count > 0 && !cond.statusSituacaoRua)
                throw new ValidationException("A origem de alimentos não pode ser informada para este caso.");

            if (cond.OrigemAlimentoSituacaoRua.Count > 5)
                throw new ValidationException("Somente 5 origens de alimentos podem ser informadas por cadastro.");

            if (cond.OrigemAlimentoSituacaoRua.Any(x => domain.TP_Origem_Alimentacao.All(y => y.codigo != x.id_tp_origem_alimento)))
                throw new ValidationException("Uma ou mais condições de higiene pessoal não foram encontradas.");

            if (cond.outraInstituicaoQueAcompanha != null)
            {
                if (!cond.statusSituacaoRua || !cond.statusAcompanhadoPorOutraInstituicao)
                    throw new ValidationException("A outra instituição de acompanhamento não deve ser preenchida.");

                if (cond.outraInstituicaoQueAcompanha.Length > 100)
                    throw new ValidationException("A outra instituição de acompanhamento aceita no máximo 100 caracteres.");
            }

            if (cond.quantidadeAlimentacoesAoDiaSituacaoRua != null && !cond.statusSituacaoRua)
                throw new ValidationException("A quantidade de alimentações ao dia não deve ser informada para este caso.");

            if (cond.quantidadeAlimentacoesAoDiaSituacaoRua != null && domain.TP_Quantas_Vezes_Alimentacao.Any(x => x.codigo != cond.quantidadeAlimentacoesAoDiaSituacaoRua))
                throw new ValidationException("A quantidade de alimentações ao dia é inválida.");

            if (cond.statusAcompanhadoPorOutraInstituicao && !cond.statusSituacaoRua)
                throw new ValidationException("O acompanhamento por outra instituição não deve ser informado para este caso.");

            if (cond.statusPossuiReferenciaFamiliar && !cond.statusSituacaoRua)
                throw new ValidationException("A referência familiar não deve ser informada para este caso.");

            if (cond.statusRecebeBeneficio && !cond.statusSituacaoRua)
                throw new ValidationException("O recebimento de benefício não deve ser informado para este caso.");

            if (cond.statusTemAcessoHigienePessoalSituacaoRua && !cond.statusSituacaoRua)
                throw new ValidationException("O acesso à higiene pessoal não deve ser informado para este caso.");

            if (cond.statusPossuiReferenciaFamiliar && !cond.statusSituacaoRua)
                throw new ValidationException("A visita frequente não deve ser informada para este caso.");

            if (cond.tempoSituacaoRua != null && !cond.statusSituacaoRua)
                throw new ValidationException("O tempo de situação de rua não deve ser informado para este caso.");

            if (cond.tempoSituacaoRua != null && domain.TP_Sit_Rua.Any(x => x.codigo != cond.tempoSituacaoRua))
                throw new ValidationException("O tempo de situação de rua é inválido.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="cad"></param>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="Exception"></exception>
        public static void Validar(this IdentificacaoUsuarioCidadao cond, CadastroIndividual cad, DomainContainer domain)
        {
            var header = cad.UnicaLotacaoTransport;

            if (cond.nomeSocial != null)
            {
                if (cond.nomeSocial.Length > 70)
                    throw new ValidationException("O Nome Social não deve ter mais que 70 caracteres.");

                var n = cond.nomeSocial.Trim().RemoveDiacritics().ToUpperInvariant();

                if (Regex.IsMatch(n, "([^a-zA-Z '])"))
                    throw new ValidationException("O Nome Social é inválido.");
            }

            if (cond.codigoIbgeMunicipioNascimento != null)
            {
                if (cond.nacionalidadeCidadao != 1)
                    throw new ValidationException("O código IBGE do município de nascimento só deve ser informado para brasileiros.");

                if (domain.Cidade.All(x => x.CodIbge != cond.codigoIbgeMunicipioNascimento))
                    throw new ValidationException("O código IBGE do município de nascimento é inválido ou não está cadastrado.");
            }

            if (cond.dataNascimentoCidadao != null && !cond.dataNascimentoCidadao.Value.IsValidBirthDateTime(cad.UnicaLotacaoTransport.dataAtendimento))
                throw new ValidationException("A data de nascimento do cidadão é inválida.");

            try
            {
                if (cond.emailCidadao != null && ((cond.emailCidadao.Length < 6 || cond.emailCidadao.Length > 100) ||
                    (new System.Net.Mail.MailAddress(cond.emailCidadao)).Address != cond.emailCidadao))
                    throw new Exception();
            }
            catch
            {
                throw new ValidationException("O email do cidadão é inválido.");
            }

            if (domain.TP_Nacionalidade.All(x => x.codigo != cond.nacionalidadeCidadao))
                throw new ValidationException("A nacionalidade informada é inválida.");

            if (cond.nomeCidadao == null || !cond.nomeCidadao.NomeValido())
                throw new ValidationException("O Nome do cidadão é inválido.");

            if (cond.nomeMaeCidadao != null)
            {
                if (cond.desconheceNomeMae)
                    throw new ValidationException("O Nome da mãe do cidadão não deve ser informado.");

                if (!cond.nomeMaeCidadao.NomeValido())
                    throw new ValidationException("O Nome da mãe do cidadão é inválido.");
            }

            if (cond.nomePaiCidadao != null)
            {
                if (cond.desconheceNomePai)
                    throw new ValidationException("O Nome do pai do cidadão não deve ser informado.");

                if (!cond.nomePaiCidadao.NomeValido())
                    throw new ValidationException("O Nome do pai do cidadão é inválido.");
            }

            if (cond.cnsCidadao != null && !cond.cnsCidadao.isValidCns())
                throw new ValidationException("O CNS do cidadão está incorreto.");

            if (cond.cnsResponsavelFamiliar != null)
            {
                if (cond.statusEhResponsavel)
                    throw new ValidationException("O CNS do responsável familiar não deve ser informado neste caso.");

                if (!cond.cnsResponsavelFamiliar.isValidCns())
                    throw new ValidationException("O CNS do responsável familiar é inválido.");
            }
            else if (!cond.statusEhResponsavel)
                throw new ValidationException("O CNS do responsável familiar deve ser informado neste caso.");

            if (cond.telefoneCelular != null && (cond.telefoneCelular.Length < 10 || cond.telefoneCelular.Length > 11 ||
                Regex.IsMatch(cond.telefoneCelular ?? "-", "([^0-9])")))
                throw new ValidationException("O telefone celular do cidadão é inválido.");

            if (cond.numeroNisPisPasep != null &&
                (cond.numeroNisPisPasep.Length != 11 || Regex.IsMatch(cond.numeroNisPisPasep ?? "-", "([^0-9])")))
                throw new ValidationException("O número do NIS/PIS/PASEP do cidadão é inválido.");

            if (cond.paisNascimento != null)
            {
                if ((cond.paisNascimento != 31 && cond.nacionalidadeCidadao == 1) || (cond.paisNascimento == 31 && cond.nacionalidadeCidadao != 1))
                    throw new ValidationException("O país de nascimento não corresponde à nacionalidade do cidadão.");
                /* FIXME - Se houver problemas com o thrift no campo de paisNascimento para nacionalidadeCidadao = 2, descomentar o código abaixo */
                //else if (cond.nacionalidadeCidadao == 2)
                //    throw new ValidationException("O país de nascimento não deve ser informado para cidadão naturalizado.");
            }
            else if (cond.nacionalidadeCidadao == 3)
                throw new ValidationException("O país de nascimento do cidadão é obrigatório.");
            else if (cond.nacionalidadeCidadao == 1)
                throw new ValidationException("O país de nascimento do cidadão é inválido para a nacionalidade informada.");

            if (domain.TP_Raca_Cor.All(x => x.id_tp_raca_cor != cond.racaCorCidadao))
                throw new ValidationException("A Raça/Cor do cidadão é inválida.");

            if (domain.TP_Sexo.All(x => x.codigo != cond.sexoCidadao))
                throw new ValidationException("O Sexo do cidadão é inválido.");

            if (cond.racaCorCidadao == 5 && (cond.etnia == null || domain.Etnia.All(x => x.CodEtnia != cond.etnia)))
                throw new ValidationException("A etnia do cidadão é inválida.");

            if (cond.racaCorCidadao != 5 && cond.etnia != null)
                throw new ValidationException("A etnia do cidadão não deve ser informada para este caso.");

            if (cond.dtNaturalizacao == null && cond.nacionalidadeCidadao == 2)
                throw new ValidationException("A data de naturalização é obrigatória.");

            if (cond.dtNaturalizacao != null && cond.nacionalidadeCidadao != 2)
                throw new ValidationException("A data de naturalização só deve ser informada para cidadões naturalizados.");

            if (cond.dtNaturalizacao != null && (cond.dtNaturalizacao > header.dataAtendimento ||
                cond.dtNaturalizacao < cond.dataNascimentoCidadao))
                throw new ValidationException("A data de naturalização é inválida.");

            if (cond.portariaNaturalizacao != null && cond.nacionalidadeCidadao != 2)
                throw new ValidationException("A portaria da naturalização não pode ser informada.");

            if (cond.portariaNaturalizacao == null && cond.nacionalidadeCidadao == 2)
                throw new ValidationException("A portaria da naturalização é obrigatória.");

            if (cond.portariaNaturalizacao != null && cond.portariaNaturalizacao.Length > 16)
                throw new ValidationException("A portaria da naturalização aceita apenas 16 caracteres.");

            if (cond.dtEntradaBrasil != null && cond.nacionalidadeCidadao != 3)
                throw new ValidationException("A data de entrada no Brasil não deve ser preenchida.");

            if (cond.dtEntradaBrasil == null && cond.nacionalidadeCidadao == 3)
                throw new ValidationException("A data de entrada no Brasil é obrigatória.");

            if (cond.dtEntradaBrasil != null && (cond.dtEntradaBrasil > header.dataAtendimento ||
                cond.dtEntradaBrasil < cond.dataNascimentoCidadao))
                throw new ValidationException("A data de entrada no brasil é inválida.");

            if (cond.microarea != null && cond.stForaArea)
                throw new ValidationException("A microárea não deve ser informada quando fora de área.");

            if (cond.microarea == null && !cond.stForaArea)
                throw new ValidationException("A microárea é obrigatória.");

            if (cond.microarea != null && (cond.microarea.Length != 2 || !Regex.IsMatch(cond.microarea, "^([0-9][0-9])$")))
                throw new ValidationException("O código da microárea é inválido. Informe 2 caracteres numéricos.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="cad"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this InformacoesSocioDemograficas cond, CadastroIndividual cad, DomainContainer domain)
        {
            if (cond.DeficienciasCidadao.Count == 0 && cond.statusTemAlgumaDeficiencia)
                throw new ValidationException("Ao menos uma deficiência deve ser informada.");

            if (cond.DeficienciasCidadao.Count > 0 && !cond.statusTemAlgumaDeficiencia)
                throw new ValidationException("Nenhuma deficiência pode ser informada.");

            if (cond.DeficienciasCidadao.Count > 5)
                throw new ValidationException("Apenas 5 deficiências podem ser informadas.");

            if (cond.DeficienciasCidadao.Any(x => domain.TP_Deficiencia.All(y => y.codigo != x.id_tp_deficiencia_cidadao)))
                throw new ValidationException("Uma ou mais deficiências estão incorretas.");

            if (cond.grauInstrucaoCidadao != null && domain.TP_Curso.All(x => x.codigo != cond.grauInstrucaoCidadao))
                throw new ValidationException("O grau de instrução do cidadão é inválido.");

            if (cond.ocupacaoCodigoCbo2002 != null && domain.AS_ProfissoesTab.ToList().All(x => x.CodProfTab == null || x.CodProfTab.Trim() != cond.ocupacaoCodigoCbo2002.Trim()))
                throw new ValidationException("O CBO do cidadão é inválido.");

            if (cond.orientacaoSexualCidadao != null && !cond.statusDesejaInformarOrientacaoSexual)
                throw new ValidationException("A orientação sexual do cidadão não deve ser informada.");

            if (cond.orientacaoSexualCidadao != null && domain.TP_Orientacao_Sexual.All(x => x.codigo != cond.orientacaoSexualCidadao))
                throw new ValidationException("A orientação sexual do cidadão é inválida.");

            if (cond.povoComunidadeTradicional != null && !cond.statusMembroPovoComunidadeTradicional)
                throw new ValidationException("O povo de comunidade tradicional não deve ser informado para este caso.");

            if (cond.povoComunidadeTradicional != null && cond.povoComunidadeTradicional.Length > 100)
                throw new ValidationException("O povo de comunidade tradicioinal aceita somente 100 caracteres.");

            if (cond.relacaoParentescoCidadao != null && cad.IdentificacaoUsuarioCidadao1.statusEhResponsavel)
                throw new ValidationException("A relação de parentesco não pode ser preenchida.");

            if (cond.relacaoParentescoCidadao != null && domain.TP_Relacao_Parentesco.All(x => x.codigo != cond.relacaoParentescoCidadao)
                    && !cad.IdentificacaoUsuarioCidadao1.statusEhResponsavel)
                throw new ValidationException("A relação de parentesco é inválida.");

            if (cond.situacaoMercadoTrabalhoCidadao != null && domain.TP_Sit_Mercado.All(x => x.codigo != cond.situacaoMercadoTrabalhoCidadao))
                throw new ValidationException("A situação no mercado de trabalho é inválida.");

            if (cond.identidadeGeneroCidadao != null && !cond.statusDesejaInformarIdentidadeGenero)
                throw new ValidationException("A identidade de gênero não deve ser informada.");

            var resp = cad.UnicaLotacaoTransport.dataAtendimento.Date.AddYears(-9);

            if (cond.ResponsavelPorCrianca.Count > 0 && cad.IdentificacaoUsuarioCidadao1.dataNascimentoCidadao < resp)
                throw new ValidationException("Este cidadão não pode ser reponsável por criança.");

            if (cond.ResponsavelPorCrianca.Count > 6)
                throw new ValidationException("O limite de responsabilidade por criança é de 6.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cond"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this SaidaCidadaoCadastro cond)
        {
            if (cond.dataObito != null && cond.motivoSaidaCidadao != 135)
                throw new ValidationException("A data do óbito não deve ser informada.");

            if (cond.dataObito == null && cond.motivoSaidaCidadao == 135)
                throw new ValidationException("A data do óbito deve ser informada.");

            if (cond.numeroDO != null && cond.motivoSaidaCidadao != 135)
                throw new ValidationException("O número do documento de óbito não deve ser informado.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cad"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this CadastroIndividual cad, DomainContainer domain)
        {
            if (cad.UnicaLotacaoTransport == null)
                throw new ValidationException("Cabeçalho não encontrado.");

            if (cad.statusTermoRecusaCadastroIndividualAtencaoBasica && cad.CondicoesDeSaude1 != null)
                throw new ValidationException("Não é possível informar as condições de saúde para os casos de recusa.");

            cad.CondicoesDeSaude1?.Validar(cad, domain);

            if (cad.statusTermoRecusaCadastroIndividualAtencaoBasica && cad.EmSituacaoDeRua1 != null)
                throw new ValidationException("Não é possível informar as situações de rua para os casos de recusa.");

            cad.EmSituacaoDeRua1?.Validar(domain);

            if (!cad.statusTermoRecusaCadastroIndividualAtencaoBasica && cad.IdentificacaoUsuarioCidadao1 == null)
                throw new ValidationException("A identificação do cidadão é obrigatória.");

            cad.IdentificacaoUsuarioCidadao1?.Validar(cad, domain);

            if (!cad.statusTermoRecusaCadastroIndividualAtencaoBasica && cad.InformacoesSocioDemograficas1 == null)
                throw new ValidationException("As informações sociodemográficas do cidadão são obrigatórias.");

            cad.InformacoesSocioDemograficas1?.Validar(cad, domain);

            if (!cad.UnicaLotacaoTransport.OrigemVisita.enviarParaThrift)
            {
                cad.uuidFichaOriginadora = cad.id;
                cad.fichaAtualizada = false;
            }

            if (cad.uuidFichaOriginadora == null && cad.fichaAtualizada)
                throw new ValidationException("Informe o Uuid da ficha originadora.");

            cad.SaidaCidadaoCadastro1?.Validar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="cad"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this CondicaoMoradia cond, CadastroDomiciliar cad, DomainContainer domain)
        {
            var proibido = (new long[] { 7, 8, 9, 10, 11 }).Contains(cad.tipoDeImovel);

            if (cond.abastecimentoAgua != null && domain.TP_Abastecimento_Agua.All(x => x.codigo != cond.abastecimentoAgua))
                throw new ValidationException("O abastecimento de água da condição de moradia é inválido.");

            if (cond.areaProducaoRural != null && (proibido || cond.localizacao == 83))
                throw new ValidationException("A área de produção rural não deve ser definida para este imóvel.");

            if (cond.destinoLixo != null && domain.TP_Destino_Lixo.All(x => x.codigo != cond.destinoLixo))
                throw new ValidationException("O destino do lixo é inválido.");

            if (cond.formaEscoamentoBanheiro != null && domain.TP_Escoamento_Esgoto.All(x => x.codigo != cond.formaEscoamentoBanheiro))
                throw new ValidationException("A forma de escoamento do banheiro ou sanitário é inválida.");

            if (cond.localizacao != null && domain.TP_Localizacao.All(x => x.codigo != cond.localizacao))
                throw new ValidationException("A localização da moradia é inválida.");

            if (cond.materialPredominanteParedesExtDomicilio != null && proibido)
                throw new ValidationException("O material predominante das paredes externas do domicílio não pode ser informado para este tipo de imóvel.");

            if (cond.materialPredominanteParedesExtDomicilio != null && domain.TP_Construcao_Domicilio.All(x => x.codigo != cond.materialPredominanteParedesExtDomicilio))
                throw new ValidationException("O material predominante das paredes externas do domicílio é inválido.");

            if (cond.nuComodos != null && proibido)
                throw new ValidationException("A quantidade de cômodos não pode ser informada para este tipo de imóvel.");

            if (cond.nuComodos == 0 || cond.nuComodos > 99)
                throw new ValidationException("A quantidade de cõmodos é inválida");

            if (cond.nuMoradores != null && (cond.nuMoradores < cad.FamiliaRow.Sum(x => x.numeroMembrosFamilia) || cond.nuMoradores < cad.FamiliaRow.Count))
                throw new ValidationException("O número de moradores não pode ser menor que o total de membros / famílias cadastrados.");

            if (cond.situacaoMoradiaPosseTerra != null)
            {
                if (proibido)
                    throw new ValidationException("A situação de moradia não pode ser informada para este tipo de imóvel.");

                if (domain.TP_Situacao_Moradia.All(x => x.codigo != cond.situacaoMoradiaPosseTerra))
                    throw new ValidationException("A situação de moradia é inválida.");
            }

            if (cond.tipoAcessoDomicilio != null)
            {
                if (proibido)
                    throw new ValidationException("O tipo de acesso ao domicílio não pode ser informado para este tipo de imóvel.");

                if (domain.TP_Acesso_Domicilio.All(x => x.codigo != cond.tipoAcessoDomicilio))
                    throw new ValidationException("O tipo de acesso ao domicílio é inválido.");
            }

            if (cond.tipoDomicilio != null)
            {
                if (proibido)
                    throw new ValidationException("O tipo de domicílio não pode ser informado para este tipo de imóvel.");

                if (domain.TP_Domicilio.All(x => x.codigo != cond.tipoDomicilio))
                    throw new ValidationException("O tipo de domicílio é inválido.");
            }

            if (cond.aguaConsumoDomicilio != null && domain.TP_Tratamento_Agua.All(x => x.codigo != cond.aguaConsumoDomicilio))
                throw new ValidationException("O tipo de água de consumo no domicílio é inválido.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="cad"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this FamiliaRow cond, CadastroDomiciliar cad)
        {
            if (cond.dataNascimentoResponsavel != null && !cond.dataNascimentoResponsavel.Value.IsValidBirthDateTime(cad.UnicaLotacaoTransport.dataAtendimento))
                throw new ValidationException("A data de nascimento do responsável está incorreta.");

            if (cond.numeroCnsResponsavel == null || !cond.numeroCnsResponsavel.isValidCns())
                throw new ValidationException("O CNS do responsável está incorreto.");

            if (cond.numeroMembrosFamilia != null && (cond.numeroMembrosFamilia < 1 || cond.numeroMembrosFamilia > 99))
                throw new ValidationException("O número de membros da família está incorreto.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="cad"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this EnderecoLocalPermanencia cond, CadastroDomiciliar cad, DomainContainer domain)
        {
            if (string.IsNullOrEmpty(cond.bairro) || string.IsNullOrWhiteSpace(cond.bairro) || cond.bairro.Length < 1 || cond.bairro.Length > 72)
                throw new ValidationException("O bairro deve ter entre 1 e 72 caracteres.");

            if (string.IsNullOrEmpty(cond.cep) || string.IsNullOrWhiteSpace(cond.cep) || !Regex.IsMatch(cond.cep, "^([0-9]{8})$"))
                throw new ValidationException("O cep deve ter 8 caracteres numéricos.");

            if (cond.codigoIbgeMunicipio == null || domain.Cidade.All(x => x.CodIbge != cond.codigoIbgeMunicipio))
                throw new ValidationException("O código IBGE do município do local de permanência é inválido ou não está cadastrado.");

            if (cond.complemento != null && cond.complemento.Length > 30)
                throw new ValidationException("O complemento deve ter até 30 caracteres.");

            if (cond.nomeLogradouro == null || cond.nomeLogradouro.Length < 0 || cond.nomeLogradouro.Length > 72)
                throw new ValidationException("O Logradouro deve ter entre 1 e 72 caracteres.");

            if (cond.numero != null && cond.stSemNumero)
                throw new ValidationException("O número não pode ser definido para este local de permanência.");

            if (cond.numero == null && !cond.stSemNumero)
                throw new ValidationException("Informe o número do local de permanência.");

            if (cond.numero != null && (Regex.IsMatch(cond.numero, "([^0-9])") || cond.numero.Length < 1 || cond.numero.Length > 10))
                throw new ValidationException("O número do local de permanência deve ter entre 1 e 10 caracteres numéricos.");

            if (cond.numeroDneUf == null || domain.UF.OrderBy(x => x.DesUF).Select(RowNumberPad('0', 2)).All(x => x != cond.numeroDneUf))
                throw new ValidationException("O número DNE da UF é inválido.");

            if (cond.telefoneContato != null && (cond.telefoneContato.Length < 10 || cond.telefoneContato.Length > 11))
                throw new ValidationException("O telefone de contato é inválido.");

            if (cond.telefoneResidencia != null && (cond.telefoneResidencia.Length < 10 || cond.telefoneResidencia.Length > 11))
                throw new ValidationException("O telefone da residência é inválido.");

            if (cond.tipoLogradouroNumeroDne == null || domain.TB_MS_TIPO_LOGRADOURO.All(x => x.CO_TIPO_LOGRADOURO == null || x.CO_TIPO_LOGRADOURO.Trim() != cond.tipoLogradouroNumeroDne))
                throw new ValidationException("O tipo de logradouro é inválido.");

            if (cond.pontoReferencia != null && cond.pontoReferencia.Length > 40)
                throw new ValidationException("O ponto de referência deve ter até 40 caracteres.");

            if (cond.microarea != null && cond.stForaArea)
                throw new ValidationException("A microárea não deve ser informada para este local de permanência.");

            if (cond.microarea == null && !cond.stForaArea)
                throw new ValidationException("A microárea deve ser informada para este local de permanência.");

            if (cond.microarea != null && !Regex.IsMatch(cond.microarea, "^([0-9][0-9])$"))
                throw new ValidationException("A microárea é inválida. Informe 2 digitos numéricos.");
        }

        private static Func<UF, string> RowNumberPad(char c, int l)
        {
            var row = 1;

            return (i) => (row++).ToString().PadLeft(l, c);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cad"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public static async Task Validar(this CadastroDomiciliar cad, DomainContainer domain)
        {
            if (await domain.TP_Imovel.AllAsync(x => x.codigo != cad.tipoDeImovel))
                throw new ValidationException("O tipo de imóvel é inválido.");

            var proibido = (new long[] { 2, 3, 4, 5, 6, 12 }).Contains(cad.tipoDeImovel);

            cad.id = Guid.NewGuid();
            if (cad.AnimalNoDomicilio.Count > 0)
            {
                if (cad.tipoDeImovel != 1)
                    throw new ValidationException("Os animais no domicílio apenas podem ser atribuídos para imóvel do tipo Domicílio.");

                if (cad.statusTermoRecusa)
                    throw new ValidationException("Os animais no domicílio não podem ser atribuídos à este domicílio.");

                if (cad.AnimalNoDomicilio.Count > 4)
                    throw new ValidationException("São permitidos até quatro tipos de animais por domicílio.");
            }

            if (cad.CondicaoMoradia1 != null && (proibido || cad.statusTermoRecusa))
                throw new ValidationException("A condição de moradia não pode ser definida para este imóvel.");

            cad.CondicaoMoradia1?.Validar(cad, domain);

            if (cad.EnderecoLocalPermanencia1 != null && cad.statusTermoRecusa)
                throw new ValidationException("O endereço do local de permanência não deve ser informado para este cadastro domiciliar.");

            cad.EnderecoLocalPermanencia1?.Validar(cad, domain);

            if (cad.FamiliaRow.Count > 0 && cad.statusTermoRecusa)
                throw new ValidationException("Este cadastro domiciliar não deve conter famílias.");

            if (cad.FamiliaRow.Count > 0 && cad.tipoDeImovel != 1 && cad.FamiliaRow.Any(x => !x.stMudanca))
                throw new ValidationException("Todas as famílias devem ter o status de mudança para este tipo de imóvel.");

            cad.FamiliaRow.ToList().ForEach(x => x?.Validar(cad));

            if (cad.quantosAnimaisNoDomicilio != null)
            {
                if (cad.tipoDeImovel != 1 || !cad.stAnimaisNoDomicilio || cad.statusTermoRecusa)
                    throw new ValidationException("Não podem ser informada a quantidade de animais no domicílio.");

                if (cad.quantosAnimaisNoDomicilio <= 0)
                    throw new ValidationException("Informe o número de animais no domicílio.");

                if (cad.quantosAnimaisNoDomicilio < cad.AnimalNoDomicilio.Count || cad.quantosAnimaisNoDomicilio > 99)
                    throw new ValidationException($"O número de animais no domicílio deve ser maior ou igual à {cad.AnimalNoDomicilio.Count} e menor que 99.");
            }

            if (cad.stAnimaisNoDomicilio && (cad.tipoDeImovel != 1 || cad.statusTermoRecusa))
                throw new ValidationException("Não pode ser informados animais neste cadastro domiciliar.");

            if (!cad.UnicaLotacaoTransport.OrigemVisita.enviarParaThrift)
            {
                cad.uuidFichaOriginadora = cad.id;
                cad.fichaAtualizada = false;
            }

            if (cad.uuidFichaOriginadora == null && cad.fichaAtualizada)
                throw new ValidationException("Informe o Uuid da ficha originadora.");

            if (cad.InstituicaoPermanencia1 != null && !((new long[] { 7, 8, 9, 10, 11 }).Contains(cad.tipoDeImovel) || cad.statusTermoRecusa))
                throw new ValidationException("A instituição de permanência não pode ser informada.");

            cad.InstituicaoPermanencia1?.Validar(cad);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="cad"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validar(this InstituicaoPermanencia cond, CadastroDomiciliar cad)
        {
            if (cond.nomeInstituicaoPermanencia != null && cond.nomeInstituicaoPermanencia.Length > 100)
                throw new ValidationException("O nome da instituição de permanência é inválido");

            if (cond.nomeResponsavelTecnico == null || !cond.nomeResponsavelTecnico.NomeValido())
                throw new ValidationException("O nome do responsável técnico é inválido.");

            if (cond.cnsResponsavelTecnico != null && !cond.cnsResponsavelTecnico.isValidCns())
                throw new ValidationException("O CNS do responsável técnico é inválido.");

            if (cond.cargoInstituicao != null && cond.cargoInstituicao.Length > 100)
                throw new ValidationException("O cargo do responsável técnico é inválido.");

            if (cond.telefoneResponsavelTecnico != null && (Regex.IsMatch(cond.telefoneResponsavelTecnico, "([^0-9])") || cond.telefoneResponsavelTecnico.Length < 10 || cond.telefoneResponsavelTecnico.Length > 11))
                throw new ValidationException("O telefone do responsável técnico é inválido.");
        }
    }
}
