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
            return epoch.AddSeconds(unix);
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
            
            return unix.Value.FromUnix();
        }

        /// <summary>
        /// Convert a DateTime to unix date represantation
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>The unix date representation</returns>
        public static long ToUnix(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, date.Kind);
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
        /// <param name="epoch"></param>
        /// <returns></returns>
        public static ValidationResult ValidateESUSDate(long epoch)
        {
            var past = DateTime.UtcNow.Date.AddYears(-1).ToUnix();

            return epoch >= past ? ValidationResult.Success :
                new ValidationResult($"A data de atendimento tem mais de 1 (um) ano ou é inválida. Epoch atual: {DateTime.UtcNow.Date.ToUnix()}, epoch limite: {past}");
        }

        /// <summary>
        /// Valida data nascimento
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static ValidationResult ValidateBirthDate(long epoch, long limit)
        {
            return epoch.IsValidBirthDate(limit) ? ValidationResult.Success :
                new ValidationResult($"A data de nascimento não pode ser maior ou igual à data de atendimento. Nascimento: {epoch}, Limite: {limit}");
        }

        /// <summary>
        /// Valida data nascimento
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static bool IsValidBirthDate(this long epoch, long limit)
        {
            return epoch <= limit && limit.FromUnix().Date.AddYears(-130).ToUnix() <= epoch;
        }
    }
}
