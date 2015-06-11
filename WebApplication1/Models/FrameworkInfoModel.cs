using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FrameworkInfoModel
    {
        public List<FrameworkModel> Frameworks {get; set;}
    }
    public class FrameworkModel
    {
        public string Name{get; set;}
        public double UsePersent{get; set;}
        public int MatchCount { get; set; }
    }
}