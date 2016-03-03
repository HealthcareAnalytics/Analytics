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
using HealthcareAnalytics.ViewModels;

namespace HealthcareAnalytics.Controllers
{
    public class PatientsController : Controller
    {
        private HospitalDBContext db = new HospitalDBContext();

        // GET: Patients
        public ActionResult Index([DefaultValue(1)] int id)
        {
            var currentPage = (id > 0) ? id : 1;
            int pageSize = 10;

            int total = db.Patients.Include(p => p.HomeContactInfo).Include(p => p.NameDetails).Include(p => p.WorkContactInfo).Count();

            var totalPages = (int)Math.Ceiling((decimal)total / (decimal)pageSize);
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            var patients = db.Patients
                .Include(p => p.NameDetails)
                .OrderBy(e => e.NameDetails.FirstName)
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize);

            ViewBag.TotalItems = total;
            ViewBag.CurrentPage = currentPage;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;

            return View(patients.ToList());
        }

        // GET: Patients/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients
                .Include(p => p.HomeContactInfo)
                .Include(p => p.WorkContactInfo)
                .Include(p => p.NameDetails)
                .Include(p => p.ChecckinDetails)
                .SingleOrDefault(p => p.ID == id);

            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(new PatientsDetailsViewModel() {Patient = patient});
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
        public ActionResult Create([Bind(Include = "ID,DateOfBirth,Gender,NameDetails,HomeContactInfo,WorkContactInfo,MedicareCardNumber,BranchId")] Patient patient)
        {

            patient.WorkContactInfoId = patient.WorkContactInfo.ID;
            patient.HomeContactInfoId = patient.HomeContactInfo.ID;
            patient.NameDetailsId = patient.NameDetails.ID;

            if (ModelState.IsValid)
            {
                db.ContactInformations.Add(patient.WorkContactInfo);
                db.SaveChanges();

                db.ContactInformations.Add(patient.HomeContactInfo);
                db.SaveChanges();

                db.NameDetails.Add(patient.NameDetails);
                db.SaveChanges();

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
