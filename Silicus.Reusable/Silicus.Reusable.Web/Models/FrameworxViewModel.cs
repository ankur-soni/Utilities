﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Reusable.Web.Models
{
    public class FrameworxViewModel
    {

        public int id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string HtmlDescription { get; set; }

        public string DemoLink { get; set; }

        public string SourceCodeLink { get; set; }

        public int Likes { get; set; }

        public bool IsLiked { get; set; }
                
        public int? LikeId { get; set; }

        public int OwnerId { get; set; }

        public string Credits { get; set; }        
    }
}