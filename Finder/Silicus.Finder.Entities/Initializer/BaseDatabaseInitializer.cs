using System;
using System.Configuration;
using System.IO;
using System.Reflection;
//using Silicus.Finder.IdentityWrapper;
using Silicus.Finder.Models.DataObjects;
using System.Collections;
using System.Collections.Generic;


namespace Silicus.Finder.Entities.Initializer
{
    public class BaseDatabaseInitializer
    {
        private static readonly string DropConnectionScript = "Silicus.Finder.Entities.DatabaseScripts.DropConnection.sql";
        private static readonly string IndexScriptLocation = "Silicus.Finder.Entities.DatabaseScripts.SqlIndexes.sql";
        private static readonly string DatabaseName = ConfigurationManager.AppSettings["DBName"];
        private static readonly string MembershipDatabaseName = ConfigurationManager.AppSettings["MembershipDBName"];


        //public void Seed(FinderIpDataContext context)
        //{
        //    const string roleAdmin = "Admin";
        //    const string roleManager = "Manager";
        //    const string roleUser = "User";

        //    // Since the initializer is specified through web.config
        //    // not able to inject UserManager here.
        //    // TODO: Find a way to inject it.
        //    var userManager = new UserManager();

        //    // Create role in identity DB.
        //    userManager.CreateIdentityRoleIfNotExist(roleAdmin);
        //    userManager.CreateIdentityRoleIfNotExist(roleManager);
        //    userManager.CreateIdentityRoleIfNotExist(roleUser);

        //    // Create users in identity DB.
        //    var adminMembershipId = userManager.CreateUserIfNotExist("testadmin@test.com", "Testadmin@123");
        //    var managerMembershipId = userManager.CreateUserIfNotExist("testmanager@test.com", "Testmanager@123");
        //    var userMembershipId = userManager.CreateUserIfNotExist("testuser@test.com", "Testuser@123");
        //    var shailendraMembershipId = userManager.CreateUserIfNotExist("shailendra.birthare@gmail.com", "Shailendra@123");
        //    var abhishekMembershipId = userManager.CreateUserIfNotExist("abhishekjadhav@gmail.com", "Abhishek@123");
        //    var shivaniMembershipId = userManager.CreateUserIfNotExist("shivanisurana@gmail.com", "Shivani@123");
        //    var nikhilMembershipId = userManager.CreateUserIfNotExist("nikhildarwai@gmail.com", "Nikhil@123");
        //    var pratikMembershipId = userManager.CreateUserIfNotExist("pratikpatil@gmail.com", "Pratik@123");

        //    // Assign role to users.
        //    userManager.AssignRoleToUser(adminMembershipId, roleAdmin);
        //    userManager.AssignRoleToUser(managerMembershipId, roleManager);
        //    userManager.AssignRoleToUser(userMembershipId, roleUser);

        //    //Creating and adding Titles
        //    var SoftwareTraineeTitle = new Title
        //    {
        //        Name = "Software Trainee",
        //    };
        //    context.Add(SoftwareTraineeTitle);

        //    var softwareEngineer1Title = new Title
        //    {
        //        Name = "Software Engineer I",
        //    };
        //    context.Add(softwareEngineer1Title);

        //    var softwareEngineer2Title = new Title
        //    {
        //        Name = "Software Engineer II",
        //    };
        //    context.Add(softwareEngineer2Title);

        //    var softwareEngineer3Title = new Title
        //    {
        //        Name = "Software Engineer III",
        //    };
        //    context.Add(softwareEngineer3Title);

        //    var softwareEngineer4Title = new Title
        //    {
        //        Name = "Software Engineer IV",
        //    };
        //    context.Add(softwareEngineer4Title);

        //    var SeniorSoftwareEngineerTitle = new Title
        //    {
        //        Name = "Senior Software Engineer",
        //    };
        //    context.Add(SeniorSoftwareEngineerTitle);

        //    var TechnicleLeadTitle = new Title
        //    {
        //        Name = "Technicle Lead",
        //    };
        //    context.Add(TechnicleLeadTitle);
        //    var technicalLead1Title = new Title
        //    {
        //        Name = "Technical Lead I",
        //    };
        //    context.Add(technicalLead1Title);

        //    var technicalLead2Title = new Title
        //    {
        //        Name = "Technical Lead II",
        //    };
        //    context.Add(technicalLead2Title);

        //    var technicalLead3Title = new Title
        //    {
        //        Name = "Technical Lead III",
        //    };
        //    context.Add(technicalLead3Title);

        //    var technicalArchitect1Title = new Title
        //    {
        //        Name = "Technical Architect I",
        //    };
        //    context.Add(technicalArchitect1Title);

        //    var technicalArchitect2Title = new Title
        //    {
        //        Name = "Technical Architect II",
        //    };
        //    context.Add(technicalArchitect2Title);

        //    var technicalArchitect3Title = new Title
        //    {
        //        Name = "Technical Architect III",
        //    };
        //    context.Add(technicalArchitect3Title);

        //    var technicalArchitect4Title = new Title
        //    {
        //        Name = "Technical Architect IV",
        //    };
        //    context.Add(technicalArchitect4Title);

        //    var seniorSoftwareEngineer1Title = new Title
        //    {
        //        Name = "Senior Software Engineer I",
        //    };
        //    context.Add(seniorSoftwareEngineer1Title);

        //    var seniorSoftwareEngineer2Title = new Title
        //    {
        //        Name = "Senior Software Engineer II",
        //    };
        //    context.Add(seniorSoftwareEngineer2Title);

        //    var seniorSoftwareEngineer3Title = new Title
        //    {
        //        Name = "Senior Software Engineer III",
        //    };
        //    context.Add(seniorSoftwareEngineer3Title);

        //    var QATraineeTitle = new Title
        //    {
        //        Name = "QA Trainee",
        //    };
        //    context.Add(QATraineeTitle);

        //    var sQAEngineer1Title = new Title
        //    {
        //        Name = "SQA Engineer I",
        //    };
        //    context.Add(sQAEngineer1Title);

        //    var sQAEngineer2Title = new Title
        //    {
        //        Name = "SQA Engineer II",
        //    };
        //    context.Add(sQAEngineer2Title);

        //    var sQAEngineer3Title = new Title
        //    {
        //        Name = "SQA Engineer III",
        //    };
        //    context.Add(sQAEngineer3Title);

        //    var sQAEngineer4Title = new Title
        //    {
        //        Name = "SQA Engineer IV",
        //    };
        //    context.Add(sQAEngineer4Title);


        //    var sQAAnalyst1Title = new Title
        //    {
        //        Name = "SQA Analyst I",
        //    };
        //    context.Add(sQAAnalyst1Title);

        //    var sQAAnalyst2Title = new Title
        //    {
        //        Name = "SQA Analyst II",
        //    };
        //    context.Add(sQAAnalyst2Title);

        //    var sQAAnalyst3Title = new Title
        //    {
        //        Name = "SQA Analyst III",
        //    };
        //    context.Add(sQAAnalyst3Title);

        //    var sQAAnalyst4Title = new Title
        //    {
        //        Name = "SQA Analyst IV",
        //    };
        //    context.Add(sQAAnalyst4Title);

        //    var sQALead1Title = new Title
        //    {
        //        Name = "SQA Lead I",
        //    };
        //    context.Add(sQALead1Title);

        //    var sQALead2Title = new Title
        //    {
        //        Name = "SQA Lead II",
        //    };
        //    context.Add(sQALead2Title);

        //    var sQALead3Title = new Title
        //    {
        //        Name = "SQA Lead III",
        //    };
        //    context.Add(sQALead3Title);


        //    var sQAManager1Title = new Title
        //    {
        //        Name = "SQA Manager I",
        //    };
        //    context.Add(sQAManager1Title);

        //    var projectManager1Title = new Title
        //    {
        //        Name = "Project Manager I",
        //    };
        //    context.Add(projectManager1Title);

        //    var projectManager2Title = new Title
        //    {
        //        Name = "Project Manager II",
        //    };
        //    context.Add(projectManager2Title);

        //    var projectManager3Title = new Title
        //    {
        //        Name = "Project Manager III",
        //    };
        //    context.Add(projectManager3Title);

        //    var BusinessAnalyst1Title = new Title
        //    {
        //        Name = "Business Analyst I",
        //    };
        //    context.Add(BusinessAnalyst1Title);

        //    var BusinessAnalyst2Title = new Title
        //    {
        //        Name = "Business Analyst II",
        //    };
        //    context.Add(BusinessAnalyst2Title);

        //    var BusinessAnalyst3Title = new Title
        //    {
        //        Name = "Business Analyst III",
        //    };
        //    context.Add(BusinessAnalyst3Title);

        //    var BusinessAnalyst4Title = new Title
        //    {
        //        Name = "Business Analyst IV",
        //    };
        //    context.Add(BusinessAnalyst4Title);

        //    var SeniorBusinessAnalyst1Title = new Title
        //    {
        //        Name = "Senior Business Analyst I",
        //    };
        //    context.Add(SeniorBusinessAnalyst1Title);

        //    var MicrosoftInfrastructreAndSystemCenterConsultantTitle=new Title
        //    {
        //        Name = "Microsoft Infrastructre and System Center Consultant",
        //    };
        //    context.Add(MicrosoftInfrastructreAndSystemCenterConsultantTitle);

        //    var FrontEndDeveloper1Title = new Title
        //    {
        //        Name = "Front-End Developer I",
        //    };
        //    context.Add(FrontEndDeveloper1Title);

        //    var FrontEndDeveloper2Title = new Title
        //    {
        //        Name = "Front-End Developer II",
        //    };
        //    context.Add(FrontEndDeveloper2Title);

        //    var FrontEndDeveloper3Title = new Title
        //    {
        //        Name = "Front-End Developer III",
        //    };
        //    context.Add(FrontEndDeveloper3Title);

        //    var FrontEndDeveloper4Title = new Title
        //    {
        //        Name = "Front-End Developer IV",
        //    };
        //    context.Add(FrontEndDeveloper4Title);

        //    var FunctionalAnalyst1Title = new Title
        //    {
        //        Name = "Functional Analyst I",
        //    };
        //    context.Add(FunctionalAnalyst1Title);

        //    var FunctionalAnalyst2Title = new Title
        //    {
        //        Name = "Functional Analyst II",
        //    };
        //    context.Add(FunctionalAnalyst2Title);
                

        //    //add rewards
        //    var rewardEmployeeofTheMonth = new RewardsAndRecognition
        //    {
        //        RewardsAndRecognitionName = "Employee of the Month"
        //    };
        //    var rewardPatonTheBack = new RewardsAndRecognition
        //    {
        //        RewardsAndRecognitionName = "Pat on the back"
        //    };
        //    var rewardMostValuableEmployeeForProject = new RewardsAndRecognition
        //    {
        //        RewardsAndRecognitionName = "Most Valuable Employee For a Project"
        //    };

        //    context.Add(rewardEmployeeofTheMonth);
        //    context.Add(rewardMostValuableEmployeeForProject);
        //    context.Add(rewardPatonTheBack);

        //    // Child object for an employee
        //    var contactForShailendra = new Contact
        //    {
        //        //ContactId = 123,
        //        EmailAddress = "shailendra.birthare@gmail.com",
        //        MobileNumber = 7587382510,
        //        PhoneNumber = "02382-265234",
        //        Skype = "sbirthare.silicus"
        //    };

        //    context.Add(contactForShailendra);

        //    var cubicleLocationForShailendra = new CubicleLocation
        //    {
        //        //CubicleLocationId = 234,
        //        Building = "B",
        //        DeskNumber = "6/INCB-19",
        //        FloorNumber = 6
        //    };

        //    context.Add(cubicleLocationForShailendra);
        //    var adminUser = new Employee
        //    {
        //        Role = roleAdmin,
        //        EmployeeCode = "P387",
        //        Contact = contactForShailendra,
        //        CubicleLocation = cubicleLocationForShailendra,
        //        EmployeeType = "*",
        //        FirstName = "Shailendra",
        //        LastName = "Birthare",
        //        HighestQualification = "B.E.",
        //        ManagerRecommendation = "Hard Working",
        //        SilicusExperienceInMonths = 10,
        //        TotalExperienceInMonths = 30,
        //        Gender = Gender.Male,
        //        MembershipId = shailendraMembershipId
        //    };
        //    adminUser = AddTitle(adminUser, projectManager1Title.TitleId);
        //    context.Add(adminUser);

        //    var contact_Abhishek = new Contact
        //    {
        //        EmailAddress = "abhishekjadhav@gmail.com",
        //        MobileNumber = 7587387845,
        //        PhoneNumber = "02382-265234",
        //        Skype = "ajadhav.silicus"
        //    };

        //    context.Add(contact_Abhishek);

        //    var CubicleLocation_Abhishek = new CubicleLocation
        //    {
        //        Building = "B",
        //        DeskNumber = "5/INCB-14",
        //        FloorNumber = 5
        //    };

        //    context.Add(CubicleLocation_Abhishek);

        //    var CSharp = new SkillSet() { Name = "C#", Description = "C#.NET description" };
        //    var AspNet = new SkillSet() { Name = "ASP", Description = "ASP.NET description" };
        //    var VB = new SkillSet() { Name = "VB.NET", Description = "VB.NET description" };
        //    var java = new SkillSet() { Name = "Java", Description = "Java description" };
        //    var QA = new SkillSet() { Name="QA" , Description="QA" };
        //    var IOS = new SkillSet() { Name="IOS", Description="IOS"};
        //    var NET = new SkillSet() { Name=".NET" ,Description=".Net" };
        //    var NetPlc = new SkillSet() { Name=".NET PLC",Description=".Net Plc"};
        //    var Azure = new SkillSet() { Name="AZURE",Description="Azure"};
        //    var BA = new SkillSet() { Name="BA",Description="BA"};
        //    var CPlusPlus =new SkillSet(){Name="C++",Description="C++"};


        //    context.AddAll(new List<SkillSet>
        //        {
        //            CSharp,
        //            AspNet,
        //            VB,
        //            java,
        //            QA,
        //            IOS,
        //            NET,
        //            NetPlc,
        //            Azure,
        //            BA,
        //            CPlusPlus
        //        });

        //    var project_SilicusFinder = new Project()
        //    {
        //        ProjectName = "Silicus Finder",
        //        ProjectCode = "PR01",
        //        Description = "Employee Finder",
        //        ProjectType = ProjectType.Internal,
        //        EngagementType = EngagementType.Fixed_Price,
        //        Status = Status.On_Going,
        //        StartDate = new DateTime(2016, 1, 10),
        //        ExpectedEndDate = new DateTime(2016, 10, 20),
        //        ActualEndDate = new DateTime(2016, 10, 25),
        //        EngagementManagerId = 1,
        //        ProjectManagerId = 4,
        //        SkillSets = new List<SkillSet>
        //        {
        //            CSharp,
        //            AspNet,
        //        },
        //        AdditionalNotes = "Project is under development",
        //    };


        //    var project_Provare = new Project()
        //    {
        //        ProjectName = "Provare",
        //        ProjectCode = "PR02",
        //        Description = "Online tool for conducting tests",
        //        ProjectType = ProjectType.External,
        //        EngagementType = EngagementType.T_and_M,
        //        Status = Status.Completed,
        //        StartDate = new DateTime(2015, 1, 8),
        //        ExpectedEndDate = new DateTime(2016, 1, 1),
        //        ActualEndDate = new DateTime(2016, 1, 15),
        //        EngagementManagerId = 4,
        //        ProjectManagerId = 1,
        //        SkillSets = new List<SkillSet>
        //        {
        //            CSharp,
        //            AspNet,
        //        },
        //        AdditionalNotes = "Project is Complete"
        //    };


        //    var project_Frameworx = new Project
        //    {
        //        ProjectName = "Frameworx",
        //        ProjectCode = "PR03",
        //        Description = "Silicus internal .NET framework",
        //        ProjectType = ProjectType.Internal,
        //        EngagementType = EngagementType.Fixed_Price,
        //        Status = Status.Completed,
        //        StartDate = new DateTime(2015, 10, 10),
        //        ExpectedEndDate = new DateTime(2016, 2, 20),
        //        ActualEndDate = new DateTime(2016, 3, 1),
        //        EngagementManagerId = 4,
        //        ProjectManagerId = 3,
        //        SkillSets = new List<SkillSet>
        //           {
        //              CSharp
        //           },
        //        AdditionalNotes = "Project is udergoing final testing"
        //    };

        //    var project_Online_Reservation = new Project
        //    {
        //        ProjectName = "Online Reservation",
        //        ProjectCode = "PR04",
        //        Description = "Silicus internal .NET framework",
        //        ProjectType = ProjectType.Internal,
        //        EngagementType = EngagementType.Fixed_Price,
        //        Status = Status.On_Going,
        //        StartDate = new DateTime(2016, 1, 10),
        //        ExpectedEndDate = new DateTime(2016, 10, 20),
        //        ActualEndDate = new DateTime(2016, 12, 4),
        //        EngagementManagerId = 1,
        //        ProjectManagerId = 3,
        //        SkillSets = new List<SkillSet>
        //           {
        //               CSharp
        //           },
        //        AdditionalNotes = "under designing phase"
        //    };

        //    var project_Online_Admission = new Project
        //    {
        //        ProjectName = "Online Admission System",
        //        ProjectCode = "PR05",
        //        Description = "Silicus internal .NET framework",
        //        ProjectType = ProjectType.Internal,
        //        EngagementType = EngagementType.Fixed_Price,
        //        Status = Status.Completed,
        //        StartDate = new DateTime(2014, 12, 10),
        //        ExpectedEndDate = new DateTime(2015, 10, 20),
        //        ActualEndDate = new DateTime(2015, 12, 4),
        //        EngagementManagerId = 4,
        //        ProjectManagerId = 3,
        //        SkillSets = new List<SkillSet>
        //           {
        //               CSharp
        //           },
        //        AdditionalNotes = "Project is complete"
        //    };

        //    context.AddAll(new List<Project>
        //        {
        //            project_Provare,
        //            project_Frameworx,
        //            project_SilicusFinder,
        //            project_Online_Reservation,
        //            project_Online_Admission
        //        });

        //    var employeeAbhishek =
        //        new Employee
        //        {
        //            Role = roleUser,
        //            EmployeeCode = "P108",
        //            FirstName = "Abhishek",
        //            MiddleName = "A",
        //            LastName = "Jadhav",
        //            Gender = Gender.Male,
        //            Contact = contact_Abhishek,
        //            CubicleLocation = CubicleLocation_Abhishek,
        //            EmployeeType = "*",
        //            HighestQualification = "B.E.",
        //            ManagerRecommendation = "Punctual",
        //            SilicusExperienceInMonths = 4,
        //            TotalExperienceInMonths = 1,
        //            SkillSets = new List<SkillSet>()
        //             {
        //                CSharp,
        //                AspNet
        //            },
        //            Projects = new List<Project>()
        //            {
        //               project_Provare,
        //               project_SilicusFinder
        //           },

        //            IsActive = true,
        //            MembershipId = abhishekMembershipId
        //        };
        //    employeeAbhishek = AddTitle(employeeAbhishek, technicalArchitect1Title.TitleId);
        //    context.Add(employeeAbhishek);

        //    Contact contact_Shivani = new Contact()
        //    {
        //        EmailAddress = "shivanisurana@gmail.com",
        //        MobileNumber = 7584392356,
        //        PhoneNumber = "02382-265236",
        //        Skype = "ssurana.silicus"
        //    };

        //    CubicleLocation CubicleLocation_Shivani = new CubicleLocation()
        //    {
        //        Building = "B",
        //        DeskNumber = "5/INCB-18",
        //        FloorNumber = 5
        //    };



        //    var employeeShivani =
        //    new Employee
        //    {
        //        Role = roleUser,
        //        EmployeeCode = "P101",
        //        FirstName = "Shivani",
        //        MiddleName = "S",
        //        LastName = "Surana",
        //        Gender = Gender.Female,
        //        Contact = contact_Shivani,
        //        CubicleLocation = CubicleLocation_Shivani,
        //        //EmployeeSkillSets
        //        EmployeeType = "*",
        //        HighestQualification = "C-DAC",
        //        ManagerRecommendation = "Hard working",
        //        SilicusExperienceInMonths = 4,
        //        TotalExperienceInMonths = 4,

        //        SkillSets = new List<SkillSet> 
        //        { 
        //           CSharp
        //        },

        //        IsActive = true,
        //        MembershipId = shivaniMembershipId
        //    };
        //    employeeShivani = AddTitle(employeeShivani, projectManager1Title.TitleId);
        //    context.Add(employeeShivani);

        //    var contact_Nikhil = new Contact
        //    {
        //        EmailAddress = "nikhildarwai@gmail.com",
        //        MobileNumber = 7582395975,
        //        PhoneNumber = "02382-265230",
        //        Skype = "ndarwai.silicus"
        //    };

        //    context.Add(contact_Nikhil);

        //    var cubicalLocation_Nikhil = new CubicleLocation
        //    {
        //        Building = "B",
        //        DeskNumber = "5/INCB-23",
        //        FloorNumber = 5
        //    };

        //    context.Add(cubicalLocation_Nikhil);

        //    var employeeNikhil = new Employee
        //    {
        //        Role = roleUser,
        //        EmployeeCode = "P102",
        //        FirstName = "Nikhil",
        //        MiddleName = "D",
        //        LastName = "Darwai",
        //        Gender = Gender.Male,
        //        Contact = contact_Nikhil,
        //        CubicleLocation = cubicalLocation_Nikhil,
        //        //EmployeeSkillSets
        //        EmployeeType = "*",
        //        HighestQualification = "C-DAC",
        //        ManagerRecommendation = "Has good knowledge about Angular js",
        //        SilicusExperienceInMonths = 4,
        //        TotalExperienceInMonths = 4,
        //        SkillSets = new List<SkillSet> 
        //        { 
        //           CSharp
        //        },

        //        Projects = new List<Project> 
        //        {
        //            project_Provare
        //        },
        //        MembershipId = nikhilMembershipId
        //    };
        //    employeeNikhil = AddTitle(employeeNikhil, projectManager1Title.TitleId);
        //   context.Add(employeeNikhil);

        //    var contact_pratik = new Contact
        //    {
        //        EmailAddress = "pratikpatil@gmail.com",
        //        MobileNumber = 7581761426,
        //        PhoneNumber = "02382-124785",
        //        Skype = "ppatil.silicus"
        //    };

        //    context.Add(contact_pratik);

        //    var cubicleLocation_pratik = new CubicleLocation
        //    {
        //        Building = "B",
        //        DeskNumber = "5/INCB-20",
        //        FloorNumber = 5
        //    };
        //    context.Add(cubicleLocation_pratik);

        //    var employeePratik =
        //        new Employee
        //    {
        //        Role = roleUser,
        //        EmployeeCode = "P104",
        //        FirstName = "Pratik",
        //        MiddleName = "P",
        //        LastName = "Patil",
        //        Gender = Gender.Male,
        //        Contact = contact_pratik,
        //        CubicleLocation = cubicleLocation_pratik,
        //        //EmployeeSkillSets
        //        EmployeeType = "*",
        //        HighestQualification = "C-DAC",
        //        ManagerRecommendation = "Good problem solving capability",
        //        SilicusExperienceInMonths = 4,
        //        TotalExperienceInMonths = 4,

        //        SkillSets = new List<SkillSet> 
        //        { 
        //           VB
        //        },

        //        Projects = new List<Project> 
        //        {
        //           project_Online_Reservation
        //        },
        //        IsActive = true,
        //        MembershipId = pratikMembershipId
        //    };
        //    employeePratik = AddTitle(employeePratik, softwareEngineer1Title.TitleId);
        //    context.Add(employeePratik);
        //}

        //private Employee AddTitle(Employee newEmployee, int titleId)
        //{
        //    newEmployee.EmployeeTitles = new List<EmployeeTitles>();
        //    newEmployee.EmployeeTitles.Add(
        //    new EmployeeTitles
        //    {
        //        TitleId = titleId,
        //        IsCurrent = true
        //    });
        //    return newEmployee;
        //}
        //private static void AddIndexes(FinderIpDataContext context, string databaseName)
        //{
        //    context.Database.CreateIfNotExists();

        //    var sqlContent = Content(IndexScriptLocation);

        //    var modifiedSqlScript = sqlContent.Replace("@DatabaseName", databaseName);

        //    context.Database.ExecuteSqlCommand(modifiedSqlScript);
        //}

        //private static void DropExistingConnectionToDatabase(FinderIpDataContext context, string databaseName)
        //{
        //    var sqlContent = Content(DropConnectionScript);

        //    var modifiedSqlScript = sqlContent.Replace("@DatabaseName", databaseName);

        //    context.Database.ExecuteSqlCommand(modifiedSqlScript);           
        //}

        //private static string Content(string fileLocation)
        //{
        //    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileLocation))
        //    {
        //        if (stream == null)
        //        {
        //            return string.Empty;
        //        }

        //        var streamReader = new StreamReader(stream);

        //        return streamReader.ReadToEnd();
        //    }
      //  }
    }
}