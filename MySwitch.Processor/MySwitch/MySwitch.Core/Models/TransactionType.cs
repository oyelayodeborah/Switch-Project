using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySwitch.Core.Models
{
   public class TransactionType:Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
       
    }
}
