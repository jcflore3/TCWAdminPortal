using Microsoft.WindowsAzure.ServiceRuntime;
using System.Data.Entity.Migrations;

namespace TCWAdminPortalWeb
{
    public class MigrationsConfig
    {
        public static void RunMigrationsOnStartup()
        {
            if (bool.Parse(RoleEnvironment.GetConfigurationSettingValue("MigrateDatabaseToLatestVersion")))
            {
                var configuration = new Migrations.Configuration();
                var migrator = new DbMigrator(configuration);
                migrator.Update();
            }
        }
    }
}