using System.Configuration;

namespace System
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class Parametros
    {
        public static int idPaisCliente => Convert.ToInt32(ConfigurationManager.AppSettings[nameof(idPaisCliente)]);
        public static string nomePaisCliente => ConfigurationManager.AppSettings[nameof(nomePaisCliente)];
        public static int idMunicipioCliente => Convert.ToInt32(ConfigurationManager.AppSettings[nameof(idMunicipioCliente)]);
        public static string nomeCliente => ConfigurationManager.AppSettings[nameof(nomeCliente)];
        public static string estadoCliente => ConfigurationManager.AppSettings[nameof(estadoCliente)];
        public static string siglaEstadoCliente => ConfigurationManager.AppSettings[nameof(siglaEstadoCliente)];
        public static string nomeInicialSistema => ConfigurationManager.AppSettings[nameof(nomeInicialSistema)];
        public static string logoInicialSistema => ConfigurationManager.AppSettings[nameof(logoInicialSistema)];
        public static bool exibirMensagemSistema => !string.IsNullOrEmpty(ConfigurationManager.AppSettings[nameof(exibirMensagemSistema)]) &&
            ConfigurationManager.AppSettings[nameof(exibirMensagemSistema)] == "true";
        public static string mensagemSistema => ConfigurationManager.AppSettings[nameof(mensagemSistema)];
        public static string rodapeMensagemSistema => ConfigurationManager.AppSettings[nameof(rodapeMensagemSistema)];
        public static string corPrincipalSistema => ConfigurationManager.AppSettings[nameof(corPrincipalSistema)];
        public static string corMenuTop => ConfigurationManager.AppSettings[nameof(corMenuTop)];
        public static string corMenuTopHover => ConfigurationManager.AppSettings[nameof(corMenuTopHover)];
        public static string corLinks => ConfigurationManager.AppSettings[nameof(corLinks)];
        public static string corHoverLinks => ConfigurationManager.AppSettings[nameof(corHoverLinks)];
        public static string corHoverMenu => ConfigurationManager.AppSettings[nameof(corHoverMenu)];
        public static string corFundoMenu => ConfigurationManager.AppSettings[nameof(corFundoMenu)];
        public static string corFundoMenuActive => ConfigurationManager.AppSettings[nameof(corFundoMenuActive)];
        public static string corFundoAbaActive => ConfigurationManager.AppSettings[nameof(corFundoAbaActive)];
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}