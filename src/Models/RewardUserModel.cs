using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Core.Models
{
    public class RewardUserModel
    {

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string UserFullName { get; set; }

        public string OfficeLocation { get; set; }

        public string JobTitle { get; set; }

        public bool IsInRedeemGroup { get; set; }

    }
}
