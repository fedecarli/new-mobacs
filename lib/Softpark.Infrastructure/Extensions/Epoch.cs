using System;
using System.ComponentModel.DataAnnotations;

namespace Softpark.Infrastructure.Extras
{
    /// <summary>
    /// Unix era extension
    /// </summary>
    public static class Epoch
    {
        /// <summary>
        /// Convert unix date representation to DateTime
        /// </summary>
        /// <param name="unix">The unix date representation.</param>
        /// <param name="isUtc">Set false if unix date representation is not a UTC.</param>
        /// <returns>DateTime</returns>
        public static DateTime FromUnix(this long unix, bool isUtc = true)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, isUtc ? DateTimeKind.Utc : DateTimeKind.Local);
            return epoch.AddSeconds(unix).ToLocalTime();
        }

        /// <summary>
        /// Convert unix date representation to DateTime
        /// </summary>
        /// <param name="unix">The unix date representation.</param>
        /// <param name="isUtc">Set false if unix date representation is not a UTC.</param>
        /// <returns>DateTime</returns>
        public static DateTime? FromUnix(this long? unix, bool isUtc = true)
        {
            if (!unix.HasValue) return null;
            
            return unix.Value.FromUnix(isUtc);
        }

        /// <summary>
        /// Convert a DateTime to unix date represantation
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>The unix date representation</returns>
        public static long ToUnix(this DateTime date)
        {
            date = date.ToUniversalTime();
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return (long)(date - epoch).TotalSeconds;
        }

        /// <summary>
        /// Convert a DateTime to unix date represantation
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>The unix date representation</returns>
        public static long? ToUnix(this DateTime? date)
        {
            if (!date.HasValue) return null;

            return date.Value.ToUnix();
        }

        /// <summary>
        /// Valida data eSUS
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ValidationResult ValidateESUSDateTime(DateTime data)
        {
            var past = DateTime.Now.Date.AddYears(-1);

            return data >= past ? ValidationResult.Success :
                new ValidationResult($"A data de atendimento tem mais de 1 (um) ano ou é inválida. Data atual: {DateTime.UtcNow}, Data limite: {past}, Data informada: {data}");
        }

        /// <summary>
        /// Valida data eSUS
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ValidationResult ValidateESUSDateTime(DateTime? data)
        {
            var past = DateTime.Now.Date.AddYears(-1);

            if (data == null)
                return new ValidationResult("Informe uma data.");

            return data >= past ? ValidationResult.Success :
                new ValidationResult($"A data de atendimento tem mais de 1 (um) ano ou é inválida. Data atual: {DateTime.UtcNow}, Data limite: {past}, Data informada: {data}");
        }

        /// <summary>
        /// Valida data eSUS
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public static ValidationResult ValidateESUSEpoch(long epoch)
        {
            var data = epoch.FromUnix();

            return ValidateESUSDateTime(data);
        }

        /// <summary>
        /// Valida data nascimento
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static ValidationResult ValidateBirthDate(long epoch, long limit)
        {
            return epoch.IsValidBirthEpoch(limit) ? ValidationResult.Success :
                new ValidationResult($"A data de nascimento não pode ser maior ou igual à data de atendimento. Nascimento: {epoch}, Limite: {limit}");
        }

        /// <summary>
        /// Valida data nascimento
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static ValidationResult ValidateBirthDateTime(DateTime epoch, DateTime limit)
        {
            return epoch.IsValidBirthDateTime(limit) ? ValidationResult.Success :
                new ValidationResult($"A data de nascimento não pode ser maior ou igual à data de atendimento. Nascimento: {epoch:dd/MM/yyyy}, Limite: {limit:dd/MM/yyyy}");
        }

        /// <summary>
        /// Valida data nascimento
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static bool IsValidBirthEpoch(this long epoch, long limit)
        {
            var e = epoch.FromUnix();
            var l = limit.FromUnix().AddYears(-130);

            return epoch <= limit && l <= e;
        }

        /// <summary>
        /// Valida data nascimento
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static bool IsValidBirthDateTime(this DateTime epoch, DateTime limit)
        {
            return epoch <= limit && limit.AddYears(-130) <= epoch;
        }
    }
}
