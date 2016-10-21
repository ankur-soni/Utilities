using System.Configuration;
using System.IO;
using System.Reflection;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Entities.Initializer
{
    public static class BaseDatabaseInitializer
    {
        private static readonly string DropConnectionScript = "Silicus.Ensure.Entities.DatabaseScripts.DropConnection.sql";
        private static readonly string IndexScriptLocation = "Silicus.Ensure.Entities.DatabaseScripts.SqlIndexes.sql";
        private static readonly string DatabaseName = ConfigurationManager.AppSettings["DBName"];
        private static readonly string MembershipDatabaseName = ConfigurationManager.AppSettings["MembershipDBName"];

        public static void Seed(SilicusIpDataContext context)
        {
            //DropExistingConnectionToDatabase(context, DatabaseName);

            //AddIndexes(context, DatabaseName);
            context.Add(new User
           {
               FirstName = "Test",
               LastName = "Admin",
               NewPassword = "testadmin@123",
               ConfirmPassword = "testadmin@123",
               Address = "Test",
               Email = "testadmin@test.com",
               IdentityUserId = new System.Guid("2D3B545B-50F6-45D4-B916-23D276B3DB00"),
               Role = "ADMIN",
               isActive = true

           });
            context.Add(new User
            {
                FirstName = "Test",
                LastName = "User",
                NewPassword = "testuser@123",
                ConfirmPassword = "testuser@123",
                Address = "Test",
                Email = "testuser@test.com",
                IdentityUserId = new System.Guid("88B34980-EDD3-420C-8387-621B629A4E9B"),
                Role = "USER",
                isActive = true

            });
            context.Add(new Project
            {
                ProjectDescription = "Landmark"
            });

            context.Add(new Project
            {
                ProjectDescription = "MorningStar"
            });

            context.Add(new ProjectDetail
            {
                ProjectName = "Landmark",
                Status = "Red"
            });

            context.Add(new ProjectDetail
            {
                ProjectName = "MorningStar",
                Status = "Green"
            });

            context.Add(new Manager
            {
                ManagerName = "Shailendra"
            });

            context.Add(new Manager
            {
                ManagerName = "Sulekha"
            });
        }

        private static void AddIndexes(SilicusIpDataContext context, string databaseName)
        {
            context.Database.CreateIfNotExists();

            var sqlContent = Content(IndexScriptLocation);

            var modifiedSqlScript = sqlContent.Replace("@DatabaseName", databaseName);

            context.Database.ExecuteSqlCommand(modifiedSqlScript);
        }
        
        private static void DropExistingConnectionToDatabase(SilicusIpDataContext context, string databaseName)
        {
            var sqlContent = Content(DropConnectionScript);

            var modifiedSqlScript = sqlContent.Replace("@DatabaseName", databaseName);

            context.Database.ExecuteSqlCommand(modifiedSqlScript);
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
