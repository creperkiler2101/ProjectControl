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

        public bool IsAccepted { get; set; }

        public DateTime? CreatedTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string Status {
            get {
                if (IsAccepted)
                {
                    if (DateTime.Now < EndTime && DateTime.Now > StartTime)
                        return "In work";
                    else if (DateTime.Now > EndTime)
                        return "Completed";
                    else
                        return "Accepted";
                }
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