using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCMA.RewardRecognition.Core.Contexts;
using FCMA.RewardRecognition.Infrastructure.ServiceContracts;
using FCMA.RewardRecognition.Common.Email;
using FCMA.RewardRecognition.Common.ContextProvider;
using FCMA.RewardRecognition.Common.Logging;
using FCMA.RewardRecognition.Common.Configuration;
using Moq;
using FCMA.RewardRecognition.Infrastructure.Services;
using System.Collections.Generic;
using FCMA.RewardRecognition.Core.Entities;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;

namespace FCMA.RewardRecognition.Tests.Services
{
    [TestClass]
    public class RewardRecognitionServiceTest
    {
        private IRewardRecognitionService _rewardRecognitionService;       

        private IRewardRecognitionService SetupRewardRecognitionService()
        {
            IRewardRecognitionService service = null;

            var _configurationRepository = new Mock<IConfigurationRepository>();
            _configurationRepository.Setup(c => c.GetConfigurationValue<string>("SmtpServer")).Returns("mySmtpServer");
            var _logggingService = new Mock<ILoggingService>();
            var _contextService = new Mock<IContextService>();
            var _emailService = new Mock<IEmailService>();
            var _rewardUserService = new Mock<IRewardUserService>();
            var _context = new Mock<IRewardRecognitionContext>();
            var _cacheContext = new Mock<IContextCacheService>();

            var myFakeReward = new List<Reward>
            {
                new Reward
                {
                Recipient = "jdoe",
                RewardTypeID = 1,
                RewardReasonID = 1,
                RewardStatusID = 1,
                CreatedBy = "ssmith"
                }
            }.AsQueryable();

            var dbSetMockReward = new Mock<DbSet<Reward>>();
            dbSetMockReward.As<IQueryable<Reward>>().Setup(m => m.Provider).Returns(myFakeReward.Provider);
            dbSetMockReward.As<IQueryable<Reward>>().Setup(m => m.Expression).Returns(myFakeReward.Expression);
            dbSetMockReward.As<IQueryable<Reward>>().Setup(m => m.ElementType).Returns(myFakeReward.ElementType);
            dbSetMockReward.As<IQueryable<Reward>>().Setup(m => m.GetEnumerator()).Returns(myFakeReward.GetEnumerator());
            _context.Setup(x => x.Rewards).Returns(dbSetMockReward.Object);

            var myFakeRewardReasons = new List<RewardReason>
                {
                    new RewardReason { Code = "A", RewardReasonID = 1}
                }.AsQueryable();
            var myFakeRewardStatus = new List<RewardStatus>
                {
                    new RewardStatus { Code = "P", RewardStatusID = 1}
                }.AsQueryable();
            var myFakeRewardTypes = new List<RewardType>
                {
                    new RewardType { Code = "A", RewardTypeID = 1}
                }.AsQueryable();

            var myFakeRewardReasonList = new List<RewardReason>
                {
                    new RewardReason { Code = "A", RewardReasonID = 1}
                };
            var myFakeRewardStatusList = new List<RewardStatus>
                {
                    new RewardStatus { Code = "P", RewardStatusID = 1}
                };
            var myFakeRewardTypesList = new List<RewardType>
                {
                    new RewardType { Code = "A", RewardTypeID = 1}
                };

            var dbSetMockRewardReason = new Mock<DbSet<RewardReason>>();
            dbSetMockRewardReason.As<IQueryable<RewardReason>>().Setup(m => m.Provider).Returns(myFakeRewardReasons.Provider);
            dbSetMockRewardReason.As<IQueryable<RewardReason>>().Setup(m => m.Expression).Returns(myFakeRewardReasons.Expression);
            dbSetMockRewardReason.As<IQueryable<RewardReason>>().Setup(m => m.ElementType).Returns(myFakeRewardReasons.ElementType);
            dbSetMockRewardReason.As<IQueryable<RewardReason>>().Setup(m => m.GetEnumerator()).Returns(myFakeRewardReasons.GetEnumerator());
            _context.Setup(x => x.RewardReasons).Returns(dbSetMockRewardReason.Object);
            _cacheContext.Setup(c => c.RewardReasons).Returns(myFakeRewardReasonList);


            var dbSetMockRewardStatus = new Mock<DbSet<RewardStatus>>();
            dbSetMockRewardStatus.As<IQueryable<RewardStatus>>().Setup(m => m.Provider).Returns(myFakeRewardStatus.Provider);
            dbSetMockRewardStatus.As<IQueryable<RewardStatus>>().Setup(m => m.Expression).Returns(myFakeRewardStatus.Expression);
            dbSetMockRewardStatus.As<IQueryable<RewardStatus>>().Setup(m => m.ElementType).Returns(myFakeRewardStatus.ElementType);
            dbSetMockRewardStatus.As<IQueryable<RewardStatus>>().Setup(m => m.GetEnumerator()).Returns(myFakeRewardStatus.GetEnumerator());
            _context.Setup(x => x.RewardStatus).Returns(dbSetMockRewardStatus.Object);
            _cacheContext.Setup(c => c.RewardStatuses).Returns(myFakeRewardStatusList);

            var dbSetMockRewardType = new Mock<DbSet<RewardType>>();
            dbSetMockRewardType.As<IQueryable<RewardType>>().Setup(m => m.Provider).Returns(myFakeRewardTypes.Provider);
            dbSetMockRewardType.As<IQueryable<RewardType>>().Setup(m => m.Expression).Returns(myFakeRewardTypes.Expression);
            dbSetMockRewardType.As<IQueryable<RewardType>>().Setup(m => m.ElementType).Returns(myFakeRewardTypes.ElementType);
            dbSetMockRewardType.As<IQueryable<RewardType>>().Setup(m => m.GetEnumerator()).Returns(myFakeRewardTypes.GetEnumerator());
            _context.Setup(x => x.RewardTypes).Returns(dbSetMockRewardType.Object);
            _cacheContext.Setup(c => c.RewardTypes).Returns(myFakeRewardTypesList);

            _contextService.Setup(c => c.GetContextProperties()).Returns(new ContextProperties { BaseSiteUrl = "http://localhost" });
            _configurationRepository.Setup(c => c.GetConfigurationValue<string>("TestEmailCopy")).Returns("test email address");

            service = new RewardRecognitionService(_context.Object, _rewardUserService.Object, _emailService.Object, _contextService.Object, _logggingService.Object, _configurationRepository.Object, _cacheContext.Object);
            return service;
        }

        [TestMethod]
        public async Task InsertReward_Cannot_Add_If_RewardType_Not_Exist_In_DbSet()
        {
            _rewardRecognitionService = SetupRewardRecognitionService();
            List<Reward> rewardsToAdd = new List<Reward>();
            rewardsToAdd.Add(new Reward()
            {
                Recipient = "jdoe",
                RewardTypeID = 999,
                RewardReasonID = 1,
                RewardStatusID = 1,
                CreatedBy = "ssmith"
            });

            var result = await _rewardRecognitionService.InsertRewardAsync(rewardsToAdd);
            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod]
        public async Task InsertReward_Cannot_Add_If_RewardReason_Not_Exist_In_DbSet()
        {
            _rewardRecognitionService = SetupRewardRecognitionService();
            List<Reward> rewardsToAdd = new List<Reward>();
            rewardsToAdd.Add(new Reward()
            {
                Recipient = "jdoe",
                RewardTypeID = 1,
                RewardReasonID = 999,
                RewardStatusID = 1,
                CreatedBy = "ssmith"
            });
                       
            var result = await _rewardRecognitionService.InsertRewardAsync(rewardsToAdd);
            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod]
        public async Task InsertReward_Cannot_Add_If_RewardStatus_Not_Exist_In_DbSet()
        {
            _rewardRecognitionService = SetupRewardRecognitionService();
            List<Reward> rewardsToAdd = new List<Reward>();
            rewardsToAdd.Add(new Reward()
            {
                Recipient = "jdoe",
                RewardTypeID = 1,
                RewardReasonID = 1,
                RewardStatusID = 999,
                CreatedBy = "ssmith"
            });

            var result = await _rewardRecognitionService.InsertRewardAsync(rewardsToAdd);
            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod]
        public async Task InsertReward_Cannot_Add_If_Recipient_SameAs_Sender()
        {
            _rewardRecognitionService = SetupRewardRecognitionService();
            List<Reward> rewardsToAdd = new List<Reward>();
            rewardsToAdd.Add(new Reward()
            {
                Recipient = "jdoe",
                RewardTypeID = 1,
                RewardReasonID = 1,
                RewardStatusID = 1,
                CreatedBy = "jdoe"
            });         


            var result = await _rewardRecognitionService.InsertRewardAsync(rewardsToAdd);
            Assert.AreEqual(result.Count, 0);           
        }

        [TestMethod]
        public async Task InsertReward_Add_NewRecord_If_Recipient_Not_SameAs_Sender_And_AllIds_In_DBsets()
        {
            _rewardRecognitionService = SetupRewardRecognitionService();
            List<Reward> rewardsToAdd = new List<Reward>();
            rewardsToAdd.Add(new Reward()
            {
                Recipient = "jdoe",
                RewardTypeID = 1,
                RewardReasonID = 1,
                RewardStatusID = 1,
                CreatedBy = "ssmith"
            });
                
            var result = await _rewardRecognitionService.InsertRewardAsync(rewardsToAdd);
            Assert.AreEqual(result.Count, 1);
        }
    }
}
