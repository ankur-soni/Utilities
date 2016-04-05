﻿using System.ComponentModel.DataAnnotations;


namespace Silicus.UtilityContainer.Models.DataObjects
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
}
