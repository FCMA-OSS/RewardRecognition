using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;

namespace FCMA.RewardRecognition.Core.Entities.Mapping
{
    [ExcludeFromCodeCoverage]
    public class RewardReasonMap : EntityTypeConfiguration<RewardReason>
    {
        public RewardReasonMap()
        {
            // Primary Key
            this.HasKey(t => t.RewardReasonID);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("RewardReason");
            this.Property(t => t.RewardReasonID).HasColumnName("RewardReasonID");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
        }
    }
}
