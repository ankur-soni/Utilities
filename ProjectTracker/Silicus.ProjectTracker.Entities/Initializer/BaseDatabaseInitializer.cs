using System.Configuration;
using System.IO;
using System.Reflection;
using Silicus.ProjectTracker.Models.DataObjects;
using System;
using System.Web;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Entities.Initializer
{
    public static class BaseDatabaseInitializer
    {
        private static readonly string DropConnectionScript = "Silicus.ProjectTracker.Entities.DatabaseScripts.DropConnection.sql";
        private static readonly string IndexScriptSeedMembershipLocation = "Silicus.ProjectTracker.Entities.DatabaseScripts.SeedMembershipData.sql";
        private static readonly string UniqueConstraintScript = "Silicus.ProjectTracker.Entities.DatabaseScripts.UniqueConstraints.sql";
        private static readonly string DatabaseName = ConfigurationManager.AppSettings["DBName"];

        public static void Seed(ProjectTrackerIpDataContext context)
        {
            AddConstraints(context, DatabaseName);
            SeedMembershipData(context, DatabaseName);

            AddDefaultProject(context);
            AddDefaultStatus(context);

            AddDefaultWeekData(context);
            AddProjectMapping(context);
            AddProjecStatusData(context);
            AddProjectResouceUtilizationData(context);
        }

        private static void AddDefaultProject(ProjectTrackerIpDataContext context)
        {
            context.Add(new Project
            {
                ProjectName = "SMART",
                ProjectDescription = "Online Attendance System",
                StartDate = DateTime.Now.AddMonths(-5),
                PlannedEndDate = DateTime.Now.AddMonths(5),
                IsActive = false,
                CreatedBy = "SHaldar",
                CreatedDate = DateTime.Now.AddMonths(-5),
                ModifiedBy = "SBirthare",
                ModifiedDate = DateTime.Now.AddMonths(-5)
            });

            context.Add(new Project
            {
                ProjectName = "RDBI",
                ProjectDescription = "RigDig Business Intelligence",
                StartDate = DateTime.Now.AddMonths(-4),
                PlannedEndDate = DateTime.Now.AddMonths(4),
                IsActive = true,
                CreatedBy = "SBirthare",
                CreatedDate = DateTime.Now.AddMonths(-4),
                ModifiedBy = "SRoy",
                ModifiedDate = DateTime.Now.AddMonths(-4)
            });

            context.Add(new Project
            {
                ProjectName = "Project Tracker",
                ProjectDescription = "Project management System",
                StartDate = DateTime.Now.AddMonths(-3),
                PlannedEndDate = DateTime.Now.AddMonths(3),
                IsActive = true,
                CreatedBy = "SBirthare",
                CreatedDate = DateTime.Now.AddMonths(-3),
                ModifiedBy = "SRoy",
                ModifiedDate = DateTime.Now.AddMonths(-3)
            });

            context.Add(new Project
            {
                ProjectName = "LandMark",
                ProjectDescription = "Geological System",
                StartDate = DateTime.Now.AddMonths(-2),
                PlannedEndDate = DateTime.Now.AddMonths(3),
                IsActive = true,
                CreatedBy = "SHaldar",
                CreatedDate = DateTime.Now.AddMonths(-2),
                ModifiedBy = "SBirthare",
                ModifiedDate = DateTime.Now.AddMonths(-2)
            });

            context.Add(new Project
            {
                ProjectName = "Entrac",
                ProjectDescription = "Internal System",
                StartDate = DateTime.Now.AddMonths(-1),
                PlannedEndDate = DateTime.Now.AddMonths(2),
                IsActive = true,
                CreatedBy = "SHaldar",
                CreatedDate = DateTime.Now.AddMonths(-1),
                ModifiedBy = "SBirthare",
                ModifiedDate = DateTime.Now.AddMonths(-1)
            });

            context.Add(new Project
            {
                ProjectName = "Morning Star",
                ProjectDescription = "Internal System",
                StartDate = DateTime.Now.AddMonths(-8),
                PlannedEndDate = DateTime.Now.AddMonths(8),
                IsActive = true,
                CreatedBy = "SHaldar",
                CreatedDate = DateTime.Now.AddMonths(-8),
                ModifiedBy = "SBirthare",
                ModifiedDate = DateTime.Now.AddMonths(-8)
            });

            context.Add(new Project
            {
                ProjectName = "Apache",
                ProjectDescription = "ERP System",
                StartDate = DateTime.Now.AddMonths(-9),
                PlannedEndDate = DateTime.Now.AddMonths(8),
                IsActive = true,
                CreatedBy = "SHaldar",
                CreatedDate = DateTime.Now.AddMonths(-9),
                ModifiedBy = "SBirthare",
                ModifiedDate = DateTime.Now.AddMonths(-9)
            });

            context.Add(new Project
            {
                ProjectName = "Jericho",
                ProjectDescription = "ERP System",
                StartDate = DateTime.Now.AddMonths(-10),
                PlannedEndDate = DateTime.Now.AddMonths(9),
                IsActive = true,
                CreatedBy = "SHaldar",
                CreatedDate = DateTime.Now.AddMonths(-10),
                ModifiedBy = "SBirthare",
                ModifiedDate = DateTime.Now.AddMonths(-10)
            });

            context.Add(new Project
            {
                ProjectName = "Johnas",
                ProjectDescription = "Dental Plan System",
                StartDate = DateTime.Now.AddMonths(-5),
                PlannedEndDate = DateTime.Now.AddMonths(5),
                IsActive = true,
                CreatedBy = "SHaldar",
                CreatedDate = DateTime.Now.AddMonths(-5),
                ModifiedBy = "SBirthare",
                ModifiedDate = DateTime.Now.AddMonths(-5)
            });
        }

        private static void AddDefaultStatus(ProjectTrackerIpDataContext context)
        {
            context.Add(new Status
            {
                StatusName = "Green"
            });

            context.Add(new Status
            {
                StatusName = "Yellow"
            });

            context.Add(new Status
            {
                StatusName = "Red"
            });

            context.Add(new Status
            {
                StatusName = "Closed"
            });
        }

        private static void AddDefaultWeekData(ProjectTrackerIpDataContext context)
        {
            string yearCounts = ConfigurationManager.AppSettings["YearCountsForPastYear"];
            if (yearCounts == "0" || yearCounts == null)
            {
                yearCounts = "2";
            }
            int weekCount =0;
            ///Get weeks of year
            int year = DateTime.Now.Year;       
            //past data
            for (int iYearCount = 0; iYearCount < Convert.ToInt16(yearCounts); iYearCount++)
            {
                int yearCalculate = year - iYearCount - 1;
                var firstDayOfYear = new System.DateTime(yearCalculate, 1, 1);
                var beginningDayOfWeek = firstDayOfYear.AddDays(-1 * Convert.ToInt32(firstDayOfYear.DayOfWeek));
                var endingDayOfWeek = beginningDayOfWeek.AddDays(6);
                var weekOfYear = 1;

                List<WeekModel> weeksOfTheYear = new List<WeekModel>();

                while (beginningDayOfWeek.Year < yearCalculate + 1)
                {
                    var week = new WeekModel { Number = weekOfYear, BeginningOfWeek = beginningDayOfWeek };
                    weeksOfTheYear.Add(week);

                    beginningDayOfWeek = beginningDayOfWeek.AddDays(7);
                    endingDayOfWeek = beginningDayOfWeek.AddDays(6);

                    weekOfYear++;

                    context.Add(new Week
                    {
                        WeekId = weekCount++,
                        WeekNumber = week.Number,
                        Year = yearCalculate,
                        Text = String.Format(
                            "Week {0}: ({1} to {2})",
                            week.Number,
                            beginningDayOfWeek.ToShortDateString(),
                            endingDayOfWeek.ToShortDateString())
                    });
                }
            }
            yearCounts = ConfigurationManager.AppSettings["YearCountsForFutureYear"];
            //future data
            for (int iYearCount = 0; iYearCount < Convert.ToInt16(yearCounts); iYearCount++)
            {
                int yearCalculate = year + iYearCount;
                var firstDayOfYear = new System.DateTime(yearCalculate, 1, 1);
                var beginningDayOfWeek = firstDayOfYear.AddDays(-1 * Convert.ToInt32(firstDayOfYear.DayOfWeek));
                var endingDayOfWeek = beginningDayOfWeek.AddDays(6);

                var weekOfYear = 1;

                List<WeekModel> weeksOfTheYear = new List<WeekModel>();

                while (beginningDayOfWeek.Year < yearCalculate + 1)
                {
                    var week = new WeekModel { Number = weekOfYear, BeginningOfWeek = beginningDayOfWeek };
                    weeksOfTheYear.Add(week);

                    beginningDayOfWeek = beginningDayOfWeek.AddDays(7);
                    endingDayOfWeek = beginningDayOfWeek.AddDays(6);

                    weekOfYear++;

                    context.Add(new Week
                    {
                        WeekId = weekCount++,
                        WeekNumber = week.Number,
                        Year = yearCalculate,
                        Text = String.Format(
                            "Week {0}: ({1} to {2})",
                            week.Number,
                            beginningDayOfWeek.ToShortDateString(),
                            endingDayOfWeek.ToShortDateString())
                    });
                }
            }
        }

        private static void AddConstraints(ProjectTrackerIpDataContext context, string databaseName)
        {
            context.Database.CreateIfNotExists();

            var sqlContent = Content(UniqueConstraintScript);

            var modifiedSqlScript = sqlContent.Replace("@DatabaseName", databaseName);

            context.Database.ExecuteSqlCommand(modifiedSqlScript);
        }

        private static void AddProjectMapping(ProjectTrackerIpDataContext context)
        {
            context.Add(new ProjectMapping
            {
                UserName = "SRoy",
                ProjectId = 1,
                IsDeleted = false,
                CreatedBy = "SBirthare",
                CreatedDate = DateTime.Now.AddYears(-1),
                ModifiedBy = "SBirthare",
                ModifiedDate = DateTime.Now.AddDays(-10)
            });
            context.Add(new ProjectMapping
            {
                UserName = "SHaldar",
                ProjectId = 3,
                IsDeleted = false,
                CreatedBy = "SHaldar",
                CreatedDate = DateTime.Now.AddYears(-1),
                ModifiedBy = "SBirthare",
                ModifiedDate = DateTime.Now.AddDays(-10)
            });
            context.Add(new ProjectMapping
            {
                UserName = "SRoy",
                ProjectId = 2,
                IsDeleted = false,
                CreatedBy = "SBirthare",
                CreatedDate = DateTime.Now.AddYears(-1),
                ModifiedBy = "SBirthare",
                ModifiedDate = DateTime.Now.AddDays(-10)
            });

            context.Add(new ProjectMapping
            {
                UserName = "SRoy",
                ProjectId = 4,
                IsDeleted = false,
                CreatedBy = "SRoy",
                CreatedDate = DateTime.Now.AddYears(-1),
                ModifiedBy = "SRoy",
                ModifiedDate = DateTime.Now.AddDays(-10)
            });
            context.Add(new ProjectMapping
            {
                UserName = "SHaldar",
                ProjectId = 5,
                IsDeleted = false,
                CreatedBy = "SRoy",
                CreatedDate = DateTime.Now.AddYears(-1),
                ModifiedBy = "SRoy",
                ModifiedDate = DateTime.Now.AddDays(-10)
            });
            context.Add(new ProjectMapping
            {
                UserName = "SRoy",
                ProjectId = 6,
                IsDeleted = false,
                CreatedBy = "SHaldar",
                CreatedDate = DateTime.Now.AddYears(-1),
                ModifiedBy = "SHaldar",
                ModifiedDate = DateTime.Now.AddDays(-10)
            });
            context.Add(new ProjectMapping
            {
                UserName = "SHaldar",
                ProjectId = 7,
                IsDeleted = false,
                CreatedBy = "SHaldar",
                CreatedDate = DateTime.Now.AddYears(-1),
                ModifiedBy = "SHaldar",
                ModifiedDate = DateTime.Now.AddDays(-10)
            });

            context.Add(new ProjectMapping
            {
                UserName = "SHaldar",
                ProjectId = 9,
                IsDeleted = false,
                CreatedBy = "SHaldar",
                CreatedDate = DateTime.Now.AddYears(-1),
                ModifiedBy = "SHaldar",
                ModifiedDate = DateTime.Now.AddDays(-10)
            });

            //context.Add(new ProjectMapping
            //{
            //    UserName = "SRoy",
            //    ProjectId = 9,
            //    IsDeleted = false,
            //    CreatedBy = "SBirthare",
            //    CreatedDate = DateTime.Now.AddYears(-1),
            //    ModifiedBy = "SBirthare",
            //    ModifiedDate = DateTime.Now.AddDays(-10)
            //});
        }

        private static void AddProjecStatusData(ProjectTrackerIpDataContext context)
        {
            context.Add(new ProjectStatus
            {
                ProjectSummary = "Design Document Received.",
                StatusId = 2,
                WeekId = 60,
	            ProjectId = 4,
                CreatedBy = "SRoy",
	            CreatedDate = DateTime.Now,
                ModifiedBy = "SRoy",
                ModifiedDate = DateTime.Now,
                            });

            context.Add(new ProjectStatus
            {
                ProjectSummary = "Design Document Received. Kick off meeting this week",
                StatusId = 4,
                WeekId = 61,
                ProjectId = 4,
                CreatedBy = "SRoy",
                CreatedDate = DateTime.Now,
                ModifiedBy = "SHaldar",
                ModifiedDate = DateTime.Now
            });

            context.Add(new ProjectStatus
            {
                ProjectSummary = "Project is in critical condition.",
                StatusId = 3,
                WeekId = 61,
                ProjectId = 5,
                CreatedBy = "SHaldar",
                CreatedDate = DateTime.Now,
                ModifiedBy = "SHaldar",
                ModifiedDate = DateTime.Now
            });

            context.Add(new ProjectStatus
            {
                ProjectSummary = "Started Analysis for the requirement.",
                StatusId = 1,
                WeekId = 59,
                ProjectId = 5,
                CreatedBy = "shaldar",
                CreatedDate = DateTime.Now,
                ModifiedBy = "shaldar",
                ModifiedDate = DateTime.Now
            });

            context.Add(new ProjectStatus
            {
                ProjectSummary = "Design and Setup is complete.",
                StatusId = 2,
                WeekId = 61,
                ProjectId = 7,
                CreatedBy = "shaldar",
                CreatedDate = DateTime.Now,
                ModifiedBy = "shaldar",
                ModifiedDate = DateTime.Now
            });

            context.Add(new ProjectStatus
            {
                ProjectSummary = "Almost complete the final sprint.",
                StatusId = 1,
                WeekId = 61,
                ProjectId = 3,
                CreatedBy = "Shaldar",
                CreatedDate = DateTime.Now,
                ModifiedBy = "shaldar",
                ModifiedDate = DateTime.Now
            });           
        }

        private static void AddProjectResouceUtilizationData(ProjectTrackerIpDataContext context)
        {
            context.Add(new ProjectResourceUtilization
            {
                ProjectId = 1,
                WeekId = 61,
                RoleName = "Software Engineer",
                ResourceName = "Subeer",
                AvailableEfforts = 800,
                ConsumedEfforts = 800,
                Status = "For Development and Testing",
                IsActive = true,
                CreatedBy = "shaldar",
                CreatedDate = DateTime.Now,
                ModifiedBy = "shaldar",
                ModifiedDate = DateTime.Now
            }); 

            context.Add(new ProjectResourceUtilization
            {
                ProjectId = 3,
	            WeekId = 61,
	            RoleName = "Software Engineer",
	            ResourceName = "subeer",
	            AvailableEfforts = 400,
	            ConsumedEfforts = 100,
                Status = "For Development",
                IsActive = true,
                CreatedBy = "shaldar",
                CreatedDate = DateTime.Now,
                ModifiedBy = "shaldar",
                ModifiedDate = DateTime.Now
            });

            context.Add(new ProjectResourceUtilization
            {
                ProjectId = 2,
                WeekId = 61,
                RoleName = "Sr. Software Engineer",
                ResourceName = "Sulekha",
                AvailableEfforts = 400,
                ConsumedEfforts = 200,
                Status = "For Design and Development",
                IsActive = true,
                CreatedBy = "shaldar",
                CreatedDate = DateTime.Now,
                ModifiedBy = "shaldar",
                ModifiedDate = DateTime.Now
            });

            context.Add(new ProjectResourceUtilization
            {
                ProjectId = 3,
                WeekId = 61,
                RoleName = "Sr. Software Engineer",
                ResourceName = "Sulekha",
                AvailableEfforts = 400,
                ConsumedEfforts = 0,
                Status = "For Design and Development",
                IsActive = true,
                CreatedBy = "shaldar",
                CreatedDate = DateTime.Now,
                ModifiedBy = "shaldar",
                ModifiedDate = DateTime.Now
            });
        }

        private static void SeedMembershipData(ProjectTrackerIpDataContext context, string databaseName)
        {
            context.Database.CreateIfNotExists();

            var sqlContent = Content(IndexScriptSeedMembershipLocation);

            var modifiedSqlScript = sqlContent.Replace("@DatabaseName", databaseName);

            context.Database.ExecuteSqlCommand(modifiedSqlScript);
        }

        private static void DropExistingConnectionToDatabase(ProjectTrackerIpDataContext context, string databaseName)
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

        public static int GetWeek(DateTime date)
        {
            System.Globalization.CultureInfo cult_info = System.Globalization.CultureInfo.CreateSpecificCulture("no");
            System.Globalization.Calendar cal = cult_info.Calendar;
            int weekCount = cal.GetWeekOfYear(date, cult_info.DateTimeFormat.CalendarWeekRule, cult_info.DateTimeFormat.FirstDayOfWeek);
            return weekCount;

        }
    }
}
