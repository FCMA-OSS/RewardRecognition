using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FCMA.RewardRecognition.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class Reward
    {
        public int RewardID { get; set; }
        public int RewardTypeID { get; set; }
        public int RewardReasonID { get; set; }
        public string OtherReason { get; set; }
        public string Recipient { get; set; }
        public string RecipientFullName { get; set; }
        public string Supervisor { get; set; }
        public int RewardStatusID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByFullName { get; set; }
        public Nullable<System.DateTime> LastChangedDate { get; set; }
        public string ChangedBy { get; set; }
        public Nullable<System.DateTime> RedeemedDate { get; set; }
        public string RedeemedBy { get; set; }
        public System.DateTime PresentationDate { get; set; }
        public virtual RewardReason RewardReason { get; set; }
        public virtual RewardStatus RewardStatus { get; set; }
        public virtual RewardType RewardType { get; set; }
    }
}
