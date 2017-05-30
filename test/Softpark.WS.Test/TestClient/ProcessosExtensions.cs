﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Softpark.WS.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Rest;
    using Models;

    /// <summary>
    /// Extension methods for Processos.
    /// </summary>
    public static partial class ProcessosExtensions
    {
            /// <summary>
            /// Cadastrar cabeçalho das fichas
            /// </summary>
            /// Todas as fichas usarão esse cabeçalho
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='header'>
            /// Dados à serem enviados
            /// </param>
            public static Guid? EnviarCabecalho(this IProcessos operations, UnicaLotacaoTransportCadastroViewModel header)
            {
                return Task.Factory.StartNew(s => ((IProcessos)s).EnviarCabecalhoAsync(header), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Cadastrar cabeçalho das fichas
            /// </summary>
            /// Todas as fichas usarão esse cabeçalho
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='header'>
            /// Dados à serem enviados
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<Guid?> EnviarCabecalhoAsync(this IProcessos operations, UnicaLotacaoTransportCadastroViewModel header, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.EnviarCabecalhoWithHttpMessagesAsync(header, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Envia uma ficha de visita (child) para cadastro
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='child'>
            /// </param>
            public static bool? EnviarFichaVisita(this IProcessos operations, FichaVisitaDomiciliarChildCadastroViewModel child)
            {
                return Task.Factory.StartNew(s => ((IProcessos)s).EnviarFichaVisitaAsync(child), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Envia uma ficha de visita (child) para cadastro
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='child'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<bool?> EnviarFichaVisitaAsync(this IProcessos operations, FichaVisitaDomiciliarChildCadastroViewModel child, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.EnviarFichaVisitaWithHttpMessagesAsync(child, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Cadastro Individual
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cadInd'>
            /// </param>
            public static bool? EnviarCadastroIndividual(this IProcessos operations, CadastroIndividualViewModel cadInd)
            {
                return Task.Factory.StartNew(s => ((IProcessos)s).EnviarCadastroIndividualAsync(cadInd), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Cadastro Individual
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cadInd'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<bool?> EnviarCadastroIndividualAsync(this IProcessos operations, CadastroIndividualViewModel cadInd, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.EnviarCadastroIndividualWithHttpMessagesAsync(cadInd, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Cadastro Domiciliar
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cadDom'>
            /// </param>
            public static bool? EnviarCadastroDomiciliar(this IProcessos operations, CadastroDomiciliarViewModel cadDom)
            {
                return Task.Factory.StartNew(s => ((IProcessos)s).EnviarCadastroDomiciliarAsync(cadDom), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Cadastro Domiciliar
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cadDom'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<bool?> EnviarCadastroDomiciliarAsync(this IProcessos operations, CadastroDomiciliarViewModel cadDom, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.EnviarCadastroDomiciliarWithHttpMessagesAsync(cadDom, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Cadastro Atômico
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cadastros'>
            /// </param>
            public static bool? CadastramentoAtomico(this IProcessos operations, IList<AtomicTransporViewModel> cadastros)
            {
                return Task.Factory.StartNew(s => ((IProcessos)s).CadastramentoAtomicoAsync(cadastros), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Cadastro Atômico
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cadastros'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<bool?> CadastramentoAtomicoAsync(this IProcessos operations, IList<AtomicTransporViewModel> cadastros, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.CadastramentoAtomicoWithHttpMessagesAsync(cadastros, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Finaliza a transmissão de dados (encerra o token)
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='token'>
            /// </param>
            public static bool? FinalizarTransmissao(this IProcessos operations, Guid token)
            {
                return Task.Factory.StartNew(s => ((IProcessos)s).FinalizarTransmissaoAsync(token), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Finaliza a transmissão de dados (encerra o token)
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='token'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<bool?> FinalizarTransmissaoAsync(this IProcessos operations, Guid token, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.FinalizarTransmissaoWithHttpMessagesAsync(token, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}