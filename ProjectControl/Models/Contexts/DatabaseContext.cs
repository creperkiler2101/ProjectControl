using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace ProjectControl.Models.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }

        public DatabaseContext() : base("DbContext") {
            if (!Database.Exists())
                Database.Create();

            if (Roles.Where(x => x.Name == "User").FirstOrDefault() == null)
            {
                Roles.Add(new Role()
                {
                    Name = "User",
                    AccessLevel = 1
                });

                Roles.Add(new Role()
                {
                    Name = "Admin",
                    AccessLevel = 2
                });
                SaveChanges();
            }

            if (Users.Where(x => x.Login == "admin").FirstOrDefault() == null)
            {
                Users.Add(new User()
                {
                    Login = "admin",
                    Password = Hash.ComputeSha256Hash("admin"),
                    RoleId = 2,
                    IsActivated = true,
                    Name = "UMP",
                    SecondName = "9",
                    Email = "slimik043568@gmail.com",
                });

                Users.Add(new User()
                {
                    Login = "creperkiler2101",
                    Password = Hash.ComputeSha256Hash("1234"),
                    RoleId = 1,
                    IsActivated = true,
                    Name = "Ruslan",
                    SecondName = "Kudrjavtsev",
                    Email = "creperkiler2101@mail.ru",
                }); 

                SaveChanges();
            }

            if (Projects.Count() == 0)
            {
                Projects.Add(new Project()
                {
                    Name = "ProjName",
                    CreatorLogin = "admin",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddYears(1),
                    CreatedTime = DateTime.Now
                });

                Projects.Add(new Project()
                {
                    Name = "ProjName1",
                    CreatorLogin = "admin",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddYears(1),
                    CreatedTime = DateTime.Now
                });

                Projects.Add(new Project()
                {
                    Name = "ProjName2",
                    CreatorLogin = "admin",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddYears(1),
                    CreatedTime = DateTime.Now
                });

                Projects.Add(new Project()
                {
                    Name = "ProjName3",
                    CreatorLogin = "admin",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddYears(1),
                    CreatedTime = DateTime.Now
                });
                SaveChanges();
            }
        }
    }
}