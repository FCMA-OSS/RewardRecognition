using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCMA.RewardRecognition.Infrastructure.ServiceContracts;
using Moq;
using System.Collections.Generic;
using FCMA.RewardRecognition.Core.Models;
using FCMA.RewardRecognition.Infrastructure.Services;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace FCMA.RewardRecognition.Tests.Services
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RewardUserServiceTest
    {
        private IActiveDirectoryUserService _activeDirectoryUserService;
        private IRewardUserService _rewardUserService;
        public RewardUserServiceTest()
        {
            var mockActiveDirectoryUserService = new Mock<IActiveDirectoryUserService>();
            mockActiveDirectoryUserService.Setup(s => s.GetAllActiveDirectoryUsers()).Returns(
                new List<RewardUserDataModel>(){
                new RewardUserDataModel{
                    UserName = "UN1",
                    UserFullName = "FN1 LN1",
                    EmailAddress = "UN1@DOMAIN.COM",
                    JobTitle = "CEO",
                    OfficeLocation = "MAIN",
                    Manager = new FullNameUserName(),
                    DirectReports = new List<FullNameUserName>{
                        new FullNameUserName{
                            FullName = "FN2 LN2",
                            UserName = "UN2"
                        },
                        new FullNameUserName{
                            FullName = "FN3 LN3",
                            UserName = "UN3"
                        }
                    }
                },
                new RewardUserDataModel{
                    UserName = "UN2",
                    UserFullName = "FN2 LN2",
                    EmailAddress = "UN2@DOMAIN.COM",
                    JobTitle = "VP1",
                    OfficeLocation = "SECONDARY",
                    Manager = new FullNameUserName{
                        FullName = "FN1 LN1",
                        UserName = "UN1"
                    },
                    DirectReports = new List<FullNameUserName>()
                    },
                    new RewardUserDataModel{
                    UserName = "UN3",
                    UserFullName = "FN3 LN3",
                    EmailAddress = "UN3@DOMAIN.COM",
                    JobTitle = "VP2",
                    OfficeLocation = "SECONDARY",
                    Manager = new FullNameUserName{
                        FullName = "FN1 LN1",
                        UserName = "UN1"
                    },
                    DirectReports = new List<FullNameUserName>{
                        new FullNameUserName{
                            FullName = "FN4 LN4",
                            UserName = "UN"
                        }
                    }
                    },
                    new RewardUserDataModel{
                    UserName = "UN4",
                    UserFullName = "FN4 LN4",
                    EmailAddress = "UN4@DOMAIN.COM",
                    JobTitle = "VP4",
                    OfficeLocation = "SECONDARY",
                    Manager = new FullNameUserName{
                        FullName = "FN3 LN3",
                        UserName = "UN3"
                    },
                    DirectReports = new List<FullNameUserName>()
                    }
                });
            _activeDirectoryUserService = mockActiveDirectoryUserService.Object;
            _rewardUserService = new RewardUserService(_activeDirectoryUserService);



        }
        #region ValidateRewardUserCacheState
        [TestMethod]
        public void ValidateRewardUserCacheState()
        {
            _rewardUserService.ValidateRewardUserCacheState();
            Assert.AreEqual(4, _rewardUserService.RewardDataUsers.Count);
        }

        #endregion


        #region GetEmailAddress
        [TestMethod]
        public void GetEmailAddress_Valid_UserName()
        {
            var userName = "UN2";
            var result = _rewardUserService.GetEmailAddress(userName);
            Assert.AreEqual("UN2@DOMAIN.COM", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetEmailAddress_InValid_UserName()
        {
            var userName = "UN_X";
            var result = _rewardUserService.GetEmailAddress(userName);

        }

        #endregion


        #region GetSupervisor
        [TestMethod]
        public void GetSupervisor_Valid_UserName()
        {
            var userName = "UN2";
            var result = _rewardUserService.GetSupervisor(userName);
            Assert.AreEqual("UN1", result.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetSupervisor_InValid_UserName()
        {
            var userName = "UN_X";
            var result = _rewardUserService.GetSupervisor(userName);

        }


        #endregion


        #region GetLocation
        [TestMethod]
        public void GetLocation_Valid_UserName()
        {
            var userName = "UN2";
            var result = _rewardUserService.GetLocation(userName);
            Assert.AreEqual("SECONDARY", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetLocation_InValid_UserName()
        {
            var userName = "UN_X";
            var result = _rewardUserService.GetLocation(userName);

        }


        #endregion

        #region RewardUsers
        [TestMethod]
        public void RewardUsers()
        {
            Assert.AreEqual(_rewardUserService.RewardUsers.Count(), _rewardUserService.RewardDataUsers.Count());
        }

        #endregion

        #region All level Reports  
        [TestMethod]
        public void Supervisor_Has_Multiple_Level_Reports_Should_Include_All_Level_Reports_Test()
        {
            Assert.AreEqual(_rewardUserService.GetAllLevelReports("UN1").Count, 4);
        }

        [TestMethod]
        public void Supervisor_Has_One_Level_Reports_Should_Include_Only_That_Level_Reports_Test()
        {
            Assert.AreEqual(_rewardUserService.GetAllLevelReports("UN3").Count, 2);
        }

        [TestMethod]
        public void Supervisor_Has_No_Direct_Reports_Should_Only_Include_Himself_Test()
        {
            Assert.AreEqual(_rewardUserService.GetAllLevelReports("UN2").Count, 1);
            Assert.AreEqual(_rewardUserService.GetAllLevelReports("UN4").Count, 1);
        }
        #endregion
    }
}
