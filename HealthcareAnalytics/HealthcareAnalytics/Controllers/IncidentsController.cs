using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HealthcareAnalytics.Models;

namespace HealthcareAnalytics.Controllers
{
    public class IncidentsController : Controller
    {
        private HospitalDBContext db = new HospitalDBContext();

        // GET: Incidents
        public async Task<ActionResult> Index([DefaultValue(1)] int id)
        {

            
            var currentPage = (id > 0) ? id : 1;
            int pageSize = 10;

            int total = db.Incidents.Count();

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

            var incidents = db.Incidents.Include(i => i.Branch).Include(i => i.Employee).Include(i => i.IncidentType).Include(i => i.Patient)
                .OrderBy(i => i.ID)
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize);

            ViewBag.TotalItems = total;
            ViewBag.CurrentPage = currentPage;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;

            return View(incidents.ToList());
        }

        // GET: Incidents/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = await db.Incidents.FindAsync(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // GET: Incidents/Create
        public ActionResult Create()
        {
            ViewBag.BranchId = new SelectList(db.Branches, "ID", "BranchName");
            ViewBag.EmployeeId = new SelectList(db.People, "ID", "Gender");
            ViewBag.IncidentTypeId = new SelectList(db.IncidentTypes, "ID", "Name");
            ViewBag.PatientId = new SelectList(db.People, "ID", "Gender");
            return View();
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,BranchId,EmployeeId,PatientId,IncidentTypeId,DateAndTime,Location,Details,FollowUpActions")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                incident.ID = Guid.NewGuid();
                db.Incidents.Add(incident);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BranchId = new SelectList(db.Branches, "ID", "BranchName", incident.BranchId);
            ViewBag.EmployeeId = new SelectList(db.People, "ID", "Gender", incident.EmployeeId);
            ViewBag.IncidentTypeId = new SelectList(db.IncidentTypes, "ID", "Name", incident.IncidentTypeId);
            ViewBag.PatientId = new SelectList(db.People, "ID", "Gender", incident.PatientId);
            return View(incident);
        }

        // GET: Incidents/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = await db.Incidents.FindAsync(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchId = new SelectList(db.Branches, "ID", "BranchName", incident.BranchId);
            ViewBag.EmployeeId = new SelectList(db.People, "ID", "Gender", incident.EmployeeId);
            ViewBag.IncidentTypeId = new SelectList(db.IncidentTypes, "ID", "Name", incident.IncidentTypeId);
            ViewBag.PatientId = new SelectList(db.People, "ID", "Gender", incident.PatientId);
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,BranchId,EmployeeId,PatientId,IncidentTypeId,DateAndTime,Location,Details,FollowUpActions")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incident).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BranchId = new SelectList(db.Branches, "ID", "BranchName", incident.BranchId);
            ViewBag.EmployeeId = new SelectList(db.People, "ID", "Gender", incident.EmployeeId);
            ViewBag.IncidentTypeId = new SelectList(db.IncidentTypes, "ID", "Name", incident.IncidentTypeId);
            ViewBag.PatientId = new SelectList(db.People, "ID", "Gender", incident.PatientId);
            return View(incident);
        }

        // GET: Incidents/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = await db.Incidents.FindAsync(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Incident incident = await db.Incidents.FindAsync(id);
            db.Incidents.Remove(incident);
            await db.SaveChangesAsync();
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
