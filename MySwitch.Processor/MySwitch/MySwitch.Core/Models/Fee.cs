using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySwitch.Core.Models
{
    public class Fee:Entity
    {
        public string Name { get; set; }

        [Display(Name = "Flat Amount")]

        public string FlatAmount { get; set; }

        [Display(Name="Percentage Of Transaction")]
        public string PercentOfTransaction { get; set; }
        public string Maximum { get; set; }
        public string Minimum { get; set; }
       
    }
}
