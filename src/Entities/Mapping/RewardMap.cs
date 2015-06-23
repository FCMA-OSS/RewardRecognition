using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;

namespace FCMA.RewardRecognition.Core.Entities.Mapping
{
    [ExcludeFromCodeCoverage]
    public class RewardMap : EntityTypeConfiguration<Reward>
    {
        public RewardMap()
        {
            // Primary Key
            this.HasKey(t => t.RewardID);

            // Properties
           
            this.Property(t => t.OtherReason)
                .HasMaxLength(4000);

            this.Property(t => t.Recipient)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Supervisor)
                .HasMaxLength(10);

            this.Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.ChangedBy)
                .HasMaxLength(10);

            this.Property(t => t.RedeemedBy)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Reward");
            this.Property(t => t.RewardID).HasColumnName("RewardID");
            this.Property(t => t.RewardTypeID).HasColumnName("RewardTypeID");
            this.Property(t => t.RewardReasonID).HasColumnName("RewardReasonID");
            this.Property(t => t.OtherReason).HasColumnName("OtherReason");
            this.Property(t => t.Recipient).HasColumnName("Recipient");
            this.Property(t => t.Supervisor).HasColumnName("Supervisor");
            this.Property(t => t.RewardStatusID).HasColumnName("RewardStatusID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastChangedDate).HasColumnName("LastChangedDate");
            this.Property(t => t.ChangedBy).HasColumnName("ChangedBy");
            this.Property(t => t.RedeemedDate).HasColumnName("RedeemedDate");
            this.Property(t => t.RedeemedBy).HasColumnName("RedeemedBy");
            this.Property(t => t.PresentationDate).HasColumnName("PresentationDate");

            //Relationships
            this.HasRequired(t => t.RewardReason);
                //.WithMany(t => t.Rewards)
                //.HasForeignKey(d => d.RewardReasonID);
            this.HasRequired(t => t.RewardStatus);
                //.WithMany(t => t.Rewards)
                //.HasForeignKey(d => d.RewardStatusID);
            this.HasRequired(t => t.RewardType);
                //.WithMany(t => t.Rewards)
                //.HasForeignKey(d => d.RewardTypeID);

        }
    }
}
