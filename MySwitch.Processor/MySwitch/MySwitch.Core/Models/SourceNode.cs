using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySwitch.Core.Models
{
    public enum NodeType
    {
        Server, Client
    }

    public class SourceNode:Node
    {
        
        public Scheme Scheme { get; set; }
        [Display(Name = "Scheme")]

        public int SchemeId { get; set; }
        public IEnumerable<Scheme> Schemes { get; set; }
        
    }
}
