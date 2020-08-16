using DapperExtensions.Mapper;
using NetTopologySuite.Geometries;
using System;

namespace Runtracker.Domain.Dal.Model
{
    public class TripTick
    {
        public long Id { get; set; }
        public long TripId { get; set; }
        public int Order { get; set; }
        public DateTime TimeCreated { get; set; }
        public Point Geom { get; set; } 
    }

    public class TripTickMapper : ClassMapper<TripTick>
    {
        public TripTickMapper()
        {
            Table("trip_tick");
            Map(m => m.Id).Column("id");
            Map(m => m.Order).Column("order");
            Map(m => m.Geom).Column("geom");
            Map(m => m.TimeCreated).Column("time_created");
            Map(m => m.TripId).Column("trip_id");
        }
    }
}
