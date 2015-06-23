using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FCMA.RewardRecognition.Core.Entities
{

    [Table("Reward")]
    public partial class Reward
    {
        public Reward()
        {
        }

        public int RewardID { get; set; }

        public int RewardTypeID { get; set; }

        [StringLength(255)]
        public string OtherDescription { get; set; }

        public decimal? OtherValue { get; set; }

        public int ReasonID { get; set; }

        [StringLength(4000)]
        public string OtherReason { get; set; }

        public int RecipientID { get; set; }

        public int? SupervisorID { get; set; }

        public int StatusID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int CreatedByID { get; set; }

        public DateTime? LastChangedDate { get; set; }

        public int? ChangedByID { get; set; }

        public DateTime? RedeemedDate { get; set; }

        public int? RedeemedByID { get; set; }

        public DateTime PresentationDate { get; set; }

        public virtual Reason Reason { get; set; }

        public virtual RewardType RewardType { get; set; }

        public virtual Status Status { get; set; }

    }
}
