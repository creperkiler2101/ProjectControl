using ProjectControl.Models.Contexts;
using ProjectControl.Models.Rest.Responce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProjectControl.Models;
using System.Diagnostics;

namespace ProjectControl.Controllers
{
    public class ProjectsController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        private bool IsNotAccessable()
        {
            return UserControll.LoggedAs == null || !UserControll.LoggedAs.IsAdmin;
        }

        public ProjectResponce Get()
        {
            ProjectResponce responce = new ProjectResponce();
            responce.StatusCode = 200;

            if (IsNotAccessable())
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                responce.Projects.AddRange(db.Projects);
            }

            return responce;
        }

        public ProjectResponce Get(int id)
        {
            ProjectResponce responce = new ProjectResponce();
            responce.StatusCode = 200;

            if (IsNotAccessable())
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                responce.Projects.AddRange(db.Projects.Where(x => x.Id == id));
            }

            return responce;
        }

        public ProjectResponce Get(string title, string creator, bool? isCanAdd)
        {
            ProjectResponce responce = new ProjectResponce();
            responce.StatusCode = 200;

            if (IsNotAccessable())
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(title))
                    title = "";
                if (string.IsNullOrWhiteSpace(creator))
                    creator = "";

                if (isCanAdd != null)
                    responce.Projects.AddRange(db.Projects.ToList().Where(x => x.Name.Contains(title) && x.CreatorLogin.Contains(creator) && x.IsUsersCanAddTask == isCanAdd));
                else
                    responce.Projects.AddRange(db.Projects.Where(x => x.Name.Contains(title) && x.CreatorLogin.Contains(creator)));
            }

            return responce;
        }

        public ProjectResponce Post(Project _project)
        {
            ProjectResponce responce = new ProjectResponce();
            responce.StatusCode = 200;

            if (IsNotAccessable())
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(_project.Name))
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Name is required");
                }
                if (_project.StartTime == null)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Start time is required");
                }
                if (_project.EndTime == null)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("End time is required");
                }
                if (_project.StartTime > _project.EndTime)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Start time need to be less than end time");
                }

                if (responce.StatusCode == 200)
                {
                    _project.CreatorLogin = UserControll.LoggedAs.Login;
                    _project.CreatedTime = DateTime.Now;

                    db.Projects.Add(_project);
                    db.SaveChanges();
                }
            }

            return responce;
        }

        public ProjectResponce Put(Project _project)
        {
            ProjectResponce responce = new ProjectResponce();
            responce.StatusCode = 200;

            if (IsNotAccessable())
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(_project.Name))
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Title is required");
                }
                if (_project.StartTime == null)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Start time is required");
                }
                if (_project.EndTime == null)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("End time is required");
                }
                if (_project.StartTime > _project.EndTime)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Start time need to be less than end time");
                }

                if (responce.StatusCode == 200)
                {
                    Project project = db.Projects.Where(x => x.Id == _project.Id).FirstOrDefault();
                    project.Name = _project.Name;
                    project.StartTime = _project.StartTime;
                    project.EndTime = _project.EndTime;

                    db.SaveChanges();
                }
            }

            return responce;
        }

        public ProjectResponce Delete(int id)
        {
            ProjectResponce responce = new ProjectResponce();
            responce.StatusCode = 200;

            if (IsNotAccessable())
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                Project project = db.Projects.Where(x => x.Id == id).FirstOrDefault();
                if (project == null)
                    return responce;

                db.Projects.Remove(project);
                db.SaveChanges();
            }

            return responce;
        }
    }
}