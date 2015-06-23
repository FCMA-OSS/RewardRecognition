using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;

namespace FCMA.RewardRecognition.Core.Entities.Mapping
{
    [ExcludeFromCodeCoverage]
    public class RewardTypeMap : EntityTypeConfiguration<RewardType>
    {
        public RewardTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.RewardTypeID);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("RewardType");
            this.Property(t => t.RewardTypeID).HasColumnName("RewardTypeID");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.NeedApproval).HasColumnName("NeedApproval");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
        }
    }
}
