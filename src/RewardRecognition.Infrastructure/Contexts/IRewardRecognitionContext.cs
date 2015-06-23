using FCMA.RewardRecognition.Core.Entities;
using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
namespace FCMA.RewardRecognition.Infrastructure.Contexts
{
    public interface IRewardRecognitionContext
    {
        DbSet<Reason> Reasons { get; set; }
        DbSet<Reward> Rewards { get; set; }
        DbSet<RewardType> RewardTypes { get; set; }
        DbSet<Status> Status { get; set; }


        int SaveChanges();

        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
