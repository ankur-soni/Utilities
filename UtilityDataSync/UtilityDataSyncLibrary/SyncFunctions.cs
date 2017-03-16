using System;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Silicus.FrameWorx.Logger;
using UtilityDataSyncLibrary.Mapping;

namespace UtilityDataSyncLibrary
{
    public class SyncFunctions
    {
        private static IMappingService _mappingService;
        private static ILogger _logger;

        public SyncFunctions()
        {
            _mappingService = new MappingService();
            _logger = new DatabaseLogger("name=UtilityContainerEntities", Type.GetType(string.Empty), (Func<DateTime>)(() => DateTime.UtcNow), string.Empty);
        }

        public void SyncData()
        {
            try
            {
                AutoMapperConfiguration.Configure();
                using (var enableContext = new EnableDevEntities())
                using (var utilityContainerContext = new UtilityContainerEntities())
                {
                    enableContext.Configuration.AutoDetectChangesEnabled = false;
                    enableContext.Configuration.ValidateOnSaveEnabled = false;

                    _logger.Log("Sync started");
                    //Master data
                    SyncClients(enableContext, utilityContainerContext);
                    SyncEngagementTypes(enableContext, utilityContainerContext);
                    SyncDepartments(enableContext, utilityContainerContext);
                    //SyncLocations(enableContext, utilityContainerContext);
                    SyncResourceTypes(enableContext, utilityContainerContext);
                    SyncSkills(enableContext, utilityContainerContext);

                    // Data
                    SyncTitles(enableContext, utilityContainerContext);
                    SyncUsers(enableContext, utilityContainerContext);
                    SyncEngagements(enableContext, utilityContainerContext);
                    //SyncEngagementTaskTypes(enableContext, utilityContainerContext);
                    SyncResources(enableContext, utilityContainerContext);
                    SyncResourceHistories(enableContext, utilityContainerContext);
                    //SyncResourceSkillLevels(enableContext, utilityContainerContext);
                    SyncEngagementRoles(enableContext, utilityContainerContext);
                    //SyncCompanies(enableContext, utilityContainerContext);

                    _logger.Log("Sync finished");
                }  
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        public void SyncUsers(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
            var users = enableContext.vwExt_User.ToList();
            var enableUsers = _mappingService.Map<vwExt_User[], User[]>(users.ToArray()).ToList();
            var utilityUserIds = utilityContainerContext.Users.Select(c => c.ID).ToList();
            using (var transaction = utilityContainerContext.Database.BeginTransaction())
            {
                try
                {
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[User] ON");
                    utilityContainerContext.AddAll(enableUsers.Where(e => !utilityUserIds.Contains(e.ID)));
                    utilityContainerContext.UpdateAll(enableUsers.Where(e => utilityUserIds.Contains(e.ID)));
                    utilityContainerContext.SaveChanges();
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[User] OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Log(ex);
                    transaction.Rollback();
                }
            }
        }

        public void SyncEngagements(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
            var engagements = enableContext.vwExt_Engagement.ToList();
            var enableEngagements = _mappingService.Map<vwExt_Engagement[], Engagement[]>(engagements.ToArray()).ToList();
            var utilityEngagementIds = utilityContainerContext.Engagements.Select(c => c.ID).ToList();
            using (var transaction = utilityContainerContext.Database.BeginTransaction())
            {
                try
                {
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Engagement] ON");
                    utilityContainerContext.AddAll(enableEngagements.Where(e => !utilityEngagementIds.Contains(e.ID)));
                    utilityContainerContext.UpdateAll(enableEngagements.Where(e => utilityEngagementIds.Contains(e.ID)));
                    utilityContainerContext.SaveChanges();
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Engagement] OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Log(ex); 
                    transaction.Rollback();
                }
            }
        }

        public void SyncClients(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
            var clients = enableContext.vwExt_Client.ToList();
            var enableClients = _mappingService.Map<vwExt_Client[], Client[]>(clients.ToArray()).ToList();
            var utilityClientIds = utilityContainerContext.Clients.Select(c => c.ID).ToList();
            using (var transaction = utilityContainerContext.Database.BeginTransaction())
            {
                try
                {
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Client] ON");
                    utilityContainerContext.AddAll(enableClients.Where(e => !utilityClientIds.Contains(e.ID)));
                    utilityContainerContext.UpdateAll(enableClients.Where(e => utilityClientIds.Contains(e.ID)));
                    utilityContainerContext.SaveChanges();
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Client] OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Log(ex); 
                    transaction.Rollback();
                }
            }
        }

        public void SyncResources(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
            var resources = enableContext.vwExt_Resource.ToList();
            var enableResources = _mappingService.Map<vwExt_Resource[], Resource[]>(resources.ToArray()).ToList();
            var utilityResourceIds = utilityContainerContext.Resources.Select(c => c.ID).ToList();
            using (var transaction = utilityContainerContext.Database.BeginTransaction())
            {
                try
                {
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Resource] ON");
                    utilityContainerContext.AddAll(enableResources.Where(e => !utilityResourceIds.Contains(e.ID)));
                    utilityContainerContext.UpdateAll(enableResources.Where(e => utilityResourceIds.Contains(e.ID)));
                    utilityContainerContext.SaveChanges();
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Resource] OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Log(ex); 
                    transaction.Rollback();
                }
            }
        }

        public void SyncResourceHistories(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
            var resourceHistorys = enableContext.vwExt_ResourceHistory.ToList();
            var enableResourceHistorys = _mappingService.Map<vwExt_ResourceHistory[], ResourceHistory[]>(resourceHistorys.ToArray()).ToList();
            var utilityResourceHistoryIds = utilityContainerContext.ResourceHistories.Select(c => c.ID).ToList();
            using (var transaction = utilityContainerContext.Database.BeginTransaction())
            {
                try
                {
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[ResourceHistory] ON");
                    utilityContainerContext.AddAll(enableResourceHistorys.Where(e => !utilityResourceHistoryIds.Contains(e.ID)));
                    utilityContainerContext.UpdateAll(enableResourceHistorys.Where(e => utilityResourceHistoryIds.Contains(e.ID)));
                    utilityContainerContext.SaveChanges();
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[ResourceHistory] OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Log(ex); 
                    transaction.Rollback();
                }
            }
        }

        public void SyncDepartments(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
            var departments = enableContext.vwExt_Department.ToList();
            var enableDepartments = _mappingService.Map<vwExt_Department[], Department[]>(departments.ToArray()).ToList();
            var utilityDepartmentIds = utilityContainerContext.Departments.Select(c => c.ID).ToList();
            using (var transaction = utilityContainerContext.Database.BeginTransaction())
            {
                try
                {
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Department] ON");
                    utilityContainerContext.AddAll(enableDepartments.Where(e => !utilityDepartmentIds.Contains(e.ID)));
                    utilityContainerContext.UpdateAll(enableDepartments.Where(e => utilityDepartmentIds.Contains(e.ID)));
                    utilityContainerContext.SaveChanges();
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Department] OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Log(ex); 
                    transaction.Rollback();
                }
            }
        }

        //public void SyncLocations(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        //{
        //    _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
        //    var locations = enableContext.vwExt_Location.ToList();
        //    var enableLocations = _mappingService.Map<vwExt_Location[], Location[]>(locations.ToArray()).ToList();
        //    var utilityLocationIds = utilityContainerContext.Locations.Select(c => c.ID).ToList();
        //    using (var transaction = utilityContainerContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Location] ON");
        //            utilityContainerContext.AddAll(enableLocations.Where(e => !utilityLocationIds.Contains(e.ID)));
        //            utilityContainerContext.UpdateAll(enableLocations.Where(e => utilityLocationIds.Contains(e.ID)));
        //            utilityContainerContext.SaveChanges();
        //            utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Location] OFF");
        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.Log(ex); 
        //            transaction.Rollback();
        //        }
        //    }
        //}

        public void SyncEngagementRoles(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
            var engagementRoles = enableContext.vwExt_EngagementRoles.ToList();
            var enableEngagementRoles = _mappingService.Map<vwExt_EngagementRoles[], EngagementRoles[]>(engagementRoles.ToArray()).ToList();
            var utilityEngagementRoleIds = utilityContainerContext.EngagementRoles.Select(c => c.ID).ToList();
            using (var transaction = utilityContainerContext.Database.BeginTransaction())
            {
                try
                {
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[EngagementRoles] ON");
                    utilityContainerContext.AddAll(enableEngagementRoles.Where(e => !utilityEngagementRoleIds.Contains(e.ID)));
                    utilityContainerContext.UpdateAll(enableEngagementRoles.Where(e => utilityEngagementRoleIds.Contains(e.ID)));
                    utilityContainerContext.SaveChanges();
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[EngagementRoles] OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Log(ex);
                    transaction.Rollback();
                }
            }
        }

        public void SyncTitles(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
            var titles = enableContext.vwExt_Title.ToList();
            var enableTitles = _mappingService.Map<vwExt_Title[], Title[]>(titles.ToArray()).ToList();
            var utilityTitleIds = utilityContainerContext.Titles.Select(c => c.ID).ToList();
            using (var transaction = utilityContainerContext.Database.BeginTransaction())
            {
                try
                {
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Title] ON");
                    utilityContainerContext.AddAll(enableTitles.Where(e => !utilityTitleIds.Contains(e.ID)));
                    utilityContainerContext.UpdateAll(enableTitles.Where(e => utilityTitleIds.Contains(e.ID)));
                    utilityContainerContext.SaveChanges();
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Title] OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Log(ex); 
                    transaction.Rollback();
                }
            }
        }

        public void SyncResourceSkillLevels(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
            var resourceSkillLevels = enableContext.vwExt_ResourceSkillLevel.ToList();
            var enableResourceSkillLevels = _mappingService.Map<vwExt_ResourceSkillLevel[], ResourceSkillLevel[]>(resourceSkillLevels.ToArray()).ToList();
            var utilityResourceSkillLevelIds = utilityContainerContext.ResourceSkillLevels.Select(c => c.ID).ToList();
            using (var transaction = utilityContainerContext.Database.BeginTransaction())
            {
                try
                {
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[ResourceSkillLevel] ON");
                    utilityContainerContext.AddAll(enableResourceSkillLevels.Where(e => !utilityResourceSkillLevelIds.Contains(e.ID)));
                    utilityContainerContext.UpdateAll(enableResourceSkillLevels.Where(e => utilityResourceSkillLevelIds.Contains(e.ID)));
                    utilityContainerContext.SaveChanges();
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[ResourceSkillLevel] OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Log(ex); 
                    transaction.Rollback();
                }
            }
        }

        public void SyncSkills(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
            var skills = enableContext.vwExt_Skill.ToList();
            var enableSkills = _mappingService.Map<vwExt_Skill[], Skill[]>(skills.ToArray()).ToList();
            var utilitySkillIds = utilityContainerContext.Skills.Select(c => c.ID).ToList();
            using (var transaction = utilityContainerContext.Database.BeginTransaction())
            {
                try
                {
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Skill] ON");
                    utilityContainerContext.AddAll(enableSkills.Where(e => !utilitySkillIds.Contains(e.ID)));
                    utilityContainerContext.UpdateAll(enableSkills.Where(e => utilitySkillIds.Contains(e.ID)));
                    utilityContainerContext.SaveChanges();
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Skill] OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Log(ex); 
                    transaction.Rollback();
                }
            }
        }

        public void SyncCompanies(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
            var companies = enableContext.vwExt_Company.ToList();
            var enableCompanies = _mappingService.Map<vwExt_Company[], Company[]>(companies.ToArray()).ToList();
            var utilityCompanyIds = utilityContainerContext.Companies.Select(c => c.ID).ToList();
            using (var transaction = utilityContainerContext.Database.BeginTransaction())
            {
                try
                {
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Company] ON");
                    utilityContainerContext.AddAll(enableCompanies.Where(e => !utilityCompanyIds.Contains(e.ID)));
                    utilityContainerContext.UpdateAll(enableCompanies.Where(e => utilityCompanyIds.Contains(e.ID)));
                    utilityContainerContext.SaveChanges();
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Company] OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Log(ex); 
                    transaction.Rollback();
                }
            }
        }

        public void SyncEngagementTaskTypes(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
            var engagementTaskTypes = enableContext.vwExt_EngagementTaskTypes.ToList();
            var enableEngagementTaskTypes = _mappingService.Map<vwExt_EngagementTaskTypes[], EngagementTaskTypes[]>(engagementTaskTypes.ToArray()).ToList();
            var utilityEngagementTaskTypeIds = utilityContainerContext.EngagementTaskTypes.Select(c => c.ID).ToList();
            using (var transaction = utilityContainerContext.Database.BeginTransaction())
            {
                try
                {
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[EngagementTaskTypes] ON");
                    utilityContainerContext.AddAll(enableEngagementTaskTypes.Where(e => !utilityEngagementTaskTypeIds.Contains(e.ID)));
                    utilityContainerContext.UpdateAll(enableEngagementTaskTypes.Where(e => utilityEngagementTaskTypeIds.Contains(e.ID)));
                    utilityContainerContext.SaveChanges();
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[EngagementTaskTypes] OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Log(ex); 
                    transaction.Rollback();
                }
            }
        }

        public void SyncEngagementTypes(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
            var engagementTypes = enableContext.vwExt_EngagementType.ToList();
            var enableEngagementTypes = _mappingService.Map<vwExt_EngagementType[], EngagementType[]>(engagementTypes.ToArray()).ToList();
            var utilityEngagementTypeIds = utilityContainerContext.EngagementTypes.Select(c => c.ID).ToList();
            using (var transaction = utilityContainerContext.Database.BeginTransaction())
            {
                try
                {
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[EngagementType] ON");
                    utilityContainerContext.AddAll(enableEngagementTypes.Where(e => !utilityEngagementTypeIds.Contains(e.ID)));
                    utilityContainerContext.UpdateAll(enableEngagementTypes.Where(e => utilityEngagementTypeIds.Contains(e.ID)));
                    utilityContainerContext.SaveChanges();
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[EngagementType] OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Log(ex); 
                    transaction.Rollback();
                }
            }
        }

        public void SyncResourceTypes(EnableDevEntities enableContext, UtilityContainerEntities utilityContainerContext)
        {
            _logger.Log("Running " + MethodBase.GetCurrentMethod().Name);
            var resourceTypes = enableContext.vwExt_ResourceType.ToList();
            var enableResourceTypes = _mappingService.Map<vwExt_ResourceType[], ResourceType[]>(resourceTypes.ToArray()).ToList();
            var utilityResourceTypeIds = utilityContainerContext.ResourceTypes.Select(c => c.ID).ToList();
            using (var transaction = utilityContainerContext.Database.BeginTransaction())
            {
                try
                {
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[ResourceType] ON");
                    utilityContainerContext.AddAll(enableResourceTypes.Where(e => !utilityResourceTypeIds.Contains(e.ID)));
                    utilityContainerContext.UpdateAll(enableResourceTypes.Where(e => utilityResourceTypeIds.Contains(e.ID)));
                    utilityContainerContext.SaveChanges();
                    utilityContainerContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[ResourceType] OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.Log(ex); 
                    transaction.Rollback();
                }
            }
        }

    }
}
