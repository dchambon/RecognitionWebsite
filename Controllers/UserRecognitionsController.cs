using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using CentricProjectTeam4.DAL;
using CentricProjectTeam4.Models;
using Microsoft.AspNet.Identity;

namespace CentricProjectTeam4.Controllers
{
    public class UserRecognitionsController : Controller
    {
        private MyContext db = new MyContext();

        // GET: UserRecognitions
        [Authorize]
        public ActionResult Index(string search, string sortOrder)
        {
            // Search section
            var records = db.UserRecognitions.AsQueryable();

            records = records.Include(u => u.registeredUser);

            if (!string.IsNullOrEmpty(search))
            {
                records = records.Where(x => x.fullName.Contains(search) || x.recGiver.Contains(search));
            }

            // Sorting section
            ViewBag.EmployeeSortParm = String.IsNullOrEmpty(sortOrder) ? "registeredUser.fullName" : "";
            ViewBag.SponsorSortParm = String.IsNullOrEmpty(sortOrder) ? "recGiver" : "";
            ViewBag.CreateDateSortParm = sortOrder == "createDate" ? "createDate_desc" : "createDate";

            switch (sortOrder)
            {
                case "registeredUser.fullName":
                    records = records.OrderBy(s => s.fullName);
                    break;
                case "recGiver":
                    records = records.OrderBy(s => s.recGiver);
                    break;
                case "createDate":
                    records = records.OrderBy(s => s.createDate);
                    break;
                case "createDate_desc":
                    records = records.OrderByDescending(s => s.createDate);
                    break;
                default:
                    records = records.OrderBy(s => s.fullName);
                    break;
            }

            return View(records.ToList());
        }

        // GET: UserRecognitions/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRecognition userRecognition = db.UserRecognitions.Find(id);
            if (userRecognition == null)
            {
                return HttpNotFound();
            }
            return View(userRecognition);
        }

        // GET: UserRecognitions/Create
        [Authorize]
        public ActionResult Create()
        {
            var currentUserID = Guid.Parse(User.Identity.GetUserId());
            var tempList = db.RegisteredUsers.Where(x => x.registeredUserID != currentUserID).OrderBy(x => x.lastName).ToList();

            ViewBag.registeredUserID = new SelectList(tempList, "registeredUserID", "fullName");
            return View();
        }

        // POST: UserRecognitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "recognitionID,registeredUserID,recGiver,coreValue,recognition,createDate,fullName")] UserRecognition userRecognition)
        {
            var currentUserID = Guid.Parse(User.Identity.GetUserId());
            RegisteredUser currentUser = db.RegisteredUsers.Find(currentUserID);
            RegisteredUser receiver = db.RegisteredUsers.Find(userRecognition.registeredUserID);


            if (ModelState.IsValid)
            {
                userRecognition.recGiver = currentUser.fullName;
                userRecognition.createDate = DateTime.Now;
                userRecognition.fullName = receiver.fullName;

                db.UserRecognitions.Add(userRecognition);
                db.SaveChanges();

                // To send email
                try
                {
                    var name = receiver.fullName;
                    var email = receiver.email;
                    var giver = currentUser.fullName;
                    var value = userRecognition.coreValue;
                    var recognition = userRecognition.recognition;
                    var message = "Hi " + name + ",\n\nYou have received a recognition from " + giver + "!";
                    message += "\n\nYou are being recognized for " + value + "!";
                    message += " Here is the recognition you were given: \n\"" + recognition + "\"";
                    message += "\n\nYou can see more details on the Centric recognitions website.";
                    message += "\n\nSincerely,\n\nCentric Consulting's Recognition Team";

                    MailMessage myMessage = new MailMessage();
                    MailAddress from = new MailAddress("centricrecognitions@gmail.com", "Centric Consulting Recognition Team");
                    myMessage.From = from;
                    myMessage.To.Add(email);
                    myMessage.Subject = "You have received a recognition!";
                    myMessage.Body = message;


                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("centricrecognitions@gmail.com", "OHIOtest!23");
                    smtp.EnableSsl = true;
                    smtp.Send(myMessage);
                    TempData["mailError"] = "";
                }
                catch (Exception ex)
                {
                    TempData["mailError"] = ex.Message;
                    return View("mailError");
                }

                return RedirectToAction("Index");
            }

            var tempList = db.RegisteredUsers.Where(x => x.registeredUserID != currentUserID).OrderBy(x => x.lastName).ToList();

            ViewBag.registeredUserID = new SelectList(tempList, "registeredUserID", "fullName", userRecognition.registeredUserID);
            return View(userRecognition);
        }

        // GET: UserRecognitions/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRecognition userRecognition = db.UserRecognitions.Find(id);
            if (userRecognition == null)
            {
                return HttpNotFound();
            }

            var currentUserID = Guid.Parse(User.Identity.GetUserId());
            RegisteredUser currentUser = db.RegisteredUsers.Find(currentUserID);

            if (currentUser.fullName == userRecognition.recGiver)
            {
                var tempList = db.RegisteredUsers.Where(x => x.registeredUserID != currentUserID).OrderBy(x => x.lastName).ToList();
                ViewBag.registeredUserID = new SelectList(tempList, "registeredUserID", "fullName", userRecognition.registeredUserID);
                return View(userRecognition);
            }
            else
            {
                return View("NotAuthorized");
            }
        }

        // POST: UserRecognitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "recognitionID,registeredUserID,recGiver,coreValue,recognition,createDate,fullName")] UserRecognition userRecognition)
        {
            var currentUserID = Guid.Parse(User.Identity.GetUserId());
            RegisteredUser currentUser = db.RegisteredUsers.Find(currentUserID);
            RegisteredUser receiver = db.RegisteredUsers.Find(userRecognition.registeredUserID);

            if (ModelState.IsValid)
            {
                userRecognition.recGiver = currentUser.fullName;
                userRecognition.createDate = DateTime.Now;
                userRecognition.fullName = receiver.fullName;

                db.Entry(userRecognition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var tempList = db.RegisteredUsers.Where(x => x.registeredUserID != currentUserID).OrderBy(x => x.lastName).ToList();
            ViewBag.registeredUserID = new SelectList(tempList, "registeredUserID", "firstName", userRecognition.registeredUserID);
            return View(userRecognition);
        }

        // GET: UserRecognitions/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRecognition userRecognition = db.UserRecognitions.Find(id);
            if (userRecognition == null)
            {
                return HttpNotFound();
            }
            Guid registeredUserId;
            Guid.TryParse(User.Identity.GetUserId(), out registeredUserId);
            RegisteredUser currentUser = db.RegisteredUsers.Find(registeredUserId);
            bool isDirector = currentUser.role == RegisteredUser.roles.Director;
            if (isDirector || currentUser.fullName == userRecognition.recGiver)
            {
                return View(userRecognition);
            }
            else
            {
                return View("DirectorOnly");
            }
        }

        // POST: UserRecognitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserRecognition userRecognition = db.UserRecognitions.Find(id);
            db.UserRecognitions.Remove(userRecognition);
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
