using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Common.Email
{
    public class EmailArguments
    {
        private string _subject;
        private string _message;
        private List<string> _to;
        private string _from;
        private string _smtpServer;
        private List<string> _cc;
        private bool _isBodyHtml;

        public EmailArguments(string subject, string message, List<string> to, string from, string smtpServer, bool isBodyHtml, List<string> cc)
        {
            if (string.IsNullOrEmpty(subject))
                throw new ArgumentNullException("Email subject cannot be null or empty");
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException("Email message cannot be null or empty");
            if (to.Count < 1)
                throw new ArgumentNullException("Email recipient cannot be null or empty");
            if (string.IsNullOrEmpty(from))
                throw new ArgumentNullException("Email sender cannot be null or empty");
            this._from = from;
            this._message = message;
            this._smtpServer = smtpServer;
            this._subject = subject;
            this._to = to;
            this._isBodyHtml = isBodyHtml;
            this._cc = cc;
        }

        public List<EmbeddedEmailResource> EmbeddedResources { get; set; }

        public List<string> To
        {
            get
            {
                return this._to;
            }
        }

        public string From
        {
            get
            {
                return this._from;
            }
        }

        public string Subject
        {
            get
            {
                return this._subject;
            }
        }

        public string SmtpServer
        {
            get
            {
                return this._smtpServer;
            }
        }

        public string Message
        {
            get
            {
                return this._message;
            }
        }

        public bool IsBodyHtml
        {
            get
            {
                return this._isBodyHtml;
            }
        }

        public List<string> CC
        {
            get
            {
                return this._cc;
            }
        }
    }
}
