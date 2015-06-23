using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FCMA.RewardRecognition.Core.Entities
{

    public partial class Status
    {
        public Status()
        {
            Rewards = new HashSet<Reward>();
        }

        public int StatusID { get; set; }

        [Required]
        [StringLength(1)]
        public string Code { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        public bool Archive { get; set; }

        public virtual ICollection<Reward> Rewards { get; set; }
    }
}
