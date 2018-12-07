using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;



namespace Transport.Models

{

    public partial class LoadViewModel

    {

        public int Id { get; set; }

        public IEnumerable<Models.Load> LoadList { get; set; }

        public IEnumerable<Models.Dock> DockList { get; set; }

    }

}