using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transport.Models
{
    public class LoadGeneric
    {
        public int LoadNumber { get; set; }
        public int NumberOfPallets { get; set; }
        public DateTime startDate { get; set; }
        public DateTime? ArivalTime { get; set; }
        public DateTime? DockOn { get; set; }
        public DateTime? DockOff { get; set; }
        public DateTime? endDate { get; set; }
        public int IDStatus { get; set; }
        public int IDLoadType { get; set; }
        public int IDCustomers { get; set; }
        public int IDDocks { get; set; }
    }
}