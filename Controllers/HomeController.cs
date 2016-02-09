using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using HotelReservation.Data;

namespace HotelReservation.Controllers
{
    public class HomeController : Controller
    {
     



        public ActionResult ViewGuests()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Index() // sets the defualy properties 
        {
            
                FreeRooms FreeRoom = new FreeRooms();

        // You can set default properties here (e.g. model.GuestNumbers = 2;)
        FreeRoom.BookingFrom = DateTime.Today.Date;
            FreeRoom.BookingTo = DateTime.Today.AddDays(1).Date;
            FreeRoom.GuestNumbers = "1";
            //FreeRoom.RoomNum = " ";
            FreeRoom.Name = "";
            FreeRoom.Email = "";
            FreeRoom.Add();
            FreeRoom.RoomDetails();
            FreeRoom.combinelist();
            ViewBag.error = " Now";

            return View(FreeRoom); //sends the FreeRoom class to the form
        }

        [HttpPost] //run this method when its posting back information
        public ActionResult Index(FreeRooms FreeRoom, FreeRooms model)
        {

            if (!ModelState.IsValid)
            {
                ////mvc automatically checkes the state of any values coming in
                //                // return the view to correct validation errors
                ViewBag.error = " Error";
                FreeRoom.BookingFrom = DateTime.Today.Date;
                FreeRoom.BookingTo = DateTime.Today.Date.AddDays(1).Date;
                FreeRoom.GuestNumbers = "1";

                return View(FreeRoom);
            }
            //sets the values if there are no errors

          //  FreeRoom.BookingFrom = model.BookingFrom;
          //  FreeRoom.BookingTo = model.BookingTo2;
         //   FreeRoom.GuestNumbers = model.GuestNumbers;
                FreeRoom.Name = model.Name;
                FreeRoom.Email = model.Email;
                //FreeRoom.RoomNum = model.RoomNum;
                //FreeRoom.CostOfRoom = model.CostOfRoom;
             
               FreeRoom.Add();
               FreeRoom.RoomDetails();
               FreeRoom.combinelist();
                ViewBag.error = " Working";
         
            
            if (!string.IsNullOrWhiteSpace(FreeRoom.Name) && !string.IsNullOrWhiteSpace(FreeRoom.Email) && !string.IsNullOrWhiteSpace(model.RoomNumandCost)) // checks if there is text in name & email and if a room number is selected fore sending data
            {
                FreeRoom.NewBooking(); //need to add the booking before the guest
                FreeRoom.NewGuest();

            }

            return View(FreeRoom);
        }
          
        }
          
    }
