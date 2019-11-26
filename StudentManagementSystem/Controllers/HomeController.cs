using StudentManagementSystem.DB;
using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Login";
            if (Session["StudentId"] != null && Session["EmailId"] != null)
            {
                if (Convert.ToString(Session["Role"]).Contains("Admin"))
                {
                    return RedirectToAction("AdminDeshBoard", "Home");
                }
                else
                {
                    return RedirectToAction("UserProfile", "Home");
                }
            }
            else
            {
                return View();
            }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Index(LoginCheckVM log)
        {
            if (string.IsNullOrEmpty(log.EmailId))
            {
                ModelState.AddModelError("EmailId", "EmailId is required!");
            }
            if (string.IsNullOrEmpty(log.Password))
            {
                ModelState.AddModelError("Password", "Password is required!");
            }
            if (ModelState.IsValid)
                {
                List<LoginCheckVM> SubList = new List<LoginCheckVM>();
                using (StudentsEntities context = new StudentsEntities())
                    {
                     var result = context.LoginCheck(log.EmailId.Trim(), log.Password.Trim()).ToList();
                    if (result.Count > 0)
                    {
                        foreach (var item in result.ToList())
                        {
                            SubList.Add(new LoginCheckVM
                            {
                                StudentId = item.StudentId,
                                FirstName = item.FirstName,
                                LastName = item.LastName,
                                EmailId = item.EmailId,
                                Role = item.Role
                            });
                        }
                        Session["StudentId"] = SubList[0].StudentId <= 0 ? 0 : SubList[0].StudentId;
                        Session["FirstName"] = SubList[0].FirstName;
                        Session["LastName"] = SubList[0].LastName;
                        Session["EmailId"] = SubList[0].EmailId;
                        Session["Role"] = SubList[0].Role;
                        TempData["UserName"] = SubList[0].FirstName + " " + SubList[0].LastName;
                        TempData["StudentId"] = SubList[0].StudentId;
                        if (SubList[0].Role.Contains("Admin"))
                        {
                            return RedirectToAction("AdminDeshBoard", "Home");
                        }
                        else
                        {
                            return RedirectToAction("UserProfile", "Home");
                        }
                    }
                    else
                    {
                        return RedirectToAction("index", "Home");
                    }
                }
            }
            return View();
        }

        public ActionResult AdminDeshBoard()
        {
            ViewBag.Title = "Admin DeshBoard";
            if (Session["StudentId"] != null && Session["EmailId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult UserProfile()
        {
            ViewBag.Title = "Student Profile";
            if (Session["StudentId"] != null && Session["EmailId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult StudentRegistration()
        {
            ViewBag.Title = "Student Registration";
            return View();
        }
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Logout()
        {
           Session.Clear();
           Session.Abandon();
           Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
           return RedirectToAction("Index", "Home");
        }
    }
}
