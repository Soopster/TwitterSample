using System.Net.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterSample.Hubs;
using TwitterSample.Services;
using IDependencyResolver = System.Web.Mvc.IDependencyResolver;

namespace Ninject.Mvc
{

    /// <summary>
    /// Resolves Dependencies Using Ninject
    /// </summary>
    public class NinjectResolver : IDependencyResolver
    {
        public IKernel Kernel { get; private set; }
        public NinjectResolver(params NinjectModule[] modules)
        {
            Kernel = new StandardKernel(modules);
        }

        public object GetService(Type serviceType)
        {
            return Kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Kernel.GetAll(serviceType);
        }
    }


    // List and Describe Necessary Modules
    public class NinjectModules
    {
        //Return Lists of Modules in the Application
        public static NinjectModule[] Modules
        {
            get
            {
                //Return Modules you want to use for DI
                return new[] { new MainModule() };
            }
        }

        //Main Module For Application. 
        public class MainModule : NinjectModule
        {
            public override void Load()
            {
                Bind<ITwitterStreamService>().To<TwitterStreamService>().InSingletonScope().WithConstructorArgument("clients", GlobalHost.ConnectionManager.GetHubContext<TwitterStreamHub>().Clients);
                Bind<ITwitterAuthService>().To<TwitterAuthService>();                
                Bind<ITwitterService>().To<TwitterService>().WithConstructorArgument("httpClient", new HttpClient());

                //Bind<IHubConnectionContext>().To(GlobalHost.ConnectionManager.GetHubContext<TwitterStreamHub>().Clients);
            }
        }

        //You can create as many Modules as you wish
    }


    /// <summary>
    /// Its job is to Register Ninject Modules and Resolve Dependencies
    /// </summary>
    public class NinjectContainer
    {
        private static NinjectResolver _resolver;

        //Register Ninject Modules
        public static void RegisterModules(NinjectModule[] modules)
        {
            _resolver = new NinjectResolver(modules);
            DependencyResolver.SetResolver(_resolver);
            GlobalHost.DependencyResolver = new NinjectDependencyResolver(_resolver.Kernel);
        }

        //Manually Resolve Dependencies
        public static T Resolve<T>()
        {
            return _resolver.Kernel.Get<T>();
        }
    }

    public class NinjectDependencyResolver : DefaultDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }

            _kernel = kernel;
        }

        public override object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType).Concat(base.GetServices(serviceType));
        }
    }
}