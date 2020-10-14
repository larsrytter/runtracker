using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTracker.Web.DtoModel
{
    public class TripExtendedDto
    {
        public Guid TripGuid { get; set; }
        //public Guid UserGuid { get; set; }
        public long ActivityTypeId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string Wkt { get; set; }
    }
}
