using FCMA.RewardRecognition.Core.Models;
using FCMA.RewardRecognition.Infrastructure.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FCMA.RewardRecognition.Infrastructure.Services
{
    public class RewardUserService : IRewardUserService
    {
        private static List<RewardUserDataModel> _rewardUsers { get; set; }
        private readonly IActiveDirectoryUserService _activeDirectoryUserService;
        private readonly string IsInRedeemGroup = ConfigurationManager.AppSettings["IsInRedeemGroup"];
        private readonly string IsInLeaderOverrideGroup = ConfigurationManager.AppSettings["IsInLeaderOverrideGroup"];
        private readonly string SupervisorIsInAudit = ConfigurationManager.AppSettings["SupervisorIsInAudit"];



        public RewardUserService(IActiveDirectoryUserService activeDirectoryUserService)
        {
            _activeDirectoryUserService = activeDirectoryUserService;
            ValidateRewardUserCacheState();
        }


        public List<RewardUserDataModel> RewardDataUsers
        {
            get
            {
                ValidateRewardUserCacheState();
                return _rewardUsers;
            }
        }

        public void ValidateRewardUserCacheState()
        {
            if (_rewardUsers == null)
            {
                _rewardUsers = _activeDirectoryUserService.GetAllActiveDirectoryUsers();
            }
        }


        public List<RewardUserDataModel> RewardUsers
        {
            get
            {
                ValidateRewardUserCacheState();
                return _rewardUsers.ToList<RewardUserDataModel>();
            }
        }

        public string GetLocation(string userName)
        {
            ValidateRewardUserCacheState();
            var user = _rewardUsers.SingleOrDefault(w => w.UserName.Trim().ToLower() == userName.Trim().ToLower());
            if (user != null)
                return user.OfficeLocation;
            throw new ArgumentException(string.Format("Invalid user name supplied or user name not in collection ({0}).", userName));
        }

        public string GetEmailAddress(string userName)
        {
            ValidateRewardUserCacheState();
            var user = _rewardUsers.SingleOrDefault(w => w.UserName.Trim().ToLower() == userName.Trim().ToLower());
            if (user != null)
                return user.EmailAddress;
            throw new ArgumentException(string.Format("Invalid user name supplied or user name not in collection ({0}).", userName));
        }


        public RewardUserDataModel GetSupervisor(string userName)
        {
            ValidateRewardUserCacheState();
            var user = _rewardUsers.SingleOrDefault(w => w.UserName.Trim().ToLower() == userName.Trim().ToLower());
            if (user != null)
            {
                var supervisor = _rewardUsers.SingleOrDefault(w => w.UserName.Trim().ToLower() == user.Manager.UserName.Trim().ToLower());
                if (supervisor != null)
                    return supervisor;
                throw new ArgumentException(string.Format("Invalid manager user name supplied or user name not in collection ({0}).", user.Manager.UserName));
            }
            throw new ArgumentException(string.Format("Invalid user name supplied or user name not in collection ({0}).", userName));
        }

        public RewardUserDataModel GetCurrentUser(IPrincipal domainUser)
        {
            ValidateRewardUserCacheState();
            var id = domainUser.Identity;
            string[] a = id.Name.Split('\\');
            string userName = a[1];
            var rewardUser = _rewardUsers.SingleOrDefault(w => w.UserName.Trim().ToLower() == userName.Trim().ToLower());
            if (rewardUser != null)
            {
                rewardUser.IsInRedeemGroup = IsRedeemGroup(domainUser);
                if (!rewardUser.IsLeader)
                {
                    rewardUser.IsLeader = IsLeaderOverrideGroup(domainUser);
                }
                return rewardUser;
            }
            throw new ArgumentException(string.Format("Invalid user name supplied or user name not in collection ({0}).", userName));
        }

        public RewardUserDataModel GetUserDetails(string userName)
        {
            ValidateRewardUserCacheState();
            var rewardUser = _rewardUsers.SingleOrDefault(w => w.UserName.Trim().ToLower() == userName.Trim().ToLower());
            if (rewardUser != null)
            {
                return rewardUser;
            }
            throw new ArgumentException(string.Format("Invalid user name supplied or user name not in collection ({0}).", userName));
        }

        private bool IsRedeemGroup(IPrincipal user)
        {
            return CheckADGroup(user, IsInRedeemGroup);
        }
        public bool IsSupervisorInAudit(string user)
        {
            var usersSupervisor = GetSupervisor(user);
            foreach (var auditSupervisors in SupervisorIsInAudit.Split(',').Select(v => v.Trim()))
            {
                if (usersSupervisor.UserName.Trim().ToLower() == auditSupervisors.Trim().ToLower())
                    return true;
            }
            return false;
        }
        private bool IsLeaderOverrideGroup(IPrincipal user)
        {
            return CheckADGroup(user, IsInLeaderOverrideGroup);
        }
        private bool CheckADGroup(IPrincipal user, string group)
        {
            foreach (var role in group.Split(',').Select(v => v.Trim()))
            {
                if (user.IsInRole(role))
                    return true;
            }
            return false;
        }


        public List<RewardUserDataModel> GetAllLevelReports(string supervisorId)
        {
            ValidateRewardUserCacheState();
            List<RewardUserDataModel> allReports = new List<RewardUserDataModel>();
            if (!string.IsNullOrEmpty(supervisorId))
            {
                //business rule: all level reports include supervisor him/herself
                allReports.Add(RewardDataUsers.Where(s => s.UserName.Equals(supervisorId, StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault());
                List<RewardUserDataModel> directReports = RewardDataUsers.Where(w => (!string.IsNullOrEmpty(w.Manager.UserName) && w.Manager.UserName.Equals(supervisorId, StringComparison.CurrentCultureIgnoreCase))).ToList();
                allReports.AddRange(GetDirectReportsInternal(directReports));
            }
            return allReports;
        }

        private List<RewardUserDataModel> GetDirectReportsInternal(List<RewardUserDataModel> directReports)
        {
            List<RewardUserDataModel> subReports = new List<RewardUserDataModel>();
            if (directReports.Count > 0)
            {
                subReports.AddRange(directReports);
                foreach (RewardUserDataModel item in directReports)
                {
                    subReports.AddRange(GetDirectReportsInternal(RewardDataUsers.Where(w => (!string.IsNullOrEmpty(w.Manager.UserName) && w.Manager.UserName.Equals(item.UserName, StringComparison.CurrentCultureIgnoreCase))).ToList()));
                }
            }
            return subReports;
        }
    }
}
