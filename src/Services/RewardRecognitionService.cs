using FCMA.RewardRecognition.Core.Contexts;
using FCMA.RewardRecognition.Core.Entities;
using FCMA.RewardRecognition.Core.Models;
using FCMA.RewardRecognition.Infrastructure.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using FCMA.RewardRecognition.Common.Email;
using FCMA.RewardRecognition.Common.ContextProvider;
using FCMA.RewardRecognition.Common.Logging;
using FCMA.RewardRecognition.Common.Configuration;
using System.Security.Principal;
using System.Configuration;

namespace FCMA.RewardRecognition.Infrastructure.Services
{
    public class RewardRecognitionService : IRewardRecognitionService
    {
        private readonly IRewardRecognitionContext _context;
        private readonly IRewardUserService _rewardUserService;
        private readonly IEmailService _emailService;
        private readonly IContextService _contextService;
        private readonly ILoggingService _loggingService;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IContextCacheService _cacheContext;

        private readonly string AuditApprover = ConfigurationManager.AppSettings["AuditApprover"];
        private readonly string SeniorAuditApprover = ConfigurationManager.AppSettings["SeniorAuditApprover"];

        public RewardRecognitionService(IRewardRecognitionContext context,
                                        IRewardUserService rewardUserService,
                                        IEmailService emailService,
                                        IContextService contextService,
                                        ILoggingService loggingService,
                                        IConfigurationRepository configurationRepository, 
                                        IContextCacheService cacheContext)
        {
            _context = context;
            _rewardUserService = rewardUserService;
            _emailService = emailService;
            _contextService = contextService;
            _loggingService = loggingService;
            _configurationRepository = configurationRepository;
            _cacheContext = cacheContext;
        }
        public async Task<List<RewardType>> ListRewardTypes()
        {
            try
            {
                return await _context.RewardTypes.Where(x => x.IsActive == true).ToListAsync();
            }
            catch (Exception ex)
            {
                _loggingService.LogError(this, "Exception in ListRewardTypes", ex);
                return null;
            }
        }

        public async Task<List<RewardReason>> ListRewardReasons()
        {
            try
            {
                return await _context.RewardReasons.Where(x => x.IsActive == true).ToListAsync();
            }
            catch (Exception ex)
            {
                _loggingService.LogError(this, "Exception in ListRewardReasons", ex);
                return null;
            }
        }

        public async Task<List<RewardStatus>> ListRewardStatuses()
        {
            try
            {
                return await _context.RewardStatus.Where(x => x.IsActive == true).ToListAsync();
            }
            catch (Exception ex)
            {
                _loggingService.LogError(this, "Exception in ListRewardStatuses", ex);
                return null;
            }
        }

        public async Task<Reward> GetRewardAsync(int id)
        {
            try
            {
                return await _context.Rewards.Where(x => x.RewardID == id).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _loggingService.LogError(this, "Exception in GetRewardAsync", ex);
                return null;
            }
        }

        public async Task<Reward> GetApprovedRewardAsync(int id)
        {
            try
            {
                return await _context.Rewards.Where(x => x.RewardID == id && x.RewardStatusID == 2).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _loggingService.LogError(this, "Exception in GetApprovedRewardAsync", ex);
                return null;
            }
        }

        public List<Reward> GetPendingRewards(string supervisor)
        {
            try
            {
                return (from r in _context.Rewards
                        join rt in _context.RewardTypes on r.RewardTypeID equals rt.RewardTypeID
                        join rr in _context.RewardReasons on r.RewardReasonID equals rr.RewardReasonID
                        join rs in _context.RewardStatus on r.RewardStatusID equals rs.RewardStatusID
                        where (rs.Code == "P" && r.Supervisor == supervisor)
                        select r
                        ).ToList();
            }
            catch (Exception ex)
            {
                _loggingService.LogError(this, "Exception in GetPendingRewards", ex);
                return null;
            }
        }
        public async Task<List<Reward>> InsertRewardAsync(List<Reward> rewards)
        {
            try
            {
                List<Reward> validRewards = new List<Reward>();
                foreach (Reward item in rewards)
                {
                    if (ValidateReward(item))
                    {
                        item.LastChangedDate = DateTime.Now;
                        item.CreatedDate = DateTime.Now;
                        item.PresentationDate = DateTime.Now;
                        validRewards.Add(item);
                    }
                }
                if (validRewards.Count > 0)
                {
                    _context.Rewards.AddRange(validRewards);
                    await _context.SaveChangesAsync();
                    await EmailSupervisorForApprovalAsync(validRewards);
                }
                return validRewards;
            }
            catch (Exception ex)
            {
                _loggingService.LogError(this, "Exception in inserting reward records", ex);
                return null;
            }
        }

        private bool ValidateReward(Reward reward)
        {
            bool isValid = false;
            if (reward != null)
            {
                int rewardTypeId = reward.RewardTypeID;
                int rewardStatusId = reward.RewardStatusID;
                int rewardReasonId = reward.RewardReasonID;
                string recipientUserId = reward.Recipient;
                string senderUserId = reward.CreatedBy;

                if (_rewardUserService.GetUserDetails(recipientUserId).IsLeaderCalc()) //If recipient is leader, approval goes to their leader
                {
                    reward.RewardStatusID = 1; //All rewards for Leaders must be approved
                    rewardStatusId = 1;
                    reward.Supervisor = _rewardUserService.GetSupervisor(recipientUserId).UserName;
                }
                if (_rewardUserService.IsSupervisorInAudit(recipientUserId))
                {
                    reward.RewardStatusID = 1; //All rewards for Audit must be approved
                    rewardStatusId = 1;
                    reward.Supervisor = AuditApprover;
                }
                if (recipientUserId == AuditApprover)//Randie is Audit leader must go up
                {
                    reward.RewardStatusID = 1; //All rewards for Audit must be approved
                    rewardStatusId = 1;
                    reward.Supervisor = SeniorAuditApprover;
                }

                RewardType rewardType = _cacheContext.RewardTypes.Where(t => t.RewardTypeID == rewardTypeId).SingleOrDefault();
                if (rewardType == null) return false;
                RewardStatus rewardStatus = _cacheContext.RewardStatuses.Where(t => t.RewardStatusID == rewardStatusId).SingleOrDefault();
                if (rewardStatus == null) return false;
                RewardReason rewardReason = _cacheContext.RewardReasons.Where(t => t.RewardReasonID == rewardReasonId).SingleOrDefault();
                if (rewardReason == null) return false;
                if (string.IsNullOrEmpty(recipientUserId) || string.IsNullOrEmpty(senderUserId) || recipientUserId.Equals(senderUserId, StringComparison.CurrentCultureIgnoreCase)) return false;

                isValid = true;
            }

            return isValid;
        }
        private async Task<bool> EmailSupervisorForApprovalAsync(List<Reward> rewards)
        {
            string message = string.Empty;
            string fromUserAddress = string.Empty;
            List<string> toUserAddress = null;
            List<string> ccUserAddress = new List<string>();
            string senderName = string.Empty;
            string siteLink = string.Empty;
            string toUserId = string.Empty;
            string fromUserId = string.Empty;
            string url = _contextService.GetContextProperties().BaseSiteUrl;
            siteLink = "<a href='" + url + "/#/approval" + "'>here</a>";
            ccUserAddress.Add(_configurationRepository.GetConfigurationValue<string>("TestEmailCopy"));

            try
            {
                foreach (Reward reward in rewards)
                {
                    string subject = "Reward Approval Request from ";
                    toUserAddress = new List<string>();
                    RewardType rewardType = _context.RewardTypes.Where(t => t.RewardTypeID == reward.RewardTypeID).SingleOrDefault();
                    if (rewardType != null && reward.RewardStatusID == 1)
                    {
                        toUserId = reward.Supervisor;
                        fromUserId = reward.CreatedBy;
                        RewardUserDataModel fromUser = _rewardUserService.GetUserDetails(fromUserId);
                        if (fromUser != null)
                        {
                            fromUserAddress = fromUser.EmailAddress;
                            senderName = fromUser.UserFullName;
                            subject = subject + senderName;
                        }
                        RewardUserDataModel toUser = _rewardUserService.GetUserDetails(toUserId);
                        if (toUser != null)
                        {
                            toUserAddress.Add(toUser.EmailAddress);
                        }

                        message = "You have received an approval request from " + senderName + ". Please click " + siteLink + " to retrieve the request, and then select approve or deny to complete the request.";

                        EmailArguments arguments = new EmailArguments(subject, message, toUserAddress, fromUserAddress, string.Empty, true, ccUserAddress);
                        EmailSendingResult result = _emailService.SendEmail(arguments);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError(this, "Exception in sending email to supervisor for Approval", ex);
                return false;
            }
        }

        public async Task<bool> UpdateRewardStatusAsync(int id, string statusCode)
        {
            int previousStatusId = 0;
            int statusId = 0;
            try
            {
                Reward reward = _context.Rewards.Where(x => x.RewardID == id).SingleOrDefault();
                if (reward != null)
                {
                    previousStatusId = reward.RewardStatusID;

                    RewardStatus rewardStatus = _context.RewardStatus.Where(s => s.Code.Equals(statusCode)).SingleOrDefault();

                    if (rewardStatus != null)
                    {
                        statusId = rewardStatus.RewardStatusID;
                        if (previousStatusId != statusId)
                        {
                            if (rewardStatus.Code.Equals("R"))  //Redemption
                            { 
                                reward.RedeemedBy = _contextService.GetUserName().Split('\\')[1];
                                reward.RedeemedDate = DateTime.Now;
                            }
                            reward.RewardStatusID = statusId;

                            reward.ChangedBy = _contextService.GetUserName().Split('\\')[1];
                            reward.LastChangedDate = DateTime.Now;

                            _context.Entry(reward).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                            _loggingService.LogInfo(this, "Upate status to " + statusId + " for reward id " + reward.RewardID);

                            string subject = "2020 Vision Request - Decision";
                            string message = string.Empty;
                            string fromUserAddress = string.Empty;
                            List<string> toUserAddress = new List<string>();
                            List<string> ccUserAddress = new List<string>();
                            string leaderName = string.Empty;
                            string siteLink = string.Empty;
                            string recipientUser = string.Empty;
                            string fromUserId = reward.Supervisor;
                            string toUserId = reward.CreatedBy;

                            string url = _contextService.GetContextProperties().BaseSiteUrl;
                            siteLink = "<a href='" + url + "/#/myRewards" + "'>here</a>";
                            ccUserAddress.Add(_configurationRepository.GetConfigurationValue<string>("TestEmailCopy"));

                            RewardUserModel fromUser = _rewardUserService.GetUserDetails(fromUserId);
                            if (fromUser != null)
                            {
                                fromUserAddress = fromUser.EmailAddress;
                                leaderName = fromUser.UserFullName;
                            }
                            RewardUserModel toUser = _rewardUserService.GetUserDetails(toUserId);
                            if (toUser != null)
                            {
                                toUserAddress.Add(toUser.EmailAddress);
                            }
                            RewardUserModel recipient = _rewardUserService.GetUserDetails(reward.Recipient);
                            if (recipient != null)
                            {
                                recipientUser = recipient.UserFullName;
                            }
                            if (rewardStatus.Code.Equals("A"))  //need to send email upon approval
                            {
                                message = "Congratulations! Your request to award " + recipientUser + " has been approved by your leader " + leaderName + ". Click " + siteLink + " to retrieve your voucher and follow next steps.";
                            }
                            if (rewardStatus.Code.Equals("D"))  //need to send email upon denial
                            {
                                message = "Your request to award " + recipientUser + " has been  denied by your leader " + leaderName + ". Please get in touch with your leader for more information.";
                            }

                            EmailArguments arguments = new EmailArguments(subject, message, toUserAddress, fromUserAddress, string.Empty, true, ccUserAddress);
                            EmailSendingResult result = _emailService.SendEmail(arguments);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError(this, "Exception in updating reward status", ex);
                return false;
            }
        }
        public List<Reward> GetMyRewards(string username)
        {
            try 
            {
                //((RewardRecognitionContext)_context).Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                //((RewardRecognitionContext)_context).Database.Log = s => _loggingService.LogInfo(this, s);

                return (from r in _context.Rewards
                        join rt in _context.RewardTypes on r.RewardTypeID equals rt.RewardTypeID
                        join rr in _context.RewardReasons on r.RewardReasonID equals rr.RewardReasonID
                        join rs in _context.RewardStatus on r.RewardStatusID equals rs.RewardStatusID
                        where (
                           (r.CreatedBy == username) ||
                           (r.Recipient == username && rs.Code == "A") ||
                           (r.Recipient == username && rs.Code == "R")
                        )
                        select r
                    )
                .ToList();
            }
            catch (Exception ex)
            {
                _loggingService.LogError(this, "Exception in GetMyRewards", ex);
                return null;
            }
        }
        public List<Reward> getRecentRewards()
        {
            try 
            {
                return (from r in _context.Rewards
                        join rt in _context.RewardTypes on r.RewardTypeID equals rt.RewardTypeID
                        join rr in _context.RewardReasons on r.RewardReasonID equals rr.RewardReasonID
                        join rs in _context.RewardStatus on r.RewardStatusID equals rs.RewardStatusID
                        where (
                           (rs.Code == "R")
                        )
                        select r
                    ).OrderByDescending(r => r.CreatedDate).Take(23)
                .ToList();
            }
            catch (Exception ex)
            {
                _loggingService.LogError(this, "Exception in getRecentRewards", ex);
                return null;
            }
        }
        
        public List<Reward> GetMyTeamRewards(string supervisorId)
        {
            try
            {
                List<RewardUserDataModel> myReports = _rewardUserService.GetAllLevelReports(supervisorId);
                List<string> allReports = new List<string>();
                foreach (var item in myReports)
                {
                    allReports.Add(item.UserName);
                }

                return (from r in _context.Rewards
                        join rt in _context.RewardTypes on r.RewardTypeID equals rt.RewardTypeID
                        join rr in _context.RewardReasons on r.RewardReasonID equals rr.RewardReasonID
                        join rs in _context.RewardStatus on r.RewardStatusID equals rs.RewardStatusID
                        where (allReports.Contains(r.CreatedBy) || allReports.Contains(r.Recipient))
                        select r).ToList();
            }
            catch (Exception ex)
            {
                _loggingService.LogError(this, "Exception in GetMyTeamRewards", ex);
                return null;
            }
        }
    }
}
