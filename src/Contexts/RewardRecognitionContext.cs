using FCMA.RewardRecognition.Core.Entities;
using FCMA.RewardRecognition.Core.Entities.Mapping;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace FCMA.RewardRecognition.Core.Contexts
{
    public class RewardRecognitionContext : DbContext, FCMA.RewardRecognition.Core.Contexts.IRewardRecognitionContext
    {
        static RewardRecognitionContext()
        {
            Database.SetInitializer<RewardRecognitionContext>(null);
        }

        public RewardRecognitionContext()
            : base("Name=RewardRecognitionContext")
        {
        }

        public DbSet<Reward> Rewards { get; set; }
        public DbSet<RewardReason> RewardReasons { get; set; }
        public DbSet<RewardStatus> RewardStatus { get; set; }
        public DbSet<RewardType> RewardTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RewardMap());
            modelBuilder.Configurations.Add(new RewardReasonMap());
            modelBuilder.Configurations.Add(new RewardStatusMap());
            modelBuilder.Configurations.Add(new RewardTypeMap());
        }
    }
}
