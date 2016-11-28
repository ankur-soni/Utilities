namespace Eda.RDBI.Entities
{
    using System;
    using System.Configuration;
    using Eda.RDBI.Models.DataObjects;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Reflection;

    public class RdbiIpCustomInitializer : DropCreateDatabaseIfModelChanges<RdbiIpDataContext>
    {
        private static readonly string SqlScriptLocation = "Eda.RDBI.Entities.DatabaseScripts.DbUserCreation.sql";
        private static readonly string IndexScriptLocation = "Eda.RDBI.Entities.DatabaseScripts.SqlIndexes.sql";
        private static readonly string DatabaseName = ConfigurationManager.AppSettings["DBName"];
        private static readonly string DBUser_UserName = ConfigurationManager.AppSettings["DBUser_UserName"];
        private static readonly string DBUser_Password = ConfigurationManager.AppSettings["DBUser_Password"];

        protected override void Seed(RdbiIpDataContext context)
        {
            CreateUser(context, DatabaseName, DBUser_UserName, DBUser_Password);
            AddIndexes(context, DatabaseName);

            context.Add(new Organization
            {
                Name = "EDA",
                SalesPersonName = "Brayn",
                Description = "Nice place to work",
                Email = "eda@edadata.com",
                TaxSchedule = "NC",
                CreatedDate = DateTime.UtcNow,
                GreatPlainsNumber = 123456,
                Addresses = new List<Address>
                {
                    new Address
                    {
                        ContactFirstName = "Sonny",
                        ContactLastName = "Rivera",
                        Country = "US",
                        State = "NC",
                        City = "Charlotte",
                        Street1 = "E Independence Road",
                        Zip = "21180",
                        BillingType = Models.BillingType.Shipping
                    },
                    new Address
                    {
                        ContactFirstName = "Swapna",
                        ContactLastName = "Girija",
                        Country = "US",
                        State = "NC",
                        City = "Charlotte",
                        Street1 = "E Independence Road",
                        Zip = "21180",
                        BillingType = Models.BillingType.Billing
                    }
                }
            });

            context.Add(new County { Id = 1, Name = "Albany" });
            context.Add(new County { Id = 2, Name = "Crook" });
            context.Add(new County { Id = 3, Name = "Fremont" });

            context.Add(new OrganizationCountyDataAccess { CountyId = 1, OrganizationId = 1 });
            context.Add(new OrganizationCountyDataAccess { CountyId = 2, OrganizationId = 1 });
            context.Add(new OrganizationCountyDataAccess { CountyId = 3, OrganizationId = 1 });

            context.Add(new EstimatedPowerUnit { Id = 1, Name = "100" });
            context.Add(new EstimatedPowerUnit { Id = 2, Name = "200" });
            context.Add(new EstimatedPowerUnit { Id = 3, Name = "300" });
            context.Add(new EstimatedPowerUnit { Id = 4, Name = "300 +" });

            context.Add(new OrganizationEstimatedPowerUnitDataAccess { EstimatedPowerUnitId = 1, OrganizationId = 1 });
            context.Add(new OrganizationEstimatedPowerUnitDataAccess { EstimatedPowerUnitId = 2, OrganizationId = 1 });
            context.Add(new OrganizationEstimatedPowerUnitDataAccess { EstimatedPowerUnitId = 3, OrganizationId = 1 });

            context.Add(new GvwrClass { Id = 1, Name = "Class3" });
            context.Add(new GvwrClass { Id = 2, Name = "Class8" });
            context.Add(new GvwrClass { Id = 3, Name = "Gliders" });

            context.Add(new OrganizationGvwrClassDataAccess { GvwrClassId = 1, OrganizationId = 1 });
            context.Add(new OrganizationGvwrClassDataAccess { GvwrClassId = 2, OrganizationId = 1 });
            context.Add(new OrganizationGvwrClassDataAccess { GvwrClassId = 3, OrganizationId = 1 });
        }

        private void AddIndexes(RdbiIpDataContext context, string databaseName)
        {
            context.Database.CreateIfNotExists();

            var sqlContent = Content(IndexScriptLocation);

            var modifiedSqlScript = sqlContent.Replace("@DatabaseName", databaseName);

            context.Database.ExecuteSqlCommand(modifiedSqlScript);
        }

        private void CreateUser(RdbiIpDataContext ctx, string databaseName, string logOnUserId, string logOnPassword)
        {
                ctx.Database.CreateIfNotExists();

                var sqlContent = Content(SqlScriptLocation);

                var modifiedSqlScript = sqlContent
                    .Replace("@DatabaseName", databaseName)
                    .Replace("@Login", logOnUserId)
                    .Replace("@Password", logOnPassword)
                    .Replace("@UserName", databaseName + "_User");

                ctx.Database.ExecuteSqlCommand(modifiedSqlScript);
        }

        private static string Content(string fileLocation)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileLocation))
            {
                if (stream == null)
                {
                    return string.Empty;
                }
                var streamReader = new StreamReader(stream);

                return streamReader.ReadToEnd();
            }
        }

    }
}
