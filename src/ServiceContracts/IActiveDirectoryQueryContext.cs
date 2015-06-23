using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Infrastructure.ServiceContracts
{
    public interface IActiveDirectoryQueryContext
    {
        string DirectoryEntry { get; }
        string Filter { get; }
        int PageLimit { get; }
        int SizeLimit { get; }
    }
}
