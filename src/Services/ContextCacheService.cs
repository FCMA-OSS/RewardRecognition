using FCMA.RewardRecognition.Core.Contexts;
using FCMA.RewardRecognition.Core.Entities;
using FCMA.RewardRecognition.Infrastructure.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Infrastructure.Services
{
    public class ContextCacheService : IContextCacheService
    {
        private readonly IRewardRecognitionContext _context;
        public ContextCacheService(IRewardRecognitionContext context)
        {
            _context = context;
        }

        private static List<RewardType> _rewardTypes;
        public List<RewardType> RewardTypes
        {
            get
            {

                if (_rewardTypes == null)
                    _rewardTypes = _context.RewardTypes.ToList();
                return _rewardTypes;

            }
        }

        private static List<RewardStatus> _rewardStatuses;
        public List<RewardStatus> RewardStatuses
        {
            get
            {

                if (_rewardStatuses == null)
                    _rewardStatuses = _context.RewardStatus.ToList();
                return _rewardStatuses;

            }
        }

        private static List<RewardReason> _rewardReasons;
        public List<RewardReason> RewardReasons
        {
            get
            {

                if (_rewardReasons == null)
                    _rewardReasons = _context.RewardReasons.ToList();
                return _rewardReasons;

            }
        }


    }
}
