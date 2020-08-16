using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTracker.DtoModel
{
    public class TripDto
    {
        public Guid TripGuid { get; set; }
        //public Guid UserGuid { get; set; }
        public long ActivityTypeId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
    }
}
