using FCMA.RewardRecognition.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Infrastructure.ServiceContracts
{
    public interface IContextCacheService
    {
        List<RewardReason> RewardReasons { get; }
        List<RewardStatus> RewardStatuses { get; }
        List<RewardType> RewardTypes { get; }
    }
}
