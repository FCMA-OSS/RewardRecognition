using FCMA.RewardRecognition.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Core.Contexts.Migrations
{

    [ExcludeFromCodeCoverage]
    public sealed class RewardRecognitionConfiguration : DbMigrationsConfiguration<RewardRecognitionContext>
    {
        public RewardRecognitionConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(RewardRecognitionContext context)
        {
            context.RewardStatus.Add(new RewardStatus
            {
                Code = "S",
                Description = "Approved",
                IsActive = true
            });

            context.RewardReasons.Add(new RewardReason
            {
                Code = "1",
                Description = "Reason 1 Description",
                IsActive = true
            });

            context.RewardTypes.Add(new RewardType
            {
                Code = "1",
                Description = "50$ Gift Card",
                IsActive = true,
                NeedApproval = true
            });                      

            //context.Rewards.Add(new Reward
            //{
            //    RewardTypeID = 1,
            //    RewardReasonID = 1,
            //    RewardStatusID = 1,
            //    Recipient = "jdoe",
            //    CreatedBy = "mdoe",
            //    PresentationDate = Convert.ToDateTime("2015-03-18")
            //});

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
    
}
