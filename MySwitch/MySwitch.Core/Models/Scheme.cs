using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySwitch.Core.Models
{
    public class Scheme:Entity
    {
        public string Name { get; set; }
        public virtual Route Route { get; set; }
        [Display(Name = "Route")]

        public int RouteId { get; set; }
        public virtual Combo Combo{ get; set; }
        [Display(Name = "Transaction Type - Channel-Fee Combo")]

        public int ComboId { get; set; }

        public string Description { get; set; }

        public IEnumerable<Route> Routes { get; set; }
        public IEnumerable<Combo> Combos { get; set; }

    }
}
