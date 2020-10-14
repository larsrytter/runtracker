using System;
using System.Collections.Generic;
using System.Text;

namespace Runtracker.Domain.Dal.Model
{
    public class TripExtended
    {
        public long Id { get; set; }
        public Guid TripGuid { get; set; }
        public long UserId { get; set; }
        public long ActivityTypeId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string Wkt { get; set; }
    }
}
