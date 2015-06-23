using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Core.Models
{
    public class PendingReward
    {
        public int RewardId { get; set; }
        public string Recipient { get; set; }

        public string Gift { get; set; }

        public string Reason { get; set; }

        public string Giver { get; set; }

    }
}
