using FCMA.RewardRecognition.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Infrastructure.ServiceContracts
{
    public interface IRewardUserService
    {
        string GetEmailAddress(string userName);
        string GetLocation(string userName);
        RewardUserDataModel GetSupervisor(string userName);
        List<RewardUserDataModel> RewardDataUsers { get; }
        List<RewardUserDataModel> GetAllLevelReports(string supervisorId);
        List<RewardUserDataModel> RewardUsers { get; }
        RewardUserDataModel GetCurrentUser(IPrincipal user);
        RewardUserDataModel GetUserDetails(string user);
        void ValidateRewardUserCacheState();
        bool IsSupervisorInAudit(string user);
    }
}
