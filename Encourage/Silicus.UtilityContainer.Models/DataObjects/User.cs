﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Silicus.UtilityContainer.Models.DataObjects
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Initials { get; set; }
        public string EmailAddress { get; set; }
        public bool OverrideTimeZone { get; set; }
        public int TimezoneID { get; set; }
        public string EmployeeID { get; set; }
        public bool IsClient { get; set; }
        public int? ClientID { get; set; }
        public int PrimaryRoleID { get; set; }
        public int PrimaryRoleCostCenterID { get; set; }
        public string OfficePhone { get; set; }
        public string MobilePhone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public DateTime? ActiveDate { get; set; }
        public DateTime? InactiveDate { get; set; }
        public int IdentityProviderID { get; set; }
        public string ApplicationAuthUserID { get; set; }
    }
}
