using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySwitch.Core.Models;

namespace MySwitch.ViewModels
{
    public class SwitchViewModels
    {
        public IEnumerable<Route> Routes { get; set; }
        public IEnumerable<Combo> Combos { get; set; }
        public IEnumerable<TransactionType> TransactionTypes { get; set; }
        public IEnumerable<Channel> Channels { get; set; }
        public IEnumerable<Fee> Fees { get; set; }
        public IEnumerable<SinkNode> SinkNodes { get; set; }

    }
}