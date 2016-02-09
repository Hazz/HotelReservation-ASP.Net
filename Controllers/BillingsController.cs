using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelReservation.Models;

namespace HotelReservation.Controllers
{
    public class BillingsController : Controller
    {
        private Hotel_Reservation_SystemEntities db = new Hotel_Reservation_SystemEntities();

        // GET: Billings
        public async Task<ActionResult> Index()
        {
            var billings = db.Billings.Include(b => b.Guest);
            return View(await billings.ToListAsync());
        }

        // GET: Billings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Billing billing = await db.Billings.FindAsync(id);
            if (billing == null)
            {
                return HttpNotFound();
            }
            return View(billing);
        }

        // GET: Billings/Create
        public ActionResult Create()
        {
            ViewBag.GuestIDFK = new SelectList(db.Guests, "GuestID", "Name");
            return View();
        }

        // POST: Billings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BillingID,GuestIDFK,BarCharge,WifiCharge,RoomCharge,TotalCharge")] Billing billing)
        {
            if (ModelState.IsValid)
            {
                db.Billings.Add(billing);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.GuestIDFK = new SelectList(db.Guests, "GuestID", "Name", billing.GuestIDFK);
            return View(billing);
        }

        // GET: Billings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Billing billing = await db.Billings.FindAsync(id);
            if (billing == null)
            {
                return HttpNotFound();
            }
            ViewBag.GuestIDFK = new SelectList(db.Guests, "GuestID", "Name", billing.GuestIDFK);
            return View(billing);
        }

        // POST: Billings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BillingID,GuestIDFK,BarCharge,WifiCharge,RoomCharge,TotalCharge")] Billing billing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(billing).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.GuestIDFK = new SelectList(db.Guests, "GuestID", "Name", billing.GuestIDFK);
            return View(billing);
        }

        // GET: Billings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Billing billing = await db.Billings.FindAsync(id);
            if (billing == null)
            {
                return HttpNotFound();
            }
            return View(billing);
        }

        // POST: Billings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Billing billing = await db.Billings.FindAsync(id);
            db.Billings.Remove(billing);
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
