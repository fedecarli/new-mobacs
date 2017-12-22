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
        public static DateTime FromUnix(this long unix, bool isUtc = true) =>
            new DateTime(1970, 1, 1, 0, 0, 0, 0, isUtc ? DateTimeKind.Utc : DateTimeKind.Local).AddSeconds(unix).ToLocalTime();

        /// <summary>
        /// Convert unix date representation to DateTime
        /// </summary>
        /// <param name="unix">The unix date representation.</param>
        /// <param name="isUtc">Set false if unix date representation is not a UTC.</param>
        /// <returns>DateTime</returns>
        public static DateTime? FromUnix(this long? unix, bool isUtc = true) =>
            unix?.FromUnix(isUtc);

        /// <summary>
        /// Convert a DateTime to unix date represantation
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>The unix date representation</returns>
        public static long ToUnix(this DateTime date) =>
            (long)(date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

        /// <summary>
        /// Convert a DateTime to unix date represantation
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>The unix date representation</returns>
        public static long? ToUnix(this DateTime? date) =>
            date?.ToUnix();

        /// <summary>
        /// Valida data eSUS
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ValidationResult ValidateESUSDateTime(DateTime data) =>
            data >= DateTime.Now.Date.AddYears(-1) ? ValidationResult.Success :
                new ValidationResult($"A data de atendimento tem mais de 1 (um) ano ou é inválida. Data atual: {DateTime.UtcNow}, Data limite: {DateTime.Now.Date.AddYears(-1)}, Data informada: {data}");

        /// <summary>
        /// Valida data eSUS
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ValidationResult ValidateESUSDateTime(DateTime? data) =>
            data == null ? new ValidationResult("Informe uma data.") : ValidateESUSDateTime(data.Value);

        /// <summary>
        /// Valida data eSUS
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public static ValidationResult ValidateESUSEpoch(long epoch) =>
            ValidateESUSDateTime(epoch.FromUnix());

        /// <summary>
        /// Valida data nascimento
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static ValidationResult ValidateBirthDate(long epoch, long limit) =>
            ValidateBirthDateTime(epoch.FromUnix(), limit.FromUnix());

        /// <summary>
        /// Valida data nascimento
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static ValidationResult ValidateBirthDateTime(DateTime epoch, DateTime limit) =>
            epoch.IsValidBirthDateTime(limit) ? ValidationResult.Success :
                new ValidationResult($"A data de nascimento não pode ser maior ou igual à data de atendimento. Nascimento: {epoch:dd/MM/yyyy}, Limite: {limit:dd/MM/yyyy}");

        /// <summary>
        /// Valida data nascimento
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static bool IsValidBirthEpoch(this long epoch, long limit) =>
            epoch.FromUnix().IsValidBirthDateTime(limit.FromUnix());

        /// <summary>
        /// Valida data nascimento
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static bool IsValidBirthDateTime(this DateTime epoch, DateTime limit) =>
            epoch <= limit && limit.AddYears(-130) <= epoch;
    }
}
