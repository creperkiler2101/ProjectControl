using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectControl.Models
{
    [Table("tblProjects")]
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatorLogin { get; set; }

        public DateTime? CreatedTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public bool IsUsersCanAddTask
        {
            get
            {
                return EndTime > DateTime.Now;
            }
        }
    }
}