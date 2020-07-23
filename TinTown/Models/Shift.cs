using System;
using System.Collections.Generic;

namespace TinTown.Models
{
    public class Shift
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime startDatetime { get; set; }
        public DateTime endDatetime { get; set; }
        public List<string> supervisor { get; set; }
        public int LocationId { get; set; }
    }
}
