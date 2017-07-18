using System;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Syntax;
using System.Runtime.InteropServices;

#pragma warning disable 1591
// ReSharper disable once CheckNamespace
namespace Softpark.WS.App_Start
{
    /// <summary>
    /// Configuração de injeção de dependência
    /// </summary>
    public class NinjectDependencyScope : IDependencyScope
    {
        private IResolutionRoot _resolver;

        public NinjectDependencyScope(IResolutionRoot resolver)
        {
            _resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            if (_resolver == null)
                throw new ObjectDisposedException("this", "This scope has been disposed");

            return _resolver.TryGet(serviceType);
        }

        public System.Collections.Generic.IEnumerable<object> GetServices(Type serviceType)
        {
            if (_resolver == null)
                throw new ObjectDisposedException("this", "This scope has been disposed");

            return _resolver.GetAll(serviceType);
        }

        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var disposable = _resolver as IDisposable;
                disposable?.Dispose();
                _resolver = null;
            }
            // free native resources if there are any.  
            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
        }

        ~NinjectDependencyScope()
        {
            Dispose(false);
        }
    }
    
#pragma warning disable CA1063
    // This class is the resolver, but it is also the global scope
    // so we derive from NinjectScope.
    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel) : base(kernel)
        {
            _kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(_kernel.BeginBlock());
        }
    }
#pragma warning restore CA1063
}