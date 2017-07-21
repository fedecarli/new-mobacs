using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#pragma warning disable 1591
namespace Softpark.Infrastructure.Extensions
{
    /// <summary>
    /// Esta extenção é para validar um CNS
    /// </summary>
    /// <remarks>http://esusab.github.io/integracao/docs/algoritmo_CNS.html</remarks>
    public static class Cns
    {
        private static dynamic _service;

        private static bool _valid1Or2(string cns)
        {
            float soma;
            float resto, dv;
            string pis = string.Empty;
            string resultado = string.Empty;
            pis = cns.Substring(0, 11);

            soma = (Convert.ToInt32(pis.Substring(0, 1)) * 15);
            soma += (Convert.ToInt32(pis.Substring(1, 1)) * 14);
            soma += (Convert.ToInt32(pis.Substring(2, 1)) * 13);
            soma += (Convert.ToInt32(pis.Substring(3, 1)) * 12);
            soma += (Convert.ToInt32(pis.Substring(4, 1)) * 11);
            soma += (Convert.ToInt32(pis.Substring(5, 1)) * 10);
            soma += (Convert.ToInt32(pis.Substring(6, 1)) * 9);
            soma += (Convert.ToInt32(pis.Substring(7, 1)) * 8);
            soma += (Convert.ToInt32(pis.Substring(8, 1)) * 7);
            soma += (Convert.ToInt32(pis.Substring(9, 1)) * 6);
            soma += (Convert.ToInt32(pis.Substring(10, 1)) * 5);

            resto = soma % 11;
            dv = 11 - resto;

            if (dv == 11)
            {
                dv = 0;
            }

            if (dv == 10)
            {
                soma = (Convert.ToInt32(pis.Substring(0, 1)) * 15) +
                (Convert.ToInt32(pis.Substring(1, 1)) * 14) +
                (Convert.ToInt32(pis.Substring(2, 1)) * 13) +
                (Convert.ToInt32(pis.Substring(3, 1)) * 12) +
                (Convert.ToInt32(pis.Substring(4, 1)) * 11) +
                (Convert.ToInt32(pis.Substring(5, 1)) * 10) +
                (Convert.ToInt32(pis.Substring(6, 1)) * 9) +
                (Convert.ToInt32(pis.Substring(7, 1)) * 8) +
                (Convert.ToInt32(pis.Substring(8, 1)) * 7) +
                (Convert.ToInt32(pis.Substring(9, 1)) * 6) +
                (Convert.ToInt32(pis.Substring(10, 1)) * 5) + 2;

                resto = soma % 11;
                dv = 11 - resto;
                resultado = pis + "001" + ((int)dv);
            }
            else
            {
                resultado = pis + "000" + ((int)dv);
            }

            if (!cns.Equals(resultado))
            {
                return (false);
            }
            else
            {
                return (true);
            }
        }

        private static bool _valid7Or8Or9(string cns)
        {
            float soma = (Convert.ToInt32(cns.Substring(0, 1)) * 15) +
            (Convert.ToInt32(cns.Substring(1, 1)) * 14) +
            (Convert.ToInt32(cns.Substring(2, 1)) * 13) +
            (Convert.ToInt32(cns.Substring(3, 1)) * 12) +
            (Convert.ToInt32(cns.Substring(4, 1)) * 11) +
            (Convert.ToInt32(cns.Substring(5, 1)) * 10) +
            (Convert.ToInt32(cns.Substring(6, 1)) * 9) +
            (Convert.ToInt32(cns.Substring(7, 1)) * 8) +
            (Convert.ToInt32(cns.Substring(8, 1)) * 7) +
            (Convert.ToInt32(cns.Substring(9, 1)) * 6) +
            (Convert.ToInt32(cns.Substring(10, 1)) * 5) +
            (Convert.ToInt32(cns.Substring(11, 1)) * 4) +
            (Convert.ToInt32(cns.Substring(12, 1)) * 3) +
            (Convert.ToInt32(cns.Substring(13, 1)) * 2) +
            (Convert.ToInt32(cns.Substring(14, 1)) * 1);

            if (soma % 11 != 0)
            {
                return (false);
            }
            else
            {
                return (true);
            }
        }

        public static bool isValidCns(this string cns)
        {
            if (cns.Trim().Length != 15 || !(new[] { '1', '2', '7', '8', '9' }).Contains(cns[0]))
            {
                return (false);
            }

            if ((new[] { '1', '2' }).Contains(cns[0])) return _valid1Or2(cns);
            return _valid7Or8Or9(cns);
        }

        public static ValidationResult ValidateCNS(string cns)
        {
            return cns.isValidCns() ? ValidationResult.Success : new ValidationResult("Código CNS inválido.");
        }

        public static void SetValidationService(dynamic service)
        {
            _service = service;
        }
    }
}
