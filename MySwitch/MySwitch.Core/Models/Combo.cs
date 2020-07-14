using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MySwitch.Core.Models
{
    public class Combo:Entity
    {
        public string Name { get; set; }
        public TransactionType TransactionType { get; set; }
        [Display(Name = "Transaction Type")]

        public int TransactionTypeId { get; set; }
        public Channel Channel { get; set; }
        [Display(Name = "Channel")]

        public int ChannelId { get; set; }
        public Fee Fee { get; set; }
        [Display(Name = "Fee")]

        public int FeeId { get; set; }
        public IEnumerable<TransactionType> TransactionTypes { get; set; }
        public IEnumerable<Channel> Channels { get; set; }
        public IEnumerable<Fee> Fees { get; set; }


    }
}
