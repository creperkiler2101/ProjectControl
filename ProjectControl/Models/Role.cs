using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectControl.Models
{
    [Table("tblRoles")]
    public class Role
    {
        public int Id { get; set; }
        public int AccessLevel { get; set; }
        public string Name { get; set; }
    }
}