using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservation.Data
{
    class Billing
    {
        [Required(ErrorMessage = "BarCharge must have an entry")]
        [Display(Name = "Enter Bar cost for guest")]
        public int BarCharge { get; set; }

        [Required(ErrorMessage ="Wificharge must have an entry")]
        [Display(Name = "Enter Wifi cost for guest")]
        public int WifiCharge { get; set; }

        [Required(ErrorMessage = "Room Cost must be over $60")]
        [Display(Name = "Please Select a Guest To Check Out")]
        public int RoomCost { get; set; }

        [Required]
        public int TotalCost { get; set; }

    }
}
