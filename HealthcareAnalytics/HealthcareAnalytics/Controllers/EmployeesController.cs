using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HealthcareAnalytics.Models;
using PagedList;
using System.ComponentModel;

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

            var people = db.Employees.Include(e => e.HomeContactInfo).Include(e => e.NameDetails).Include(e => e.WorkContactInfo)
                .OrderBy(e => e.ID)
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize);

            ViewBag.TotalItems = total;
            ViewBag.CurrentPage = currentPage;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;

            return View(people.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = (Employee)db.People.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.HomeContactInfoId = new SelectList(db.ContactInformations, "ID", "Street");
            ViewBag.NameDetailsId = new SelectList(db.NameDetails, "ID", "Title");
            ViewBag.WorkContactInfoId = new SelectList(db.ContactInformations, "ID", "Street");
            ViewBag.BranchId = new SelectList(db.Branches, "ID", "BranchName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DateOfBirth,Gender,NameDetailsId,HomeContactInfoId,WorkContactInfoId,BranchId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.ID = Guid.NewGuid();
                db.People.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HomeContactInfoId = new SelectList(db.ContactInformations, "ID", "Street", employee.HomeContactInfoId);
            ViewBag.NameDetailsId = new SelectList(db.NameDetails, "ID", "Title", employee.NameDetailsId);
            ViewBag.WorkContactInfoId = new SelectList(db.ContactInformations, "ID", "Street", employee.WorkContactInfoId);
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
            Employee employee = (Employee) db.People.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.HomeContactInfoId = new SelectList(db.ContactInformations, "ID", "Street", employee.HomeContactInfoId);
            ViewBag.NameDetailsId = new SelectList(db.NameDetails, "ID", "Title", employee.NameDetailsId);
            ViewBag.WorkContactInfoId = new SelectList(db.ContactInformations, "ID", "Street", employee.WorkContactInfoId);
            ViewBag.BranchId = new SelectList(db.Branches, "ID", "BranchName", employee.BranchId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DateOfBirth,Gender,NameDetailsId,HomeContactInfoId,WorkContactInfoId,BranchId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HomeContactInfoId = new SelectList(db.ContactInformations, "ID", "Street", employee.HomeContactInfoId);
            ViewBag.NameDetailsId = new SelectList(db.NameDetails, "ID", "Title", employee.NameDetailsId);
            ViewBag.WorkContactInfoId = new SelectList(db.ContactInformations, "ID", "Street", employee.WorkContactInfoId);
            ViewBag.BranchId = new SelectList(db.Branches, "ID", "BranchName", employee.BranchId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = (Employee) db.People.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Employee employee = (Employee) db.People.Find(id);
            db.People.Remove(employee);
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
