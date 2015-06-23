using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FCMA.RewardRecognition.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class RewardType
    {
        public RewardType()
        {
        }

        public int RewardTypeID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public bool NeedApproval { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
