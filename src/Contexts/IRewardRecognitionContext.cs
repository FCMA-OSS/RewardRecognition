using FCMA.RewardRecognition.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Core.Contexts
{
    public interface IRewardRecognitionContext : IObjectContextAdapter
    {
        DbSet<RewardReason> RewardReasons { get; set; }
        DbSet<Reward> Rewards { get; set; }
        DbSet<RewardStatus> RewardStatus { get; set; }
        DbSet<RewardType> RewardTypes { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IEnumerable<DbEntityValidationResult> GetValidationErrors();

        DbEntityEntry Entry(object entity);
        void Dispose();
        string ToString();
        bool Equals(object obj);
        int GetHashCode();
        Type GetType();
    }
}
