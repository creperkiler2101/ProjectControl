using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProjectControl.Models;
using ProjectControl.Models.Contexts;
using ProjectControl.Models.Rest.Responce;

namespace ProjectControl.Controllers
{
    public class TasksController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        private bool IsNotAccessable()
        {
            return UserControll.LoggedAs == null || !UserControll.LoggedAs.IsAdmin;
        }

        public TaskResponce Get()
        {
            TaskResponce responce = new TaskResponce();
            responce.StatusCode = 200;

            if (IsNotAccessable())
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                responce.Tasks.AddRange(db.Tasks);
            }

            return responce;
        }

        public TaskResponce Get(int id)
        {
            TaskResponce responce = new TaskResponce();
            responce.StatusCode = 200;

            if (IsNotAccessable())
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                responce.Tasks.AddRange(db.Tasks.Where(x => x.Id == id));
            }

            return responce;
        }

        public TaskResponce Get(string title, string creator, string status, int projectId = -1)
        {
            TaskResponce responce = new TaskResponce();
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
                if (string.IsNullOrWhiteSpace(status) || status == "Any")
                    status = "";

                if (projectId == -1)
                    responce.Tasks.AddRange(db.Tasks.ToList().Where(x => x.Name.Contains(title) && x.CreatorLogin.Contains(creator) && x.Status.Contains(status)));
                else
                    responce.Tasks.AddRange(db.Tasks.ToList().Where(x => x.Name.Contains(title) && x.CreatorLogin.Contains(creator) && x.Status.Contains(status) && x.ProjectId == projectId));
            }

            return responce;
        }

        public TaskResponce Post(Task _task)
        {
            TaskResponce responce = new TaskResponce();
            Project _project = db.Projects.Where(x => x.Id == _task.ProjectId).FirstOrDefault();
            responce.StatusCode = 200;

            if (IsNotAccessable())
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(_task.Name))
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Name is required");
                }
                if (_task.StartTime == null)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Start time is required");
                }
                if (_task.EndTime == null)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("End time is required");
                }
                if (_task.StartTime > _task.EndTime)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Start time need to be less than end time");
                }
                if (_project == null)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Project is required");
                }
                else if (_project.StartTime > _task.StartTime || _project.EndTime < _task.EndTime)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Task time is not in bounds of project time");
                }

                if (responce.StatusCode == 200)
                {
                    _task.CreatorLogin = UserControll.LoggedAs.Login;
                    _task.CreatedTime = DateTime.Now;

                    db.Tasks.Add(_task);
                    db.SaveChanges();
                }
            }

            return responce;
        }

        public TaskResponce Put(Task _task)
        {
            TaskResponce responce = new TaskResponce();
            Task task = db.Tasks.Where(x => x.Id == _task.Id).FirstOrDefault();
            Project _project = db.Projects.Where(x => x.Id == task.ProjectId).FirstOrDefault();
            responce.StatusCode = 200;

            if (IsNotAccessable())
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(_task.Name))
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Title is required");
                }
                else if (_task.Name.Length > 10)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Title max length is 10");
                }

                if (_task.StartTime == null)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Start time is required");
                }
                if (_task.EndTime == null)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("End time is required");
                }
                if (_project.StartTime > _task.StartTime || _project.EndTime < _task.EndTime)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Task time is not in bounds of project time");
                }

                if (responce.StatusCode == 200)
                {
                    task.Name = _task.Name;
                    task.StartTime = _task.StartTime;
                    task.EndTime = _task.EndTime;
                    task.ProjectId = _task.ProjectId;
                    task.IsAccepted = _task.IsAccepted;
                    task.Description = _task.Description;

                    db.SaveChanges();
                }
            }

            return responce;
        }

        public TaskResponce Delete(int id)
        {
            TaskResponce responce = new TaskResponce();
            responce.StatusCode = 200;

            if (IsNotAccessable())
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                Task task = db.Tasks.Where(x => x.Id == id).FirstOrDefault();
                if (task == null)
                    return responce;

                db.Tasks.Remove(task);
                db.SaveChanges();
            }

            return responce;
        }
    }
}