  using System;
  using System.Configuration;
  using System.Data.Entity.Migrations;
  using System.Data.Entity.Validation;
using System.IO;
  using System.Linq;
  using System.Reflection;
  using System.Text;
  using UtilityDataSyncLibrary.Mapping;

namespace UtilityDataSyncLibrary
{
    public class SyncFunctions
    {
        private static IMappingService _mappingService;
        private string _path;

        public SyncFunctions()
        {
            _path = ConfigurationManager.AppSettings["LogFilePath"] + "\\UtilitySync_Log_" + DateTime.Now.ToString("MMddyyyy") + ".txt";
            if (!Directory.Exists(ConfigurationManager.AppSettings["LogFilePath"]))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["LogFilePath"]);
            }
            _mappingService = new MappingService();
        }

        public void SyncData()
        {
            try
            {
                AutoMapperConfiguration.Configure();
                using (var enableContext = new EnableDevEntities())
                using (var utilityContainerContext = new UtilityContainerEntities())
                {
                    File.AppendAllText(_path, DateTime.Now.ToString() + ":" + MethodBase.GetCurrentMethod().Name + " : Sync started" + Environment.NewLine);
                    // Master data
                    SyncClients(enableContext, utilityContainerContext);
                    SyncEngagementTypes(enableContext, utilityContainerContext);
                    SyncDepartments(enableContext, utilityContainerContext);
                    SyncLocations(enableContext, utilityContainerContext);
                    SyncResourceTypes(enableContext, utilityContainerContext);
                    SyncSkills(enableContext, utilityContainerContext);

                    // Data
                    SyncTitles(enableContext, utilityContainerContext);
                    SyncUsers(enableContext, utilityContainerContext);
                    SyncEngagements(enableContext, utilityContainerContext);
                    SyncEngagementTaskTypes(enableContext, utilityContainerContext);
                    SyncResources(enableContext, utilityContainerContext);
                    SyncResourceHistories(enableContext, utilityContainerContext);
                    SyncResourceSkillLevels(enableContext, utilityContainerContext);
                    SyncEngagementRoles(enableContext, utilityContainerContext);
                    SyncCompanies(enableContext, utilityContainerContext);

                    File.AppendAllText(_path, DateTime.Now.ToString() + ":" + MethodBase.GetCurrentMethod().Name + " : Sync finished" + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                while (ex.InnerException != null)
                {
                    sb.Append(ex.Message);
                    ex = ex.InnerException;
                }
                File.AppendAllText(_path, DateTime.Now.ToString() + ": Error : " + MethodBase.GetCurrentMethod().Name + " : " + sb.ToString() + Environment.NewLine);
            }
        }

        public void SyncUsers(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            var users = enableContext.vwExt_User.ToList();
            foreach (var vwExtUser in users)
            {
                var enableUser = _mappingService.Map<vwExt_User, User>(vwExtUser);

                using (var transaction = utilityContainerContext.Database.BeginTransaction())
                {
                    try
                    {
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[User] ON");
                        utilityContainerContext.Users.AddOrUpdate(enableUser);
                        utilityContainerContext.SaveChanges();
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[User] OFF");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
                    }
                }
            }
        }

        public void SyncEngagements(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            var engagements = enableContext.vwExt_Engagement.ToList();
            foreach (var vwExtEngagement in engagements)
            {
                var enableEngagement = _mappingService.Map<vwExt_Engagement, Engagement>(vwExtEngagement);

                using (var transaction = utilityContainerContext.Database.BeginTransaction())
                {
                    try
                    {
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Engagement] ON");
                        utilityContainerContext.Engagements.AddOrUpdate(enableEngagement);
                        utilityContainerContext.SaveChanges();
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Engagement] OFF");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
                    }
                }
            }
        }

        public void SyncClients(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            File.AppendAllText(_path, MethodBase.GetCurrentMethod().Name + " : " + Environment.NewLine);
            var clients = enableContext.vwExt_Client.ToList();
            foreach (var vwExtClient in clients)
            {
                var enableClient = _mappingService.Map<vwExt_Client, Client>(vwExtClient);

                using (var transaction = utilityContainerContext.Database.BeginTransaction())
                {
                    try
                    {
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Client] ON");
                        utilityContainerContext.Clients.AddOrUpdate(enableClient);
                        utilityContainerContext.SaveChanges();
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Client] OFF");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
                    }
                }
            }
        }

        public void SyncResources(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            var resources = enableContext.vwExt_Resource.ToList();
            foreach (var vwExtResource in resources)
            {
                var enableResource = _mappingService.Map<vwExt_Resource, Resource>(vwExtResource);

                using (var transaction = utilityContainerContext.Database.BeginTransaction())
                {
                    try
                    {
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Resource] ON");
                        utilityContainerContext.Resources.AddOrUpdate(enableResource);
                        utilityContainerContext.SaveChanges();
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Resource] OFF");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
                    }
                }
            }
        }

        public void SyncResourceHistories(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            var resourceHistories = enableContext.vwExt_ResourceHistory.ToList();
            foreach (var vwExtResourceHistory in resourceHistories)
            {
                var enableResourceHistory = _mappingService.Map<vwExt_ResourceHistory, ResourceHistory>(vwExtResourceHistory);

                using (var transaction = utilityContainerContext.Database.BeginTransaction())
                {
                    try
                    {
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[ResourceHistory] ON");
                        utilityContainerContext.ResourceHistories.AddOrUpdate(enableResourceHistory);
                        utilityContainerContext.SaveChanges();
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[ResourceHistory] OFF");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
                    }
                }
            }
        }

        public void SyncDepartments(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            var departments = enableContext.vwExt_Department.ToList();
            try
            {
                foreach (var vwExtDepartment in departments)
                {
                    var enableDepartment = _mappingService.Map<vwExt_Department, Department>(vwExtDepartment);

                    using (var transaction = utilityContainerContext.Database.BeginTransaction())
                    {
                        try
                        {
                            utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Department] ON");
                            utilityContainerContext.Departments.AddOrUpdate(enableDepartment);
                            utilityContainerContext.SaveChanges();
                            utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Department] OFF");
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
            }
        }

        public void SyncLocations(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            var locations = enableContext.vwExt_Location.ToList();
            foreach (var vwExtLocation in locations)
            {
                var enableLocation = _mappingService.Map<vwExt_Location, Location>(vwExtLocation);

                using (var transaction = utilityContainerContext.Database.BeginTransaction())
                {
                    try
                    {
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Location] ON");
                        utilityContainerContext.Locations.AddOrUpdate(enableLocation);
                        utilityContainerContext.SaveChanges();
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Location] OFF");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
                    }
                }
            }
        }

        public void SyncEngagementRoles(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            var engagementRoles = enableContext.vwExt_EngagementRoles.ToList();
            foreach (var vwExtEngagementRole in engagementRoles)
            {
                var enableEngagementRole = _mappingService.Map<vwExt_EngagementRoles, EngagementRoles>(vwExtEngagementRole);

                using (var transaction = utilityContainerContext.Database.BeginTransaction())
                {
                    try
                    {
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[EngagementRoles] ON");
                        utilityContainerContext.EngagementRoles.AddOrUpdate(enableEngagementRole);
                        utilityContainerContext.SaveChanges();
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[EngagementRoles] OFF");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
                    }
                }
            }
        }

        public void SyncTitles(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            var titles = enableContext.vwExt_Title.ToList();
            foreach (var vwExtTitle in titles)
            {
                var enableTitle = _mappingService.Map<vwExt_Title, Title>(vwExtTitle);

                using (var transaction = utilityContainerContext.Database.BeginTransaction())
                {
                    try
                    {
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Title] ON");
                        utilityContainerContext.Titles.AddOrUpdate(enableTitle);
                        utilityContainerContext.SaveChanges();
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Title] OFF");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
                    }
                }
            }
        }

        public void SyncResourceSkillLevels(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            var ResourceSkillLevels = enableContext.vwExt_ResourceSkillLevel.ToList();
            foreach (var vwExtResourceSkillLevel in ResourceSkillLevels)
            {
                var enableResourceSkillLevel = _mappingService.Map<vwExt_ResourceSkillLevel, ResourceSkillLevel>(vwExtResourceSkillLevel);

                using (var transaction = utilityContainerContext.Database.BeginTransaction())
                {
                    try
                    {
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[ResourceSkillLevel] ON");
                        utilityContainerContext.ResourceSkillLevels.AddOrUpdate(enableResourceSkillLevel);
                        utilityContainerContext.SaveChanges();
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[ResourceSkillLevel] OFF");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
                    }
                }
            }
        }

        public void SyncSkills(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            var skills = enableContext.vwExt_Skill.ToList();
            foreach (var vwExtSkill in skills)
            {
                var enableSkill = _mappingService.Map<vwExt_Skill, Skill>(vwExtSkill);

                using (var transaction = utilityContainerContext.Database.BeginTransaction())
                {
                    try
                    {
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Skill] ON");
                        utilityContainerContext.Skills.AddOrUpdate(enableSkill);
                        utilityContainerContext.SaveChanges();
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Skill] OFF");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
                    }
                }
            }
        }

        public void SyncCompanies(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            var companies = enableContext.vwExt_Company.ToList();
            foreach (var vwExtCompany in companies)
            {
                var enableCompany = _mappingService.Map<vwExt_Company, Company>(vwExtCompany);

                using (var transaction = utilityContainerContext.Database.BeginTransaction())
                {
                    try
                    {
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Company] ON");
                        utilityContainerContext.Companies.AddOrUpdate(enableCompany);
                        utilityContainerContext.SaveChanges();
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Company] OFF");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
                    }
                }
            }
        }

        public void SyncEngagementTaskTypes(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            var engagementTaskTypes = enableContext.vwExt_EngagementTaskTypes.ToList();
            foreach (var vwExtEngagementTaskType in engagementTaskTypes)
            {
                var enableEngagementTaskType = _mappingService.Map<vwExt_EngagementTaskTypes, EngagementTaskTypes>(vwExtEngagementTaskType);

                using (var transaction = utilityContainerContext.Database.BeginTransaction())
                {
                    try
                    {
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[EngagementTaskTypes] ON");
                        utilityContainerContext.EngagementTaskTypes.AddOrUpdate(enableEngagementTaskType);
                        utilityContainerContext.SaveChanges();
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[EngagementTaskTypes] OFF");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
                    }
                }
            }
        }

        public void SyncEngagementTypes(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            var engagementTypes = enableContext.vwExt_EngagementType.ToList();
            foreach (var vwExtEngagementType in engagementTypes)
            {
                var enableEngagementType = _mappingService.Map<vwExt_EngagementType, EngagementType>(vwExtEngagementType);

                using (var transaction = utilityContainerContext.Database.BeginTransaction())
                {
                    try
                    {
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[EngagementType] ON");
                        utilityContainerContext.EngagementTypes.AddOrUpdate(enableEngagementType);
                        utilityContainerContext.SaveChanges();
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[EngagementType] OFF");
                        transaction.Commit();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            foreach (var ve in eve.ValidationErrors)
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
                    }
                }
            }
        }

        public void SyncResourceTypes(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            File.AppendAllText(_path, DateTime.Now.ToString() + " : " + MethodBase.GetCurrentMethod().Name + Environment.NewLine);
            var resourceTypes = enableContext.vwExt_ResourceType.ToList();
            foreach (var vwExtResourceType in resourceTypes)
            {
                var enableResourceType = _mappingService.Map<vwExt_ResourceType, ResourceType>(vwExtResourceType);

                using (var transaction = utilityContainerContext.Database.BeginTransaction())
                {
                    try
                    {
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[ResourceType] ON");
                        utilityContainerContext.ResourceTypes.AddOrUpdate(enableResourceType);
                        utilityContainerContext.SaveChanges();
                        utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[ResourceType] OFF");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        File.AppendAllText(_path, DateTime.Now.ToString() + " : Error : " + MethodBase.GetCurrentMethod().Name + " : " + ex.Message + Environment.NewLine);
                    }
                }
            }
        }

    }
}
