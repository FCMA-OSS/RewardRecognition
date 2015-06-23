using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FCMA.RewardRecognition.Core.Entities;

namespace FCMA.RewardRecognition.Infrastructure.Contexts
{
    public partial class RewardRecognitionContext : DbContext, IRewardRecognitionContext
    {
        public RewardRecognitionContext()
            : base("name=RewardRecognition")
        {
        }

        public virtual DbSet<Reason> Reasons { get; set; }
        public virtual DbSet<Reward> Rewards { get; set; }
        public virtual DbSet<RewardType> RewardTypes { get; set; }
        public virtual DbSet<Status> Status { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reason>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Reason>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Reason>()
                .HasMany(e => e.Rewards)
                .WithRequired(e => e.Reason)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reward>()
                .Property(e => e.OtherDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Reward>()
                .Property(e => e.OtherValue)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Reward>()
                .Property(e => e.OtherReason)
                .IsUnicode(false);

            modelBuilder.Entity<RewardType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<RewardType>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<RewardType>()
                .Property(e => e.Amount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<RewardType>()
                .HasMany(e => e.Rewards)
                .WithRequired(e => e.RewardType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Status>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Status>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Status>()
                .HasMany(e => e.Rewards)
                .WithRequired(e => e.Status)
                .WillCascadeOnDelete(false);
        }


        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
