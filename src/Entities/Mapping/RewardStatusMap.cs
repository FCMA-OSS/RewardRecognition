using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;

namespace FCMA.RewardRecognition.Core.Entities.Mapping
{
    [ExcludeFromCodeCoverage]
    public class RewardStatusMap : EntityTypeConfiguration<RewardStatus>
    {
        public RewardStatusMap()
        {
            // Primary Key
            this.HasKey(t => t.RewardStatusID);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("RewardStatus");
            this.Property(t => t.RewardStatusID).HasColumnName("RewardStatusID");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
        }
    }
}
