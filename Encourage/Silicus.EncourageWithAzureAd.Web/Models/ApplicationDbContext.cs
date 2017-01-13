﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<UserTokenCache> UserTokenCacheList { get; set; }
    }

    public class UserTokenCache
    {
        [Key]
        public int UserTokenCacheId { get; set; }
        public string WebUserUniqueId { get; set; }
        public byte[] CacheBits { get; set; }
        public DateTime LastWrite { get; set; }
    }
}
