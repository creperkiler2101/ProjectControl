using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectControl.Models.Rest.Responce
{
    public class TaskResponce
    {
        public int StatusCode { get; set; }
        public List<string> ErrorMessage { get; set; } = new List<string>();
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}