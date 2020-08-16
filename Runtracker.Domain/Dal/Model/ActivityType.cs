using NpgsqlTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runtracker.Domain.Dal.Model
{
    [Table("activity_type")]
    public class ActivityType
    {
        [PgName("id")]
        public long Id { get; set; }
        [PgName("name")]
        public string Name { get; set; }
    }
}
