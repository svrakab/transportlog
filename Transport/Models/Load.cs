//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Transport.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Load
    {
        public int ID { get; set; }
        public int LoadNumber { get; set; }
        public int NumberOfPallets { get; set; }
        public System.DateTime PlannedTime { get; set; }
        public Nullable<System.DateTime> ArivalTime { get; set; }
        public Nullable<System.DateTime> DockOn { get; set; }
        public Nullable<System.DateTime> DockOff { get; set; }
        public System.DateTime DepartureTime { get; set; }
        public int IDStatus { get; set; }
        public int IDLoadType { get; set; }
        public int IDCustomers { get; set; }
        public int IDDocks { get; set; }
        public string IDUser { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Dock Dock { get; set; }
        public virtual LoadType LoadType { get; set; }
        public virtual Status Status { get; set; }
    }
}
