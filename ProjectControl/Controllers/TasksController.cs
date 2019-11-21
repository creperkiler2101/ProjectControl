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

        public TaskResponce Get(bool getAll = true)
        {
            TaskResponce responce = new TaskResponce();
            responce.StatusCode = 200;

            User user = UserControll.LoggedAs;

            if (user == null)
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                if (user.IsAdmin && getAll)
                    responce.Tasks.AddRange(db.Tasks);
                else
                    responce.Tasks.AddRange(db.Tasks.Where(x => x.CreatorLogin == user.Login));
            }

            return responce;
        }

        public TaskResponce Get(int id)
        {
            TaskResponce responce = new TaskResponce();
            User user = UserControll.LoggedAs;
            responce.StatusCode = 200;

            if (user == null)
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                Task task = db.Tasks.Where(x => x.Id == id).FirstOrDefault();
                if (task.CreatorLogin != user.Login && !user.IsAdmin)
                {
                    responce.StatusCode = 403;
                    responce.ErrorMessage.Add("No access rights");

                    return responce;
                }

                responce.Tasks.Add(task);
            }

            return responce;
        }

        public TaskResponce Get(string title, string status, int projectId = -1, string creator = "", bool getAll = true)
        {
            TaskResponce responce = new TaskResponce();
            User user = UserControll.LoggedAs;
            responce.StatusCode = 200;

            if (user == null)
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
                {
                    if (user.IsAdmin && getAll)
                        responce.Tasks.AddRange(db.Tasks.ToList().Where(x => x.Name.Contains(title) && x.CreatorLogin.Contains(creator) && x.Status.Contains(status)));
                    else
                        responce.Tasks.AddRange(db.Tasks.ToList().Where(x => x.Name.Contains(title) && x.Status.Contains(status) && x.CreatorLogin == user.Login));
                }
                else
                {
                    if (user.IsAdmin && getAll)
                        responce.Tasks.AddRange(db.Tasks.ToList().Where(x => x.Name.Contains(title) && x.CreatorLogin.Contains(creator) && x.Status.Contains(status) && x.ProjectId == projectId));
                    else
                        responce.Tasks.AddRange(db.Tasks.ToList().Where(x => x.Name.Contains(title) && x.Status.Contains(status) && x.ProjectId == projectId && x.CreatorLogin == user.Login));
                }
            }

            return responce;
        }

        public TaskResponce Post(Task _task, bool autoStatus = true)
        {
            TaskResponce responce = new TaskResponce();
            Project _project = db.Projects.Where(x => x.Id == _task.ProjectId).FirstOrDefault();
            User user = UserControll.LoggedAs;
            responce.StatusCode = 200;

            if (user == null)
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
                    if (autoStatus)
                        _task.IsAccepted = null;

                    db.Tasks.Add(_task);
                    db.SaveChanges();
                }
            }

            return responce;
        }

        public TaskResponce Put(Task _task, string decline = null)
        {
            TaskResponce responce = new TaskResponce();
            Task task = db.Tasks.Where(x => x.Id == _task.Id).FirstOrDefault();
            User user = UserControll.LoggedAs;
            Project _project = db.Projects.Where(x => x.Id == task.ProjectId).FirstOrDefault();
            responce.StatusCode = 200;

            if (user == null)
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                if (user.Login != task.CreatorLogin && !user.IsAdmin)
                {
                    responce.StatusCode = 403;
                    responce.ErrorMessage.Add("No access rights");

                    return responce;
                }

                if (string.IsNullOrWhiteSpace(_task.Name))
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("Title is required");
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
                if (_task.StartTime > _task.EndTime)
                {
                    responce.StatusCode = 400;
                    responce.ErrorMessage.Add("End time is less than start time");
                }

                if (responce.StatusCode == 200)
                {
                    task.Name = _task.Name;
                    task.StartTime = _task.StartTime;
                    task.EndTime = _task.EndTime;
                    task.ProjectId = _task.ProjectId;
                    task.IsAccepted = _task.IsAccepted;
                    task.Description = _task.Description;

                    if (decline != null && _task.IsAccepted == false)
                    {
                        Email.Send(db.Users.Where(x => x.Login == task.CreatorLogin).FirstOrDefault().Email, "Task " + task.Name + " declined!", "<h1>Your task declined cuz this:</h1><div>" + decline + "</div>");
                    }

                    db.SaveChanges();
                }
            }

            return responce;
        }

        public TaskResponce Delete(int id)
        {
            TaskResponce responce = new TaskResponce();
            User user = UserControll.LoggedAs;
            responce.StatusCode = 200;

            if (user == null)
            {
                responce.StatusCode = 403;
                responce.ErrorMessage.Add("No access rights");
            }
            else
            {
                Task task = db.Tasks.Where(x => x.Id == id).FirstOrDefault();
                if (task == null)
                    return responce;

                if (user.Login != task.CreatorLogin && !user.IsAdmin)
                {
                    responce.StatusCode = 403;
                    responce.ErrorMessage.Add("No access rights");

                    return responce;
                }

                db.Tasks.Remove(task);
                db.SaveChanges();
            }

            return responce;
        }
    }
}