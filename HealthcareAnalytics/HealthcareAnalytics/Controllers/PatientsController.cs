using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HealthcareAnalytics.Models;
using System.ComponentModel;

namespace HealthcareAnalytics.Controllers
{
    public class PatientsController : Controller
    {
        private HospitalDBContext db = new HospitalDBContext();

        // GET: Patients
        public ActionResult Index([DefaultValue(1)] int id)
        {
            int pageNumber = 1;

            pageNumber = (id > 0) ? id : 1;

            int numberOfObjectsPerPage = 10;

            int total = db.Patients.Include(p => p.HomeContactInfo).Include(p => p.NameDetails).Include(p => p.WorkContactInfo).Count();
            var people = db.Patients.Include(p => p.HomeContactInfo).Include(p => p.NameDetails).Include(p => p.WorkContactInfo)
                .OrderBy(e => e.ID)
                .Skip(numberOfObjectsPerPage * (pageNumber - 1))
                .Take(numberOfObjectsPerPage);

            ViewBag.PageNumber = pageNumber;
            ViewBag.ResultsPerPage = numberOfObjectsPerPage;
            ViewBag.Total = total;

            return View(people.ToList());
        }

        // GET: Patients/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = (Patient)db.People.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            ViewBag.HomeContactInfoId = new SelectList(db.ContactInformations, "ID", "Street");
            ViewBag.NameDetailsId = new SelectList(db.NameDetails, "ID", "Title");
            ViewBag.WorkContactInfoId = new SelectList(db.ContactInformations, "ID", "Street");
            ViewBag.BranchId = new SelectList(db.Branches, "ID", "BranchName");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DateOfBirth,Gender,NameDetailsId,HomeContactInfoId,WorkContactInfoId,MedicareCardNumber,BranchId")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.ID = Guid.NewGuid();
                db.People.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HomeContactInfoId = new SelectList(db.ContactInformations, "ID", "Street", patient.HomeContactInfoId);
            ViewBag.NameDetailsId = new SelectList(db.NameDetails, "ID", "Title", patient.NameDetailsId);
            ViewBag.WorkContactInfoId = new SelectList(db.ContactInformations, "ID", "Street", patient.WorkContactInfoId);
            ViewBag.BranchId = new SelectList(db.Branches, "ID", "BranchName", patient.BranchId);
            return View(patient);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = (Patient)db.People.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.HomeContactInfoId = new SelectList(db.ContactInformations, "ID", "Street", patient.HomeContactInfoId);
            ViewBag.NameDetailsId = new SelectList(db.NameDetails, "ID", "Title", patient.NameDetailsId);
            ViewBag.WorkContactInfoId = new SelectList(db.ContactInformations, "ID", "Street", patient.WorkContactInfoId);
            ViewBag.BranchId = new SelectList(db.Branches, "ID", "BranchName", patient.BranchId);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DateOfBirth,Gender,NameDetailsId,HomeContactInfoId,WorkContactInfoId,MedicareCardNumber,BranchId")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HomeContactInfoId = new SelectList(db.ContactInformations, "ID", "Street", patient.HomeContactInfoId);
            ViewBag.NameDetailsId = new SelectList(db.NameDetails, "ID", "Title", patient.NameDetailsId);
            ViewBag.WorkContactInfoId = new SelectList(db.ContactInformations, "ID", "Street", patient.WorkContactInfoId);
            ViewBag.BranchId = new SelectList(db.Branches, "ID", "BranchName", patient.BranchId);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = (Patient)db.People.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Patient patient = (Patient)db.People.Find(id);
            db.People.Remove(patient);
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
