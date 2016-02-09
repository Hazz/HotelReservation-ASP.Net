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
    public class BookingsController : Controller
    {
        private Hotel_Reservation_SystemEntities db = new Hotel_Reservation_SystemEntities();

        // GET: Bookings
        public async Task<ActionResult> Index()
        {
            var bookings = db.Bookings.Include(b => b.Room);
            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.RoomIDFK = new SelectList(db.Rooms, "RoomID", "SingleBeds");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BookingID,RoomIDFK,BookingDate,BookingFrom,BookingTo,NumOfGuests,RoomBooked,CheckedIn,CheckedOut,RoomCost")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RoomIDFK = new SelectList(db.Rooms, "RoomID", "SingleBeds", booking.RoomIDFK);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomIDFK = new SelectList(db.Rooms, "RoomID", "SingleBeds", booking.RoomIDFK);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BookingID,RoomIDFK,BookingDate,BookingFrom,BookingTo,NumOfGuests,RoomBooked,CheckedIn,CheckedOut,RoomCost")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RoomIDFK = new SelectList(db.Rooms, "RoomID", "SingleBeds", booking.RoomIDFK);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Booking booking = await db.Bookings.FindAsync(id);
            db.Bookings.Remove(booking);
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
