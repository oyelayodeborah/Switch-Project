using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySwitch.Core.Models
{
    public class Entity
    {
        public int Id { get; set; }

        [Display(Name = "Date Created")]

        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]

        public DateTime DateModified { get; set; }
    }
}
