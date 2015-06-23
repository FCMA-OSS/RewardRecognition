using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using FCMA.RewardRecognition.Common.Email;
using FCMA.RewardRecognition.Common.Configuration;
using FCMA.RewardRecognition.Common.Logging;
using FCMA.RewardRecognition.Common.ContextProvider;
using Moq;
using System.Collections.Generic;

namespace FCMA.RewardRecognition.Tests.Common
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SystemNetEmailServiceTest
    {
        private readonly IEmailService _emailService;

        public SystemNetEmailServiceTest() 
        {
            var _configurationRepository = new Mock<IConfigurationRepository>();
            _configurationRepository.Setup(c => c.GetConfigurationValue<string>("SmtpServer")).Returns("mySmtpServer");
            var _logggingService = new Mock<ILoggingService>();
            //_contextService = new Mock<IContextService>();

            _emailService = new SystemNetEmailService(_configurationRepository.Object, _logggingService.Object);            
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Email subject cannot be null or empty")]
        public void Null_Email_Subject_Throws_Exception()
        {
            List<string> to = new List<string>();
            to.Add("to");
            List<string> cc = new List<string>();
            EmailArguments emailArguments = new EmailArguments(null, "message", to, "from", null, false, cc);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Email message cannot be null or empty")]
        public void Null_Email_Message_Throws_Exception()
        {
            List<string> to = new List<string>();
            to.Add("to");
            List<string> cc = new List<string>();
            EmailArguments emailArguments = new EmailArguments("subject", "", to, "from", null, false, cc);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Email recipient cannot be null or empty")]
        public void Null_Email_To_Throws_Exception()
        {
            List<string> to = new List<string>();
            List<string> cc = new List<string>();
            EmailArguments emailArguments = new EmailArguments("subject", "message", to, "from", null, false, cc);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Email sender cannot be null or empty")]
        public void Null_Email_From_Throws_Exception()
        {
            List<string> to = new List<string>();
            to.Add("to");
            List<string> cc = new List<string>();
            EmailArguments emailArguments = new EmailArguments("subject", "message", to, "", null, false, cc);
        }

        [TestMethod]
        public void Email_Arguments_Can_Have_Null_SmtpServer()
        {
            List<string> to = new List<string>();
            to.Add("to");
            List<string> cc = new List<string>();
            EmailArguments emailArguments = new EmailArguments("subject", "message", to, "From", null, false, cc);
            Assert.IsNotNull(emailArguments);
        }

        [TestMethod]
        public void SystemNetEmailService_Will_Get_SmtpServer_From_Config_If_Client_Not_Provide_Through_EmailArguments()
        {
            List<string> to = new List<string>();
            to.Add("to");
            List<string> cc = new List<string>();
            EmailArguments emailArguments = new EmailArguments("subject", "message", to, "From", null, false, cc);           
            SystemNetEmailService emailService = (SystemNetEmailService)_emailService;
            PrivateObject obj = new PrivateObject(emailService);
            var retVal = obj.Invoke("GetSmtpServerFromConfigIfItIsNotInEmailArguments", emailArguments);
            Assert.AreEqual("mySmtpServer", retVal);
        }
    }
}
