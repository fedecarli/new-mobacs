using System;
using System.Collections.Generic;

namespace Softpark.WS.ViewModels
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class SomeErrors<T> : Exception where T : class
    {
        public SomeErrors(IDictionary<T, IList<Exception>> exceptions) : base("Um ou mais erros ocorreram durante a operação.")
        {
            Errors = exceptions;
        }

        public IDictionary<T, IList<Exception>> Errors { get; private set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}