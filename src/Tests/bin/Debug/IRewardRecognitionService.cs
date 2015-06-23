using FCMA.RewardRecognition.Core.Entities;
using FCMA.RewardRecognition.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Infrastructure.ServiceContracts
{
    public interface IRewardRecognitionService
    {
        Task<Reward> GetRewardAsync(int id);
        Task<Reward> GetApprovedRewardAsync(int id);
        Task<List<RewardType>> ListRewardTypes();

        Task<List<RewardReason>> ListRewardReasons();
        Task<List<RewardStatus>> ListRewardStatuses();


        List<Reward> GetPendingRewards(string supervisor);
        Task<List<Reward>> InsertRewardAsync(List<Reward> rewards);

        List<Reward> GetMyRewards(string username);
        List<Reward> getRecentRewards();        

        List<Reward> GetMyTeamRewards(string username);
        Task<bool> UpdateRewardStatusAsync(int id, string statusCode);
    }
}
