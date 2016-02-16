namespace TCWAdminPortalWeb.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TCWAdminPortalWeb.Models.TCWAdminContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Seeds the database tables with data if there is no data present in the table
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(TCWAdminPortalWeb.Models.TCWAdminContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate()

            // Seed Featured Property Data
            if (!context.FeaturedProperties.Any())
            {
                //if there are no featured properties in the database then add the following ones
                var rodaleFeaturedProp = new FeaturedProperty()
                {
                    Title = "Rodale House",
                    Created = DateTime.UtcNow,
                    StreetAddress = "16102 Rodale Dr",
                    City = "Houston",
                    State = "TX",
                    Zip = "77049",
                    BedRooms = 4,
                    FullBaths = 2,
                    HalfBaths = 3,
                    BuildingSQFeet = 3230,
                    LotSQFeet = 23347,
                    Garage = 2
                };
                context.FeaturedProperties.Add(rodaleFeaturedProp);

                var dillonFeaturedProp = new FeaturedProperty()
                {
                    Title = "Dillon House",
                    Created = DateTime.UtcNow,
                    StreetAddress = "7020 Dillon St",
                    City = "Houston",
                    State = "TX",
                    Zip = "77061",
                    BedRooms = 4,
                    FullBaths = 2,
                    HalfBaths = 0,
                    BuildingSQFeet = 2065,
                    LotSQFeet = 26000,
                    Garage = 0
                };
                context.FeaturedProperties.Add(dillonFeaturedProp);

                //save changes to db
                context.SaveChanges();
            }

            // Seed Agent Data
            if (!context.Agents.Any())
            {
                //no agent records in database so, create some
                var stephAgent = new Agent()
                {
                    Name = "Stephanie Gallegos",
                    Created = DateTime.UtcNow,
                    Location = "Houston",
                    Description = "Stepanie Gallegos is a Real Estate Broker and Real Estate Agent with 10 years experience",
                    Enabled = true
                };
                context.Agents.Add(stephAgent);

                //no agent records in database so, create some
                var fakeAgent1 = new Agent()
                {
                    Name = "John Doe",
                    Created = DateTime.UtcNow,
                    Location = "San Antonio",
                    Description = "John Doe has been serving the San Antonio area, as a Real Estate agent for over 5 years",
                    Enabled = true
                };
                context.Agents.Add(fakeAgent1);

                //no agent records in database so, create some
                var fakeAgent2 = new Agent()
                {
                    Name = "Jane Doe",
                    Created = DateTime.UtcNow,
                    Location = "Austin",
                    Description = "Jane Doe has been serving the Austin area for over 15 years as a real estate agent",
                    Enabled = true
                };
                context.Agents.Add(fakeAgent2);

                context.SaveChanges();
            }

            // Seed Contact Information Data
            if (!context.ContactInfos.Any())
            {
                // no contact information record so create one
                var contactInfoRec = new ContactInfo()
                {
                    StreetAddress = "5203 Telephone Rd.",
                    Created = DateTime.UtcNow,
                    City = "Houston",
                    State = "TX",
                    Zip = "77087",
                    Email = "steph@texascrosswayrealty.com",
                    Phone = "8328888247"
                };
                context.ContactInfos.Add(contactInfoRec);
                context.SaveChanges();
            }
        }
    }
}
