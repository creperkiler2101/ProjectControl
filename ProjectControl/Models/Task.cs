using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ProjectControl.Models.Contexts;

namespace ProjectControl.Models
{
    [Table("tblTasks")]
    public class Task
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }

        public string Name { get; set; }
        public string CreatorLogin { get; set; }
        public string Description { get; set; }

        public bool? IsAccepted { get; set; }

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

        public string Status {
            get {
                if (IsAccepted == true)
                {
                    if (DateTime.Now < EndTime && DateTime.Now > StartTime)
                        return "In work";
                    else if (DateTime.Now > EndTime)
                        return "Completed";
                    else
                        return "Accepted";
                }
                else if (IsAccepted == false)
                    return "Declined";
                else
                    return "Waiting...";
            }
        }

        public string ProjectName {
            get {
                DatabaseContext db = new DatabaseContext();
                return db.Projects.Where(x => x.Id == ProjectId).FirstOrDefault().Name;
            }
        }
    }
}