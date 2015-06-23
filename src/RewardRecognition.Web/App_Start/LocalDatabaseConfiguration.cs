using System.Configuration;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using FCMA.RewardRecognition.Core.Contexts.Migrations;

namespace FCMA.RewardRecognition.Web
{
    public static class LocalDatabaseConfiguration
    {
        
            public static void RegisterDatabases()
            {
                

                if (!bool.Parse(ConfigurationManager.AppSettings["CreateLocalDatabase"])) return;
                GetValue(new DbMigrator(new RewardRecognitionConfiguration()));
            }

            private static void GetValue(MigratorBase migrator)
            {
                migrator.Update();
                
            }
        }
    
}