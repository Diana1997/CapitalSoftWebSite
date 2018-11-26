using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CapitalSoftWebSite.Models
{
    public class Project
    {
        public Project()
        {
            Technologies = new List<Technology>();
        }
        public int ProjectID { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        [MaxLength(4000)]
        public string Description { set; get; }
        public ICollection<Image> Images { set; get; }
        public string Lang { set; get; }
        [MaxLength(4000)]
        public string LargeDesc { set; get; }
        [NotMapped]
        public virtual ICollection<Technology> Technologies { set; get; }
    }
}