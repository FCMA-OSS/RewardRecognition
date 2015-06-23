using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCMA.RewardRecognition.Infrastructure.ServiceContracts;
using FCMA.RewardRecognition.Infrastructure.Services;
using FCMA.RewardRecognition.Common.Configuration;
using System.DirectoryServices;
using System.Collections.Generic;
using FCMA.RewardRecognition.Core.Models;
using System.Data.Entity;
using System.Linq;
using System.Diagnostics.CodeAnalysis;


namespace FCMA.RewardRecognition.Tests.Services
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ActiveDirectoryUserServiceTest
    {
        private readonly IActiveDirectoryUserService _activeDirectoryUserService;


        public ActiveDirectoryUserServiceTest()
        {
            _activeDirectoryUserService = new ActiveDirectoryUserService(new ActiveDirectoryQueryContext(new ConfigFileConfigurationRepository()));

        }


        #region FullNameFormat

        [TestMethod]
        public void FullNameFormat_Parameter_Is_Null()
        {
            string input = null;
            var result = _activeDirectoryUserService.FullNameFormat(input);
            Assert.AreEqual(string.Empty, result);
        }
        [TestMethod]
        public void FullNameFormat_Parameter_Is_Empty()
        {
            var input = string.Empty;
            var result = _activeDirectoryUserService.FullNameFormat(input);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void FullNameFormat_Parameter_Is_Valid_Input()
        {
            var input = "CN=FN1 LN1,OU=Users,OU=Group1,OU=Group2,DC=Domain1,DC=Domain2,DC=Domain3";
            var result = _activeDirectoryUserService.FullNameFormat(input);
            Assert.AreEqual("FN1 LN1", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FullNameFormat_Parameter_Is_Not_Valid_Input_No_Equals()
        {
            var input = "CNFN1 LN1,OU=Users,OU=Group1,OU=Group2,DC=Domain1,DC=Domain2,DC=Domain3";
            var result = _activeDirectoryUserService.FullNameFormat(input);

        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FullNameFormat_Parameter_Is_Not_Valid_Input_No_Comma()
        {
            var input = "CN=FN1 LN1OU=UsersOU=Group1OU=Group2DC=Domain1DC=Domain2DC=Domain3";
            var result = _activeDirectoryUserService.FullNameFormat(input);

        }

        #endregion


        #region UserNameFormat
        [TestMethod]
        public void UserNameFormat_With_At_Sign_Delimeter()
        {
            var input = "USERNAME@DOMAIN.COM";
            var result = _activeDirectoryUserService.UserNameFormat(input);
            Assert.AreEqual("username", result);

        }
        [TestMethod]
        public void UserNameFormat_No_At_Sign_Delimeter()
        {
            var input = "USERNAME";
            var result = _activeDirectoryUserService.UserNameFormat(input);
            Assert.AreEqual("username", result);

        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UserNameFormat_Exception_Empty_Argument()
        {
            var input = string.Empty;
            var result = _activeDirectoryUserService.UserNameFormat(input);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UserNameFormat_Exception_Null_Argument()
        {
            string input = null;
            var result = _activeDirectoryUserService.UserNameFormat(input);
        }


        #endregion

        #region DirectReportsFormat
        [TestMethod]
        public void DirectReportsFormat()
        {
            var list = new List<string>{
                "CN=FN1 LN1,OU=Users,OU=Group1,OU=Group2,DC=Domain1,DC=Domain2,DC=Domain3",
                "CN=FN2 LN2,OU=Users,OU=Group1,OU=Group2,DC=Domain1,DC=Domain2,DC=Domain3",
                "CN=FN3 LN3,OU=Users,OU=Group1,OU=Group2,DC=Domain1,DC=Domain2,DC=Domain3",
                "CN=FN4 LN4,OU=Users,OU=Group1,OU=Group2,DC=Domain1,DC=Domain2,DC=Domain3",
                "CN=FN5 LN5,OU=Users,OU=Group1,OU=Group2,DC=Domain1,DC=Domain2,DC=Domain3",
            };


            var fullNameUserNameList = _activeDirectoryUserService.DirectReportsFormat(list);
            Assert.AreEqual(5, fullNameUserNameList.Count);
            Assert.AreEqual("FN1 LN1", fullNameUserNameList[0].FullName);
            Assert.AreEqual("FN2 LN2", fullNameUserNameList[1].FullName);
            Assert.AreEqual("FN3 LN3", fullNameUserNameList[2].FullName);
            Assert.AreEqual("FN4 LN4", fullNameUserNameList[3].FullName);
            Assert.AreEqual("FN5 LN5", fullNameUserNameList[4].FullName);

            Assert.AreEqual(string.Empty, fullNameUserNameList[0].UserName);
            Assert.AreEqual(string.Empty, fullNameUserNameList[1].UserName);
            Assert.AreEqual(string.Empty, fullNameUserNameList[2].UserName);
            Assert.AreEqual(string.Empty, fullNameUserNameList[3].UserName);
            Assert.AreEqual(string.Empty, fullNameUserNameList[4].UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DirectReportsFormat_Invalid_Input_Format_Missing_Equals()
        {
            var list = new List<string>{
                "CNFN1 LN1,OU=Users,OU=Group1,OU=Group2,DC=Domain1,DC=Domain2,DC=Domain3",
            };


            var fullNameUserNameList = _activeDirectoryUserService.DirectReportsFormat(list);

        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DirectReportsFormat_Invalid_Input_Format_Missing_Comma()
        {
            var list = new List<string>{
                "CNFN1 LN1OU=UsersOU=Group1OU=Group2DC=Domain1DC=Domain2DC=Domain3",
            };


            var fullNameUserNameList = _activeDirectoryUserService.DirectReportsFormat(list);

        }
        #endregion


        #region ResolveManagerUserNames

        [TestMethod]
        public void ResolveManagerUserNames()
        {
            var usersList = new List<RewardUserDataModel>(){
                new RewardUserDataModel{
                    UserName = "UN1",
                    UserFullName = "FN1 LN1",
                    Manager = new FullNameUserName(),
                    DirectReports = new List<FullNameUserName>{
                        new FullNameUserName{
                            FullName = "FN2 LN2"
                        },
                        new FullNameUserName{
                            FullName = "FN3 LN3"
                        }
                    }
                },
                new RewardUserDataModel{
                    UserName = "UN2",
                    UserFullName = "FN2 LN2",
                    Manager = new FullNameUserName{
                        FullName = "FN1 LN1"
                    },
                    DirectReports = new List<FullNameUserName>()
                    },
                    new RewardUserDataModel{
                    UserName = "UN3",
                    UserFullName = "FN3 LN3",
                    Manager = new FullNameUserName{
                        FullName = "FN1 LN1"
                    },
                    DirectReports = new List<FullNameUserName>()
                    }
                };
            _activeDirectoryUserService.ResolveManagerUserNames(usersList);

            var users = usersList.Where(w => w.Manager.UserName == "UN1").ToList();

            Assert.AreEqual(2, users.Count());
            Assert.AreEqual("FN2 LN2", users[0].UserFullName);
            Assert.AreEqual("FN3 LN3", users[1].UserFullName);

        }

        #endregion


        #region ResolveDirectReports

        [TestMethod]
        public void ResolveDirectReports()
        {
            var usersList = new List<RewardUserDataModel>(){
                new RewardUserDataModel{
                    UserName = "UN1",
                    UserFullName = "FN1 LN1",
                    Manager = new FullNameUserName(),
                    DirectReports = new List<FullNameUserName>{
                        new FullNameUserName{
                            FullName = "FN2 LN2"
                        },
                        new FullNameUserName{
                            FullName = "FN3 LN3"
                        }
                    }
                },
                new RewardUserDataModel{
                    UserName = "UN2",
                    UserFullName = "FN2 LN2",
                    Manager = new FullNameUserName{
                        FullName = "FN1 LN1",
                        UserName = "UN1"
                    },
                    DirectReports = new List<FullNameUserName>()
                    },
                    new RewardUserDataModel{
                    UserName = "UN3",
                    UserFullName = "FN3 LN3",
                    Manager = new FullNameUserName{
                        FullName = "FN1 LN1",
                        UserName = "UN1"
                    },
                    DirectReports = new List<FullNameUserName>()
                    }
                };
            _activeDirectoryUserService.ResolveDirectReports(usersList);

            var userDirectReports = usersList.Single(w => w.UserName == "UN1").DirectReports;

            Assert.AreEqual("UN2", userDirectReports[0].UserName);
            Assert.AreEqual("UN3", userDirectReports[1].UserName);

        }


        #endregion



    }



}

