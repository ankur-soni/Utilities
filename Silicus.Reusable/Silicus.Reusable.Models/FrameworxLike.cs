﻿using System;
using System.ComponentModel.DataAnnotations;
using Silicus.FrameworxProject.Models;

namespace Silicus.FrameworxProject.Models
{
    public class FrameworxLike
    {
        public int FrameworxId { get; set; }
        public int UserId { get; set; }
        // This to get the user details

        [Key]
        public int Id { get; set; }

        public virtual Frameworx Frameworx { get; set; }
    }
}
