using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FCMA.RewardRecognition.Core.Entities
{

    [Table("RewardType")]
    public partial class RewardType
    {
        public RewardType()
        {
            Rewards = new HashSet<Reward>();
        }

        public int RewardTypeID { get; set; }

        [Required]
        [StringLength(1)]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public decimal? Amount { get; set; }

        public bool NeedApproval { get; set; }

        public bool Archive { get; set; }

        public virtual ICollection<Reward> Rewards { get; set; }
    }
}
