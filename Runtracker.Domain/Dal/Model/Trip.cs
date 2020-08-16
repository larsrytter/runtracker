using DapperExtensions;
using NpgsqlTypes;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Numerics;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using Dapper;
using DapperExtensions.Mapper;

namespace Runtracker.Domain.Dal.Model
{
    [Table("trip")]
    public class Trip
    {
        public long Id { get; set; }
        public Guid TripGuid { get; set; }
        public long UserId { get; set; }
        public long ActivityTypeId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        
    }

    public class TripMapper : ClassMapper<Trip>
    {
        public TripMapper()
        {
            Table("trip");
            Map(m => m.Id).Column("id").Key(KeyType.Identity);
            Map(m => m.TripGuid).Column("trip_guid");
            Map(m => m.UserId).Column("user_id");
            Map(m => m.TimeStart).Column("time_start");
            Map(m => m.TimeEnd).Column("time_end");
            Map(m => m.ActivityTypeId).Column("activity_type_id");
        }
    }

}
