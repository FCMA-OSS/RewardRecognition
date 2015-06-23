using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Core.Models
{
    public class RewardUserDataModel : RewardUserModel
    {   
        public List<FullNameUserName> DirectReports { get; set; }

        public FullNameUserName Manager { get; set; }

        private bool isLeader;
        public bool IsLeader
        {
            get
            {
                return isLeader;
            }
            set 
            {
                isLeader = (value || this.DirectReports.Count > 0 || this.JobTitle.ToLower().Contains("director") || this.JobTitle.ToLower().Contains("president") || this.JobTitle.ToLower().Contains("vp"));
            }
        }


        public bool IsLeaderCalc()
        {
            return (this.DirectReports.Count > 0 || this.JobTitle.ToLower().Contains("director") || this.JobTitle.ToLower().Contains("president") || this.JobTitle.ToLower().Contains("vp"));
        }
    }
}
