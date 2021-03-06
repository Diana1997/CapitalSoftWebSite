﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapitalSoftWebSite.Models
{
    public class Technology
    {
        public int TechnologyID { set; get; }
        [Required]
        public string Name { set; get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Technology that = obj as Technology;
            if (that == null)
                return false;
            return this.TechnologyID == that.TechnologyID && this.Name == that.Name;
        }

        public override int GetHashCode()
        {
            var hashCode = 1107042776;
            hashCode = hashCode * -1521134295 + TechnologyID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}