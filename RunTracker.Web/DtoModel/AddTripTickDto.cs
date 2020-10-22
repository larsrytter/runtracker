using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTracker.DtoModel
{
    public class AddTripTickDto
    {
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public decimal? Altitude { get; set; }
    }
}
