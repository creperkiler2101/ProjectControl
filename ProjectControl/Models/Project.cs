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

        public string StartTimeString {
            get {
                return StartTime?.ToString("yyyy-MM-dd hh:mm");
            }
        }

        public string EndTimeString {
            get {
                return EndTime?.ToString("yyyy-MM-dd hh:mm");
            }
        }

        public string CreatedTimeString {
            get {
                return CreatedTime?.ToString("yyyy-MM-dd hh:mm");
            }
        }

        public string Status
        {
            get
            {
                if (DateTime.Now < StartTime)
                    return "Not started yet";
                else if (DateTime.Now > StartTime && DateTime.Now < EndTime)
                    return "In work";
                else
                    return "Ended";
            }
        }

        public bool IsUsersCanAddTask
        {
            get
            {
                return DateTime.Now < EndTime;
            }
        }
    }
}