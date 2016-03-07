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
   // [RoutePrefix("Employees")]
    public class EmployeesController : Controller
    {
        private HospitalDBContext db = new HospitalDBContext();

        // GET: Employees
        public ActionResult Index([DefaultValue(1)] int id) {
           
            var currentPage = (id > 0) ? id : 1;
            int pageSize = 10;

            int total = db.Employees.Include(e => e.HomeContactInfo).Include(e => e.NameDetails).Include(e => e.WorkContactInfo).Count();

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

            var employees = db.Employees
                .Include(e => e.NameDetails)
                .Include(e => e.EmploymentDetails)
                .Include(e => e.HomeContactInfo)
                .OrderBy(e => e.NameDetails.FirstName)
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize);

            ViewBag.TotalItems = total;
            ViewBag.CurrentPage = currentPage;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;

            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employee = db.Employees
                .Include(e => e.NameDetails)
                .Include(e => e.EmploymentDetails)
                .Include(e => e.HomeContactInfo)
                .Include(e => e.Branch)
                .SingleOrDefault(e => e.ID == id);

            if (employee == null)
            {
                return HttpNotFound();
            }

            return View(new EmployeesDetailsViewModel() {Employee = employee});
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.BranchId = new SelectList(db.Branches, "ID", "BranchName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DateOfBirth,Gender,NameDetails,HomeContactInfo,BranchId")] Employee employee)
        {
            employee.NameDetailsId = employee.NameDetails.ID;
            employee.HomeContactInfoId = employee.HomeContactInfo.ID;
            Branch branch = db.Branches.Include(b => b.ContactInformation).SingleOrDefault(b => b.ID == employee.BranchId);
            employee.WorkContactInfoId = branch.ContactInformation.ID;

            if (ModelState.IsValid)
            {
                //db.ContactInformations.Add(employee.HomeContactInfo);
                //db.SaveChanges();

                //db.NameDetails.Add(employee.NameDetails);
                //db.SaveChanges();

                db.People.Add(employee);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            
            ViewBag.BranchId = new SelectList(db.Branches, "ID", "BranchName", employee.BranchId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employee = db.Employees
                .Include(e => e.HomeContactInfo)
                .Include(e => e.WorkContactInfo)
                .Include(e => e.NameDetails)
                .Include(e => e.Branch)
                .SingleOrDefault(e => e.ID == id);

            if (employee == null)
            {
                return HttpNotFound();
            }
            
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DateOfBirth,Gender,NameDetails,NameDetailsId,HomeContactInfo,HomeContactInfoId,WorkContactInfo,WorkContactInfoId,BranchId")] Employee employee)
        {

            employee.NameDetails.ID = employee.NameDetailsId;
            employee.HomeContactInfo.ID = employee.HomeContactInfoId;

            if (ModelState.IsValid)
            {
                db.Entry(employee.NameDetails).State = EntityState.Modified;
                db.SaveChanges();

                db.Entry(employee.HomeContactInfo).State = EntityState.Modified;
                db.SaveChanges();

                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
       
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employee = db.Employees
                .Include(e => e.Branch)
                .Include(e => e.NameDetails)
                .SingleOrDefault(e => e.ID == id);

            if (employee == null)
            {
                return HttpNotFound();
            }

            Incident incident = db.Incidents.FirstOrDefault(i => i.EmployeeId == id);
            ViewBag.CanDelete = true;

            if (incident != null)
            {
                ViewBag.CanDelete = false;
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Incident incident = db.Incidents.FirstOrDefault(i => i.EmployeeId == id);

            if (incident != null)
            {
                return RedirectToAction("Delete");
            }

            Employee employee =  db.Employees
                .Include(e => e.NameDetails)
                .Include(e => e.HomeContactInfo)
                .Include(e => e.EmploymentDetails)
                .SingleOrDefault(e => e.ID == id);

            EmploymentDetails empDetails = db.EmploymentDetails
                .SingleOrDefault(ed => ed.EmployeeId == employee.ID);

            if (employee == null)
            {
                return HttpNotFound();
            }

            if (empDetails != null)
            {
                db.EmploymentDetails.Remove(empDetails);
                db.SaveChanges();
            }

            db.People.Remove(employee);
            db.SaveChanges();

            if (employee.HomeContactInfo != null)
            {
                db.ContactInformations.Remove(employee.HomeContactInfo);
                db.SaveChanges();
            }

            if (employee.NameDetails != null)
            {
                db.NameDetails.Remove(employee.NameDetails);
                db.SaveChanges();
            }

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
