using System.Web.Http;
using Microsoft.Practices.Unity;
using FCMA.RewardRecognition.Infrastructure.ServiceContracts;
using FCMA.RewardRecognition.Infrastructure.Services;
using FCMA.RewardRecognition.Core.Contexts;
using FCMA.RewardRecognition.Common.Configuration;
using FCMA.RewardRecognition.Common.ContextProvider;
using FCMA.RewardRecognition.Common.Logging;
using FCMA.RewardRecognition.Common.Email;

namespace FCMA.RewardRecognition.Web
{
    public class UnityConfiguration
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IConfigurationRepository, ConfigFileConfigurationRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IContextService, HttpContextService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILoggingService, Log4NetLoggingService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmailService, SystemNetEmailService>(new HierarchicalLifetimeManager());
            container.RegisterType<IActiveDirectoryQueryContext, ActiveDirectoryQueryContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IActiveDirectoryUserService, ActiveDirectoryUserService>(new HierarchicalLifetimeManager());
            container.RegisterType<IRewardUserService, RewardUserService>(new HierarchicalLifetimeManager());
            container.RegisterType<IRewardRecognitionService, RewardRecognitionService>(new HierarchicalLifetimeManager());
            container.RegisterType<IRewardRecognitionContext, RewardRecognitionContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IContextService, HttpContextService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmailService, SystemNetEmailService>(new HierarchicalLifetimeManager());
            container.RegisterType<IContextCacheService, ContextCacheService>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

        }



    }
}