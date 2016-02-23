using System;
using System.Configuration;
using System.Data.Entity.Migrations;

namespace TCWAdminPortalWeb
{
    public class MigrationsConfig
    {
        public static void RunMigrationsOnStartup()
        {
            if (bool.Parse(ConfigurationManager.AppSettings["MigrateDatabaseToLatestVersion"]))
            {
                var configuration = new Migrations.Configuration();
                var migrator = new DbMigrator(configuration);
                migrator.Update();
            }
        }
    }
}