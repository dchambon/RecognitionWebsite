using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CentricProjectTeam4.DAL;
using CentricProjectTeam4.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace CentricProjectTeam4.Controllers
{
    public class RegisteredUsersController : Controller
    {
        private MyContext db = new MyContext();

        // GET: RegisteredUsers
        [Authorize]
        public ActionResult Index(string search, string sortOrder)
        {
            // Search section
            var records = db.RegisteredUsers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                records = records.Where(x => x.lastName.Contains(search) || x.firstName.Contains(search));
            }

                // Sorting section
                ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "firstName" : "";
                ViewBag.LastNameSortParm = String.IsNullOrEmpty(sortOrder) ? "lastName" : "";
                ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title" : "";
                ViewBag.LocationSortParm = String.IsNullOrEmpty(sortOrder) ? "location" : "";
                ViewBag.HireDateSortParm = sortOrder == "hireDate" ? "hireDate_desc" : "hireDate";
              
                switch (sortOrder)
                {
                    case "firstName":
                        records = records.OrderBy(s => s.firstName);
                        break;
                    case "lastName":
                        records = records.OrderBy(s => s.lastName);
                        break;
                    case "hireDate":
                        records = records.OrderBy(s => s.hireDate);
                        break;
                    case "hireDate_desc":
                        records = records.OrderByDescending(s => s.hireDate);
                        break;
                    case "title":
                        records = records.OrderBy(s => s.role);
                        break;
                    case "location":
                        records = records.OrderBy(s => s.location);
                        break;
                    default:
                        records = records.OrderBy(s => s.lastName);
                        break;
                }

                return View(records.ToList());
        }

        // GET: RegisteredUsers/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredUser registeredUser = db.RegisteredUsers.Find(id);
            if (registeredUser == null)
            {
                return HttpNotFound();
            }
            return View(registeredUser);
        }

        // GET: RegisteredUsers/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegisteredUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "registeredUserID,firstName,lastName,phone,email,location,role,hireDate,profilePic")] RegisteredUser registeredUser)
        {
            if (ModelState.IsValid)
            {
                Guid newUser;
                Guid.TryParse(User.Identity.GetUserId(), out newUser);
                registeredUser.registeredUserID = newUser;
                registeredUser.email = User.Identity.Name;

                HttpPostedFileBase file = Request.Files["UploadedImage"];
                if (file != null && file.FileName != null && file.FileName != "")
                {
                    FileInfo fi = new FileInfo(file.FileName);
                    if (fi.Extension != ".png" && fi.Extension != ".jpg" && fi.Extension != ".gif")
                    {
                        ViewBag.Errormsg = "The file, " + file.FileName + ", does not have a valid image extension.";

                        return View(registeredUser);
                    }
                    else
                    {
                        registeredUser.profilePic = Guid.NewGuid().ToString() + fi.Extension;
                        file.SaveAs(Server.MapPath("~/Uploads/" + registeredUser.profilePic));
                    }
                }

                db.RegisteredUsers.Add(registeredUser);
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(registeredUser);
        }

        // GET: RegisteredUsers/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredUser registeredUser = db.RegisteredUsers.Find(id);
            if (registeredUser == null)
            {
                return HttpNotFound();
            }
            Guid registeredUserId;
            Guid.TryParse(User.Identity.GetUserId(), out registeredUserId);
            if (registeredUserId == id)
            {
                TempData["oldPhoto"] = registeredUser.profilePic;

                return View(registeredUser);
            }
            else
            {
                return View("NotAuthorized");
            }
        }

        // POST: RegisteredUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "registeredUserID,firstName,lastName,phone,email,location,role,hireDate,profilePic")] RegisteredUser registeredUser)
        {
            if (ModelState.IsValid)
            {
                registeredUser.email = User.Identity.Name;

                HttpPostedFileBase file = Request.Files["UploadedImage"];
                if (file != null && file.FileName != null && file.FileName != "")
                {
                    FileInfo fi = new FileInfo(file.FileName);
                    if (fi.Extension != ".png" && fi.Extension != ".jpg" && fi.Extension != ".gif")
                    {
                        ViewBag.Errormsg = "The file, " + file.FileName + ", does not have a valid image extension.";

                        return View(registeredUser);
                    }
                    else
                    {
                        if (TempData["oldPhoto"] != null)
                        {

                        string path = Server.MapPath("~/Uploads/" + TempData["oldPhoto"].ToString());
                        try
                        {
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            else
                            {
                                // must already be deleted
                            }
                        }
                        catch (Exception Ex)
                        {
                            ViewBag.deleteFailed = Ex.Message;
                            return View("Index");
                        }
                        }
                        if (fi.Name != null && fi.Name != "")
                        {
                            registeredUser.profilePic = Guid.NewGuid().ToString() + fi.Extension;
                            file.SaveAs(Server.MapPath("~/Uploads/" + registeredUser.profilePic));
                        }
                    }
                }
                else
                {
                    registeredUser.profilePic = TempData["oldPhoto"].ToString();
                }

                db.Entry(registeredUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(registeredUser);
        }

        // GET: RegisteredUsers/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredUser registeredUser = db.RegisteredUsers.Find(id);
            if (registeredUser == null)
            {
                return HttpNotFound();
            }
            Guid registeredUserId;
            Guid.TryParse(User.Identity.GetUserId(), out registeredUserId);
            RegisteredUser currentUser = db.RegisteredUsers.Find(registeredUserId);
            bool isDirector = currentUser.role == RegisteredUser.roles.Director;
            if (isDirector)
            {
                return View(registeredUser);
            }
            else
            {
                return View("DirectorOnly");
            }


            
        }

        // POST: RegisteredUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RegisteredUser registeredUser = db.RegisteredUsers.Find(id);
            string imageName = registeredUser.profilePic;
            string path = Server.MapPath("~/Uploads/" + imageName);
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                else
                {
                    // nothing, no error was found so no error
                }
            }
            catch (Exception Ex)
            {
                ViewBag.deleteFailed = Ex.Message;
                return View("Index");
            }

            db.RegisteredUsers.Remove(registeredUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

