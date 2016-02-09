using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using HotelReservation.Controllers;
using HotelReservation.Models;
using Microsoft.Ajax.Utilities;

namespace HotelReservation.Data
{
    public class FreeRooms
    {

        [Required] //have to have data entered
        [DataType(DataType.Date)]
        [Display(Name = "Select the Date you want the Booking From")]
        public DateTime BookingFrom { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Select the Date you want the Booking To")]
        public DateTime BookingTo { get; set; }

        public DateTime BookingTo2 => BookingTo;


        public string GuestNumbers { get; set; }

        public string Name { get; set; }


        [DataType(DataType.EmailAddress)]
        [Display(Name = "Please enter your email address")]
        public string Email { get; set; }

        private string _roomcost;

        private string _roomSelected;

        public string RoomNumandCost // gets and sets room cost
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_roomSelected) && _roomSelected.Length != 1)
                {
                    string[] data = _roomSelected.Split(' ');
                    _roomSelected = data[2];
                    _roomcost = data[4];

                }
                return _roomSelected;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    {

                        _roomSelected = value;
                        _roomcost = value;
                    }
                }
                else
                {
                    _roomSelected = " ";
                    _roomcost = " ";
                }

            }
        }


public IEnumerable Add() // works out what rooms are free and returns them in a Table
        {
            
           DateTime startdate = BookingFrom;
           DateTime enddate = BookingTo;

            using (var context = new Hotel_Reservation_SystemEntities())


            {
                var bookings = context.Bookings.ToList()
                    .Where(b => 
                        (startdate >= Convert.ToDateTime(b.BookingFrom) &&
                         enddate <= Convert.ToDateTime(b.BookingTo) ||
                        startdate <= Convert.ToDateTime(b.BookingFrom) &&
                         Convert.ToDateTime(b.BookingTo) <= enddate));

                if (bookings != null)
                {
                    var bookingList = bookings.ToList();
                    var theRooms = context.Rooms.ToList();
                 
                    FreeRoomDetails = theRooms;

                    for (var i = 0; i < theRooms.Count; i++)
                    {
                        for (var j = 0; j < bookingList.Count; j++)
                        {
                            if (theRooms[i].RoomID == bookingList[j].RoomIDFK)
                                theRooms.Remove(theRooms[i]);
                        }
                    }

                    var dgvRooms = from r in theRooms
                                   select
                                       new
                                       {
                                           r.RoomID,
                                           r.SingleBeds,
                                           r.DoubleBeds,
                                           r.Tarrif1Person,
                                           r.Tarrif2People,
                                           r.TarrifExtraPerson
                                       };
                    //  MessageBox.Show("There Are " + Convert.ToInt16(theRooms.Count) + " " + "  Rooms Available");
                    
                    return dgvRooms.ToList();
                }
            }
            return new List<string>();

        }
        public static List<Room> FreeRoomDetails; // a list of all the free rooms so we can calculate costs
         
public List<string> roomcosts = new List<string>(); // creates a list of all free rooms and their cost

        public List<string> RoomDetails() // works out the cost of each room depending on how many guests are selected
        {
       string Guests = GuestNumbers;
      DateTime startdate = BookingFrom;
     DateTime enddate = BookingTo;

            var inttotaldays = enddate - startdate;
            var days = inttotaldays.Days;
            
            //List<string> combine = new List<string>(); 
            


                
            foreach (var room in FreeRoomDetails)
            {
                int guestsstore = Convert.ToInt16(Guests);
                int doublebeds = Convert.ToInt16(room.DoubleBeds);
                int singlebeds = Convert.ToInt16(room.SingleBeds);
                int price = 0;
                int extrabeds = 2;

                while (doublebeds > 0 && guestsstore >= 2)
                {
                    if (guestsstore == 0)
                    {

                        break;
                    }
                    price += (Convert.ToInt16(room.Tarrif2People) * days);
                    guestsstore -= 2;
                    doublebeds -= 1;
                }

                while (singlebeds > 0)
                {
                    if (guestsstore == 0)
                    {
                        break;
                    }
                    price += (Convert.ToInt16(room.Tarrif1Person) * days);
                    guestsstore -= 1;
                    singlebeds -= 1;
                }
                while (extrabeds > 0)
                {
                    if (guestsstore == 0)
                    {
                        break;
                    }
                    price += (Convert.ToInt16(room.TarrifExtraPerson) * days);
                    guestsstore -= 1;
                    extrabeds -= 1;


                }
                // foreach (var room in FreeRoomDetails)  
                roomcosts.Add(price.ToString());
    ;


                //  yield return mylbx.Items.Add("$" + price + " " + "Room:" + room.RoomID + " " + guestsstore + " Guests need to stay in another room");
            }
            return roomcosts.ToList();
           
  
        }
        
        public void combinelist()//joins the free rooms and the rooms cost if they match up
        { 
            combine.Clear();
         for (int i = 0; i < roomcosts.Count; i++)
            {

                combine.Add("Room #: " + FreeRoomDetails[i].RoomID + " Roomcost: " +
                   roomcosts[i]);
                combine.Sort();
            }
        }
       public static List<string> combine = new List<string>();
      public void NewBooking() // creates a new booking on click of create booking
      {
          using (var context = new Hotel_Reservation_SystemEntities())
          {
              var booking = new Booking();
              booking.BookingFrom = this.BookingFrom.ToString("dd-MM-yyyy");
              booking.BookingTo = this.BookingTo.ToString("dd-MM-yyyy");
              booking.NumOfGuests = this.GuestNumbers;
              booking.BookingDate = DateTime.Today.Date.ToString("dd-MM-yyyy");
             
              booking.RoomBooked = this.RoomNumandCost;
              booking.RoomCost = this._roomcost;
              booking.RoomIDFK = Convert.ToInt32(_roomSelected);
              context.Bookings.Add(booking);
              context.SaveChanges();
          }
      }

      public void NewGuest() // adds a new guest on click of create booking and links it to the rest of the guests booking
      {
          using (var context = new Hotel_Reservation_SystemEntities())
              
            {
      var bookingID = context.Bookings.Select(b => b.BookingID).ToList().LastOrDefault();
                var guest = new Guest
                {

                    Name = this.Name,
                    Email = this.Email,
                    BookingIDFK = bookingID

                };
                context.Guests.Add(guest);
                context.SaveChanges();
            }
      }
    }
}
