using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySwitch.Core.Models
{
    public enum Status
    {
        [Display(Name="Active")]
        Active,

        [Display(Name = "InActive")]
        InActive
    }
    public class SinkNode:Node
    {
       
    }
}
