using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySwitch.Core.Models
{
    public class Route:Entity
    {
        public string Name { get; set; }
        public SinkNode SinkNode { get; set; }
        [Display(Name = "Sink Node")]

        public int SinkNodeId { get; set; }
        [Display(Name = "Card PAN")]

        public string CardPAN { get; set; }
        public string Description { get; set; }
        public IEnumerable<SinkNode> SinkNodes { get; set; }
        
    }
}
