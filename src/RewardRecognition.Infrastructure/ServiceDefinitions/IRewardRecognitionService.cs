using FCMA.RewardRecognition.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Infrastructure.ServiceDefinitions
{
    public interface IRewardRecognitionService
    {
        Task<Reward> GetReward(int id);
    }
}
