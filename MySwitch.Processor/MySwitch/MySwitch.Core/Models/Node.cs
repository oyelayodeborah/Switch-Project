using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySwitch.Core.Models
{
    
    public class Node:Entity
    {

        public string Name { get; set; }

        [Display(Name = "Host Name")]
        public string HostName { get; set; }

        [Display(Name = "IP Address")]
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public Status Status { get; set; }      //node status

        [Display(Name = "Node Type")]
        public NodeType NodeType { get; set; }

        
    }
}
