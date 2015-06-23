using FCMA.RewardRecognition.Common.Configuration;
using FCMA.RewardRecognition.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Common.Email
{
    public class SystemNetEmailService : IEmailService
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ILoggingService _loggingService;

        public SystemNetEmailService(IConfigurationRepository configurationRepository, ILoggingService loggingService)
        {
            if (configurationRepository == null) throw new ArgumentNullException("ConfigurationRepository");
            if (loggingService == null) throw new ArgumentNullException("LoggingService");
            _configurationRepository = configurationRepository;
            _loggingService = loggingService;
        }

        public EmailSendingResult SendEmail(EmailArguments emailArguments)
        {
            EmailSendingResult sendResult = new EmailSendingResult();
            sendResult.EmailSendingFailureMessage = string.Empty;
            SmtpClient client = null;
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailArguments.From);
                if (emailArguments.To.Count > 0)
                {
                    foreach (string toAddress in emailArguments.To)
                    {
                        if (toAddress.Contains(";"))
                        {

                            foreach (string sTo in toAddress.Split(';'))
                            {
                                mailMessage.To.Add(new MailAddress(sTo));
                            }
                        }
                        else
                        {
                            mailMessage.To.Add(new MailAddress(toAddress));
                        }
                        
                    }
                }
                mailMessage.Subject = emailArguments.Subject;
                mailMessage.Body = emailArguments.Message;
                mailMessage.IsBodyHtml = emailArguments.IsBodyHtml;
                if (emailArguments.CC.Count > 0)
                {
                    foreach (string ccAddress in emailArguments.CC)
                    {
                        if (ccAddress.Contains(";"))
                        {
                            
                            foreach (string sCC in ccAddress.Split(';'))
                            {
                                mailMessage.CC.Add(new MailAddress(sCC));
                            }
                        }
                        else
                        {
                            mailMessage.CC.Add(new MailAddress(ccAddress));
                        }
                       
                    }
                }
                client = new SmtpClient(GetSmtpServerFromConfigIfItIsNotInEmailArguments(emailArguments));

                if (emailArguments.EmbeddedResources != null && emailArguments.EmbeddedResources.Count > 0)
                {
                    AlternateView alternateViewHtml = AlternateView.CreateAlternateViewFromString(emailArguments.Message, Encoding.UTF8, MediaTypeNames.Text.Html);
                    foreach (EmbeddedEmailResource resource in emailArguments.EmbeddedResources)
                    {
                        LinkedResource linkedResource = new LinkedResource(resource.ResourceStream, resource.ResourceType.ToSystemNetResourceType());
                        linkedResource.ContentId = resource.EmbeddedResourceContentId;
                        alternateViewHtml.LinkedResources.Add(linkedResource);
                    }
                    mailMessage.AlternateViews.Add(alternateViewHtml);
                }

                if (mailMessage.IsBodyHtml)
                {
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailMessage.Body, new ContentType("text/html"));

                    mailMessage.AlternateViews.Add(htmlView);

                }

                _loggingService.LogInfo(this, "Starting sending email to : " + emailArguments.To);
                client.Send(mailMessage);
                sendResult.EmailSentSuccessfully = true;
                _loggingService.LogInfo(this, "Successfully sent email: " + DisplayEmailInfo(emailArguments));
            }
            catch (Exception ex)
            {
                sendResult.EmailSentSuccessfully = false;
                sendResult.EmailSendingFailureMessage = ex.Message;
                _loggingService.LogError(this, "Exception in sending email: " + DisplayEmailInfo(emailArguments), ex);
            }

            return sendResult;
        }

        private string GetSmtpServerFromConfigIfItIsNotInEmailArguments(EmailArguments emailArguments)
        {
            string smtpServer = string.Empty;
            if (!String.IsNullOrEmpty(emailArguments.SmtpServer))
            {
                smtpServer = emailArguments.SmtpServer;
            }
            else
            {
                smtpServer = _configurationRepository.GetConfigurationValue<string>("SmtpServer");
            }
            return smtpServer;
        }

        private string DisplayEmailInfo(EmailArguments emailArguments)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("From: ");
            stringBuilder.AppendLine(emailArguments.From);
            stringBuilder.Append("To: ");
            stringBuilder.AppendLine(string.Join(";", emailArguments.To));
            if (emailArguments.CC.Count > 0)
            {
                stringBuilder.Append("CC: ");
                stringBuilder.AppendLine(string.Join(";", emailArguments.CC));
            }
            stringBuilder.Append("Submect: ");
            stringBuilder.AppendLine(emailArguments.Subject);
            stringBuilder.Append("SmtpServer: ");
            stringBuilder.AppendLine(emailArguments.SmtpServer);
            stringBuilder.Append("Body: ");
            stringBuilder.AppendLine(emailArguments.Message);

            return stringBuilder.ToString();
        }
    }
}
