using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transport.Models
{
    public class Dropdowns
    {
        public List<Generic> Statuses { get; set; }
        public List<Generic> Customers { get; set; }
        public List<Generic> LoadTypes { get; set; }
    }
}