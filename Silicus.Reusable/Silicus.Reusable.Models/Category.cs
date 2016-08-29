﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.FrameworxProject.Models
{
    public class Category
    {
        public Category()
        {
            Frameworxs = new List<Frameworx>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Frameworx> Frameworxs { get; set; }
    }
}
