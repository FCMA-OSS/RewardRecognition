using FCMA.RewardRecognition.Common.Configuration;
using FCMA.RewardRecognition.Infrastructure.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
    public class ActiveDirectoryQueryContext : IActiveDirectoryQueryContext
    {
        private readonly IConfigurationRepository _config;
        public ActiveDirectoryQueryContext(IConfigurationRepository configurationRepository)
        {
            _config = configurationRepository;
        }


        public string DirectoryEntry
        {
            get { return _config.GetConfigurationValue<string>("AD_DirectoryEntry"); }
        }

        public string Filter
        {
            get { return _config.GetConfigurationValue<string>("AD_Filter"); }
        }

        public int PageLimit
        {
            get { return _config.GetConfigurationValue<int>("AD_PageLimit"); }
        }

        public int SizeLimit
        {
            get { return _config.GetConfigurationValue<int>("AD_SizeLimit"); }
        }
    }
}
