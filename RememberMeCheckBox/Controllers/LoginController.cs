using RememberMeCheckBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RememberMeCheckBox.Controllers
{
    public class LoginController : Controller
    {
        LoginDBEntities db = new LoginDBEntities();

        // GET: Login
        public ActionResult Index()
        {
            HttpCookie c = Request.Cookies["Usersname"];
            if (c != null)
            {
                ViewBag.username = c["uname"].ToString();

                string EncryptPassword = c["pass"].ToString();
                byte[] b = Convert.FromBase64String(EncryptPassword);
                string decryptPassword = ASCIIEncoding.ASCII.GetString(b);
                ViewBag.password = decryptPassword.ToString();
            }
            return View();
        }


        [HttpPost]
        public ActionResult Index(User u)
        {
            if(ModelState.IsValid == true)
            {
                HttpCookie cookie = new HttpCookie("Usersname");
                if (u.RememberMe == true)
                {                   
                    cookie["uname"] = u.UserName;

                    byte[] b = ASCIIEncoding.ASCII.GetBytes(u.Password);
                    string EncryptedPass = Convert.ToBase64String(b);
                    cookie["pass"] = EncryptedPass;

                    cookie.Expires = DateTime.Now.AddDays(2);
                    HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Response.Cookies.Add(cookie);
                }

                var row = db.Users.Where(model => model.UserName == u.UserName && model.Password == u.Password);
                if (row != null)
                {
                    Session["UserName"] = u.UserName;
                    TempData["Message"] = "<script>alert('Login Successed!!')</script>";
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    TempData["Message"] = "<script>alert('Login Failed!!')</script>";
                }
            }
            return View();
        }
    }
}