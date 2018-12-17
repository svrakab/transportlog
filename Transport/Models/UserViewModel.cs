using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transport.Models
{
    public class UserViewModel 
    {
        public string ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string StreetNumber { get; set; }
        public string City { get; set; }

        public Nullable<int> IDCountry { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string RoleName { get; set; }
        public string RoleID { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }
    }
}