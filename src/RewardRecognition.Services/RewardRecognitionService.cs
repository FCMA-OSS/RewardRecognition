using FCMA.RewardRecognition.Core.Entities;
using FCMA.RewardRecognition.Infrastructure.Contexts;
using FCMA.RewardRecognition.Infrastructure.ServiceDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Services
{
    public class RewardRecognitionService : IRewardRecognitionService
    {
        private readonly IRewardRecognitionContext _context;
        public RewardRecognitionService(IRewardRecognitionContext context)
        {
            _context = context;
        }
        public Task<Reward> GetReward(int id)
        {
            throw new NotImplementedException();
        }
    }
}
