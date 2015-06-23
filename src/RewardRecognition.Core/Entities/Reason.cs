using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FCMA.RewardRecognition.Core.Entities
{

    [Table("Reason")]
    public partial class Reason
    {
        public Reason()
        {
            Rewards = new HashSet<Reward>();
        }

        public int ReasonID { get; set; }

        [Required]
        [StringLength(1)]
        public string Code { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public bool Archive { get; set; }

        public virtual ICollection<Reward> Rewards { get; set; }
    }
}
