using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySwitch.Core.Models
{
    public class FinancialInstitution:Entity
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        public SinkNode SinkNode { get; set; }
        [Display(Name = "Sink Node")]

        public int SinkNodeId { get; set; }

        public string InstitutionCode { get; set; }

        public IEnumerable<SinkNode> SinkNodes { get; set; }

    }
}
