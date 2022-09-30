using CentricProjectTeam4.DAL;
using CentricProjectTeam4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CentricProjectTeam4.Controllers
{
    public class HomeController : Controller
    {
        private MyContext db = new MyContext();
        public ActionResult Index(string homeSearch)
        {
            var records = db.RegisteredUsers.AsQueryable();

            if (!string.IsNullOrEmpty(homeSearch))
            {
                records = records.Where(x => x.lastName.Contains(homeSearch) || x.firstName.Contains(homeSearch));
                return View("https://aspnet.cob.ohio.edu/mis4200team4/RegisteredUser/", records.ToList());
            }

            return View();
        }
        //public ActionResult About()
        //{
        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //   return View();
        //}
    }
}