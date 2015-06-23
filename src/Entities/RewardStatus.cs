using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FCMA.RewardRecognition.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class RewardStatus
    {
        public RewardStatus()
        {
        }

        public int RewardStatusID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
