using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace TinTown.Models
{
    public class Roster
    {
        public List<RosterHeader> Header { get; set; }
        public List<RosterLine> Line { get; set; }
        public List<Movement> movement { get; set; }
    }
    public class RosterLine
    {
        public string PickingZone { get; set; }
        public string Pick { get; set; }
    }
    public class Movement
    {
        public string zone { get; set; }
        public string picker_email { get; set; }

    }
    public class RosterHeader
    {
        public string PickingZone { get; set; }
        public int NoOfWorker { get; set; }
        public string CreatedBy { get; set; }
        public string WorkType { get; set; }
        public int Shift { get; set; }
        public int IsDefault { get; set; }
        public int LocationId { get; set; }
    }


    public class ReturnMessage
    {
        public JArray data { get; set; }
        public List<SendResponse> sendResponse { get; set; }
    }
}
